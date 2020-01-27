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
    public class MyGroupsStudentPageVM : BaseMyGroupsPageVM
    {
        #region Properties

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Student)value;
                OnPropertyChanged(nameof(User));
            }
        }

        #endregion Properties

        public MyGroupsStudentPageVM(ISystemUser appUser, Group selectedGroup, string dbcontextName) : base(dbcontextName)
        {
            //Initial Setup
            try
            {
                IsConfirmed = false;
                User = (Student)appUser;
                UserRole = Role.Student;
                RowLimit = 10;
                //if group passed into page select it. otherwise go into new mode.
                if (selectedGroup.Id != 0)
                    SelectedGroup = UnitOfWork.GroupRepository.Get(selectedGroup.Id);
                else
                    SelectedGroup = new Group();
                SubgridContext = SubgridContext.Students;
                ChangeSubgridContext(SubgridContext);
                GroupSearchTxt = "";
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetForStudent((Student)User, RowLimit).ToList());
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        public override bool Save()
        {
            ShowFeedback("Students cannot update or create groups.", FeedbackType.Error);
            return false;
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            ShowFeedback("Students cannot delete groups.", FeedbackType.Error);
            return false;
        }

        public override bool Remove(BaseEntity entity)
        {
            ShowFeedback("Students cannot remove students from groups.", FeedbackType.Error);
            return false;
        }

        public override bool MoveEntityOutOfList(BaseEntity entityToRemove)
        {
            ShowFeedback("Students cannot remove students from groups.", FeedbackType.Error);
            return false;
        }

        public override bool MoveEntityInToList(BaseEntity entityToAdd)
        {
            ShowFeedback("Students cannot add students to groups.", FeedbackType.Error);
            return false;
        }

        public override bool GoToEntity(BaseEntity entity)
        {
            if (entity is Session)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToThisSession.ToString(), entity);
                return true;
            }
            else if (entity is Student)
            {
                ShowFeedback("Students cannot view Students profiles.", FeedbackType.Error);
                return true;
            }
            else if (entity is null)
            {
                ShowFeedback("Cannot go to a NULL object.", FeedbackType.Error);
                return false;
            }
            ShowFeedback("Cannot go to an unsupported object type.", FeedbackType.Error);
            return false;
        }
    }
}