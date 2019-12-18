using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class MyGroupsLecturerPageVM : BaseViewModel, IMyGroupsPageVM
    {
        #region Properties

        public bool IsConfirmed { get; set; }
        private int rowLimit;
        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (value == null)
                    value = new Group();
                //if selected.id == 0 create else update
                FormContext = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedGroup = value;

                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        private ObservableCollection<Group> _groups;

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }

        private string _groupSearchTxt;

        public string GroupSearchTxt
        {
            get { return _groupSearchTxt; }
            set
            {
                _groupSearchTxt = value;
                OnPropertyChanged(nameof(GroupSearchTxt));
            }
        }

        private FormContext _formContext;

        public FormContext FormContext
        {
            get { return _formContext; }
            set
            {
                _formContext = value;
                OnPropertyChanged(nameof(FormContext));
            }
        }

        private SubgridContext _subgridContext;

        public SubgridContext SubgridContext
        {
            get { return _subgridContext; }
            set
            {
                _subgridContext = value;
                OnPropertyChanged(nameof(SubgridContext));
            }
        }

        private ISystemUser _user;

        public ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Lecturer)value;
                OnPropertyChanged(nameof(User));
            }
        }

        public SaveCmd SaveFormCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }

        #endregion Properties

        public MyGroupsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //Commands
            SaveFormCmd = new SaveCmd(this);
            NewModeCmd = new NewModeCmd(this);
            ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
            DeleteCmd = new DeleteCmd(this);
            //Initial Setup
            try
            {
                IsConfirmed = false;
                User = (Lecturer)appUser;
                rowLimit = 10;
                SelectedGroup = new Group();
                FormContext = FormContext.Create;
                SubgridContext = SubgridContext.Students;
                GroupSearchTxt = "";
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTop(rowLimit).ToList());
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
            }
        }

        public bool Save()
        {
            FeedbackMessage = "";
            bool result = false;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewGroup(SelectedGroup, UnitOfWork);
                    UpdateMyGroupsList();
                }
                else
                {
                    //Update
                    result = SelectedGroup.ValidateGroup();
                    if (result)
                        UnitOfWork.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.GetBaseException().Message;
                return false;
            }
        }

        public void EnterNewMode()
        {
            SelectedGroup = new Group();
            FormContext = FormContext.Create;
        }

        public bool Delete(object objToDelete)
        {
            Group groupToDelete = objToDelete as Group;
            if (groupToDelete == null)
            {
                FeedbackMessage = "No group selected.";
                return false;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteGroupConfirmation.ToString(), null);
                if (IsConfirmed)
                {
                    UnitOfWork.GroupRepository.Remove(groupToDelete);
                    int count = UnitOfWork.Complete();
                    if (count > 0)
                    {
                        UpdateMyGroupsList();
                        return true;
                    }
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        private void UpdateMyGroupsList()
        {
            Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTopXFromSearch(GroupSearchTxt, rowLimit));
        }

        public bool ChangeSubgridContext(SubgridContext context)
        {
            throw new NotImplementedException();
        }
    }
}