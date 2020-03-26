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
    public class MyGroupsLecturerPageVM : BaseMyGroupsPageVM
    {
        #region Properties

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Lecturer)value;
                OnPropertyChanged(nameof(User));
            }
        }

        #endregion Properties

        public MyGroupsLecturerPageVM(ISystemUser appUser, Group selectedGroup, string dbcontextName) : base(dbcontextName)
        {
            //Initial Setup
            try
            {
                IsConfirmed = false;
                User = (Lecturer)appUser;
                UserRole = Role.Lecturer;
                RowLimit = 10;
                //if group passed in load it else start in create mode.
                if (selectedGroup.Id != 0)
                    SelectedGroup = UnitOfWork.GroupRepository.Get(selectedGroup.Id);
                else
                    SelectedGroup = new Group();
                SubgridContext = SubgridContext.Students;
                ChangeSubgridContext(SubgridContext);
                GroupSearchTxt = "";
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTop(RowLimit).ToList());
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        public override bool Save()
        {
            ClearFeedback();
            bool result = false;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewGroup(SelectedGroup, UnitOfWork);
                    UpdateMyGroupsList(RowLimit);
                    FormContext = FormContext.Update;//since selected group now has ID set
                    ShowFeedback($"Successfully created: {SelectedGroup.Id}.", FeedbackType.Success);
                }
                else
                {
                    //Update
                    result = SelectedGroup.ValidateGroup(UnitOfWork);
                    if (result)
                    {
                        UnitOfWork.Complete();
                        ShowFeedback($"Successfully updated: {SelectedGroup.Id}.", FeedbackType.Success);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.GetBaseException().Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
            bool result;
            if (objToDelete is Session)
            {
                //deleting session
                result = DeleteSession(objToDelete);
            }
            else if (objToDelete is Group)
            {
                //Deleting group
                result = DeleteGroup(objToDelete);
            }
            else
            {
                ShowFeedback($"Cannot delete object of type {objToDelete.GetType().ToString()}. Please contact support.", FeedbackType.Error);
                result = false;
            }
            return result;
        }

        private bool DeleteSession(object objToDelete)
        {
            Session sessionToDelete = objToDelete as Session;
            if (sessionToDelete == null)
            {
                ShowFeedback("No session selected.", FeedbackType.Error);
                return false;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteSessionConfirmation.ToString(), sessionToDelete);
                if (IsConfirmed)
                {
                    int id = sessionToDelete.Id;
                    UnitOfWork.SessionRepository.Remove(sessionToDelete);
                    int count = UnitOfWork.Complete();
                    if (count > 0)
                    {
                        UpdateMyGroupsList(RowLimit);
                        ShowFeedback($"Successfully deleted Session: {id}.", FeedbackType.Success);
                        return true;
                    }
                    else
                    {
                        ShowFeedback("Session not deleted.", FeedbackType.Error);
                        return false;
                    }
                }
                else
                {
                    ShowFeedback("Delete canceled.", FeedbackType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        private bool DeleteGroup(object objToDelete)
        {
            Group groupToDelete = objToDelete as Group;
            if (groupToDelete == null)
            {
                ShowFeedback("No group selected.", FeedbackType.Error);
                return false;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteGroupConfirmation.ToString(), groupToDelete);
                if (IsConfirmed)
                {
                    int id = groupToDelete.Id;
                    UnitOfWork.GroupRepository.Remove(groupToDelete);
                    int count = UnitOfWork.Complete();
                    if (count > 0)
                    {
                        UpdateMyGroupsList(RowLimit);
                        ShowFeedback($"Successfully deleted Group: {id}.", FeedbackType.Success);
                        return true;
                    }
                    else
                    {
                        ShowFeedback($"Failed to delete group: {id}.", FeedbackType.Error);
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool Remove(BaseEntity entity)
        {
            bool result = false;
            if (entity == null)
            {
                ShowFeedback("No valid group selected. Canceling.", FeedbackType.Error);
                return false;
            }
            try
            {
                ClearFeedback();
                string msg = "";
                int sId = entity.Id;
                result = SelectedGroup.RemoveStudent((Student)entity, UnitOfWork, ref msg);
                if (result)
                {
                    UpdateMyGroupsList(RowLimit);
                    ShowFeedback($"Removed Student {sId} from Group {SelectedGroup.Id}.", FeedbackType.Success);
                    return true;
                }
                else
                {
                    ShowFeedback(msg, FeedbackType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool MoveEntityOutOfList(BaseEntity entityToRemove)
        {
            ClearFeedback();
            try
            {
                bool result = false;
                if (entityToRemove is Student student)
                {
                    int sId = student.Id;
                    SelectedGroup.Students.Remove(student);
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                        ShowFeedback($"Student {sId} removed from Group {SelectedGroup.Id}.", FeedbackType.Success);
                    else
                        ShowFeedback($"Failed to remove student {sId} from Group {SelectedGroup.Id}. \n Please try again or contact support.", FeedbackType.Error);
                    RefreshAvailableStudents(SelectedGroup);
                    OnPropertyChanged(nameof(SelectedGroup));
                    return true;
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool MoveEntityInToList(BaseEntity entityToAdd)
        {
            ClearFeedback();
            if (FormContext == FormContext.Create)
            {
                ShowFeedback("Create group first.", FeedbackType.Error);
                return false;
            }
            try
            {
                bool result = false;
                if (entityToAdd is Student student)
                {
                    if (SelectedGroup.Students.Contains(entityToAdd))
                    {
                        ShowFeedback($"Student already belongs to group: {entityToAdd.Name}", FeedbackType.Error);
                        return false;
                    }
                    int sId = student.Id;
                    SelectedGroup.Students.Add(student);
                    SelectedGroup.Students = SelectedGroup.Students;
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                    {
                        RefreshAvailableStudents(SelectedGroup);
                        ShowFeedback($"Student {sId} added to Group {SelectedGroup.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to add student {sId} to Group {SelectedGroup.Id}. \n Please try again or contact support.", FeedbackType.Error);
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
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
                Mediator.NotifyColleagues(MediatorChannels.GoToThisStudent.ToString(), entity);
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

        public override bool GoToAnalyseEntity(BaseEntity entity)
        {
            if (entity is Session session)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToAnalyseEntity.ToString(), session);
                return true;
            }
            if (entity is Group group)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToAnalyseEntity.ToString(), group);
                return true;
            }
            ShowFeedback("Cannot go to an unsupported object type.", FeedbackType.Error);
            return false;
        }
    }
}