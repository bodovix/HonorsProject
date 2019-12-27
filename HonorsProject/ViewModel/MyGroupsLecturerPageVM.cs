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
            set { _filteredSessions = value;
                OnPropertyChanged(nameof(FilteredSessions));
            }
        }
        private Session _selectedSession;

        public Session SelectedSession
        {
            get { return _selectedSession; }
            set { _selectedSession = value;
                OnPropertyChanged(nameof(SelectedSession));
            }
        }
        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedSession));
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
        #endregion

        public MyGroupsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //Commands
            SaveFormCmd = new SaveCmd(this);
            NewModeCmd = new NewModeCmd(this);
            ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
            DeleteCmd = new DeleteCmd(this);
            RemoveEntityCmd = new RemoveEntityCmd(this);
            //Initial Setup
            try
            {
                IsConfirmed = false;
                User = (Lecturer)appUser;
                RowLimit = 10;
                SelectedGroup = new Group();
                FormContext = FormContext.Create;
                SubgridContext = SubgridContext.Students;
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
            FeedbackMessage = "";
            bool result = false;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewGroup(SelectedGroup, UnitOfWork);
                    UpdateMyGroupsList(RowLimit);
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

        public bool Delete(BaseEntity objToDelete)
        {
            FeedbackMessage = "";
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
                FeedbackMessage = $"Cannot delete object of type {objToDelete.GetType().ToString()}. Please contact support.";
                result = false;
            }
            return result;
        }

        private bool DeleteSession(object objToDelete)
        {
            Session sessionToDelete = objToDelete as Session;
            if (sessionToDelete == null)
            {
                FeedbackMessage = "No session selected.";
                return false;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteSessionConfirmation.ToString(), sessionToDelete);
                if (IsConfirmed)
                {
                    UnitOfWork.SessionRepository.Remove(sessionToDelete);
                    int count = UnitOfWork.Complete();
                    if (count > 0)
                    {
                        UpdateMyGroupsList(RowLimit);
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    FeedbackMessage = "Delete canceled.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        private bool DeleteGroup(object objToDelete)
        {
            Group groupToDelete = objToDelete as Group;
            if (groupToDelete == null)
            {
                FeedbackMessage = "No group selected.";
                return false;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteGroupConfirmation.ToString(), groupToDelete);
                if (IsConfirmed)
                {
                    UnitOfWork.GroupRepository.Remove(groupToDelete);
                    int count = UnitOfWork.Complete();
                    if (count > 0)
                    {
                        UpdateMyGroupsList(RowLimit);
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

        public bool ChangeSubgridContext(SubgridContext context)
        {
            bool result = true;
            SubgridContext = context;

            switch (context)
            {
                case SubgridContext.ActiveSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetCurrentSessions(SelectedGroup, DateTime.Now.Date));
                    Mediator.NotifyColleagues(MediatorChannels.LoadActiveSessionsSubgrid.ToString(),null);
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
                    throw new NotImplementedException("Ansers subgrid not required. Contact Support.");
                case SubgridContext.Students:
                    //update the view to show Students Subgrid
                    Mediator.NotifyColleagues(MediatorChannels.LoadStudentsSubgrid.ToString(), null);
                    break;
                default:
                    break;
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
            throw new NotImplementedException();
        }
    }
}