using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
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

        private int rowLimit;
        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
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

        #endregion Properties

        public MyGroupsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            try
            {
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
    }
}