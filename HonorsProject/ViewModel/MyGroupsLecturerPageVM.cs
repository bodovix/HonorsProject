﻿using HonorsProject.Model.Core;
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
        public int RowLimit { get; set; }
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
                RefreshAvailableStudents(SelectedGroup);
                ChangeSubgridContext(SubgridContext);//refresh the subgrid content

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

        private ObservableCollection<Session> _filteredSessions;

        public ObservableCollection<Session> FilteredSessions
        {
            get { return _filteredSessions; }
            set
            {
                _filteredSessions = value;
                OnPropertyChanged(nameof(FilteredSessions));
            }
        }

        private Session _selectedSession;

        public Session SelectedSession
        {
            get { return _selectedSession; }
            set
            {
                _selectedSession = value;
                OnPropertyChanged(nameof(SelectedSession));
            }
        }

        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedSession));
            }
        }

        private ObservableCollection<Student> _studentsNotInGroup;

        public ObservableCollection<Student> StudentsNotInGroup
        {
            get { return _studentsNotInGroup; }
            set
            {
                _studentsNotInGroup = value;
                OnPropertyChanged(nameof(StudentsNotInGroup));
            }
        }

        private string _groupSearchTxt;

        public string GroupSearchTxt
        {
            get { return _groupSearchTxt; }
            set
            {
                _groupSearchTxt = value;
                UpdateMyGroupsList(RowLimit);
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

        #endregion Properties

        #region Commands

        public SaveCmd SaveFormCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public MoveEntityOutOfListCmd MoveEntityOutOfListCmd { get; set; }
        public MoveEntityInToListCmd MoveEntityInToListCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }

        #endregion Commands

        public MyGroupsLecturerPageVM(ISystemUser appUser, Group selectedGroup, string dbcontextName) : base(dbcontextName)
        {
            //Commands
            SaveFormCmd = new SaveCmd(this);
            NewModeCmd = new NewModeCmd(this);
            ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
            DeleteCmd = new DeleteCmd(this);
            RemoveEntityCmd = new RemoveEntityCmd(this);
            MoveEntityOutOfListCmd = new MoveEntityOutOfListCmd(this);
            MoveEntityInToListCmd = new MoveEntityInToListCmd(this);
            GoToEntityCmd = new GoToEntityCmd(this);
            CancelCmd = new CancelCmd(this);
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
                FeedbackMessage = ex.Message;
            }
        }

        public bool Save()
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
                    ShowFeedback($"Successfully created: {SelectedGroup.Id}.", FeedbackType.Success);
                }
                else
                {
                    //Update
                    result = SelectedGroup.ValidateGroup();
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

        public void EnterNewMode()
        {
            SelectedGroup = new Group();
            FormContext = FormContext.Create;
        }

        public bool Delete(BaseEntity objToDelete)
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

        public bool ChangeSubgridContext(SubgridContext context)
        {
            bool result = true;
            SubgridContext = context;

            switch (context)
            {
                case SubgridContext.ActiveSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetCurrentSessions(SelectedGroup, DateTime.Now.Date));
                    Mediator.NotifyColleagues(MediatorChannels.LoadActiveSessionsSubgrid.ToString(), null);
                    break;

                case SubgridContext.FutureSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetFutureSessions(SelectedGroup, DateTime.Now.Date));
                    //update the view to show future sessions
                    Mediator.NotifyColleagues(MediatorChannels.LoadFutureSessionsSubgrid.ToString(), null);
                    break;

                case SubgridContext.PreviousSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetPreviousSessions(SelectedGroup, DateTime.Now.Date));
                    //update the view to show previous
                    Mediator.NotifyColleagues(MediatorChannels.LoadPreviousSessionsSubgrid.ToString(), null);
                    break;

                case SubgridContext.Groups:
                    throw new NotImplementedException("Groups subgrid not required. Contact Support.");
                case SubgridContext.Questions:
                    throw new NotImplementedException("Questions subgrid not required. Contact Support.");
                case SubgridContext.Answers:
                    throw new NotImplementedException("Answers subgrid not required. Contact Support.");
                case SubgridContext.Students:
                    //update the view to show Students Subgrid
                    Mediator.NotifyColleagues(MediatorChannels.LoadStudentsSubgrid.ToString(), null);
                    break;

                default:
                    throw new AggregateException("Invalid subgrid option. Please contact support.");
            }
            return result;
        }

        private void UpdateMyGroupsList(int rows)
        {
            Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTopXFromSearch(GroupSearchTxt, rows));
            ChangeSubgridContext(SubgridContext);//refresh the subgrid content
        }

        public bool Remove(BaseEntity entity)
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

        private void RefreshAvailableStudents(Group group)
        {
            List<Student> students;
            //if (group == null || group.Id == 0) // If in New Mode
            //    students = UnitOfWork.StudentRepo.GetAll().ToList();
            //else // if student already selected
            students = UnitOfWork.StudentRepo.GetStudentsNotInGroup(group).ToList();
            StudentsNotInGroup = new ObservableCollection<Student>(students);
        }

        public bool MoveEntityOutOfList(BaseEntity entityToRemove)
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
                    {
                        RefreshAvailableStudents(SelectedGroup);
                        ShowFeedback($"Student {sId} removed from Group {SelectedGroup.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to remove student {sId} from Group {SelectedGroup.Id}. \n Please try again or contact support.", FeedbackType.Error);
                    RefreshAvailableStudents(SelectedGroup);
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

        public bool MoveEntityInToList(BaseEntity entityToAdd)
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

        public bool GoToEntity(BaseEntity entity)
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

        public bool Cancel()
        {
            ClearFeedback();
            if (FormContext == FormContext.Create)
                EnterNewMode();
            else
            {
                try
                {
                    UnitOfWork.Reload(SelectedGroup);
                    UpdateMyGroupsList(RowLimit);
                    OnPropertyChanged(nameof(SelectedGroup));
                }
                catch
                {
                    EnterNewMode();
                    ShowFeedback("Unable to re-load selected Group. \n Going back to new mode.", FeedbackType.Error);
                    return false;
                }
            }
            return true;
        }
    }
}