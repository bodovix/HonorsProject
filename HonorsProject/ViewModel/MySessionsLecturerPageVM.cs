﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsLecturerPageVM : BaseViewModel, IMySessionsPageVM
    {
        #region Properties

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

        public bool IsConfirmationAccepted { get; set; }

        private Session _selectedSession;

        public Session SelectedSession
        {
            get { return _selectedSession; }
            set
            {
                if (value == null)
                    value = new Session();
                //if selected.id == 0 create else update
                FormContext = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedSession = value;

                OnPropertyChanged(nameof(SelectedSession));
            }
        }

        private ObservableCollection<Session> _mySessions;

        public ObservableCollection<Session> MySessions
        {
            get { return _mySessions; }
            set
            {
                _mySessions = value;
                OnPropertyChanged(nameof(MySessions));
            }
        }

        private ObservableCollection<Lecturer> _availableLecturers;

        public ObservableCollection<Lecturer> AvailableLecturers
        {
            get { return _availableLecturers; }

            set
            {
                _availableLecturers = value;
                OnPropertyChanged(nameof(AvailableLecturers));
            }
        }

        private Lecturer _selectedLecturer;

        public Lecturer SelectedLecturer
        {
            get { return _selectedLecturer; }
            set
            {
                _selectedLecturer = value;
                OnPropertyChanged(nameof(SelectedLecturer));
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

        #endregion Properties

        #region CommandProperties

        public NewModeCmd NewModeCmd { get; set; }
        public SaveCmd SaveFormCmd { get; set; }
        public AddLecturerCmd AddLecturerCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }

        #endregion CommandProperties

        public MySessionsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //register commands
            NewModeCmd = new NewModeCmd(this);
            SaveFormCmd = new SaveCmd(this);
            AddLecturerCmd = new AddLecturerCmd(this);
            RemoveEntityCmd = new RemoveEntityCmd(this);
            DeleteCmd = new DeleteCmd(this);
            ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
            GoToEntityCmd = new GoToEntityCmd(this);
            CancelCmd = new CancelCmd(this);
            //initial setup
            UserRole = Role.Lecturer;
            User = (Lecturer)appUser;
            GetAllLecturers();
            SelectedSession = new Session();
            FormContext = FormContext.Create;
            GetAllGroups();
            //initially loads current sessions
            GetAllMyCurrentSessions();
        }

        private void GetAllLecturers()
        {
            try
            {
                AvailableLecturers = new ObservableCollection<Lecturer>();
                List<Lecturer> results = UnitOfWork.LecturerRepo.GetAll().ToList();
                if (results != null)
                {
                    foreach (Lecturer l in results)
                    {
                        AvailableLecturers.Add(l);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        private void GetAllGroups()
        {
            try
            {
                Groups = new ObservableCollection<Group>();
                Groups.Add(new Group());
                List<Group> results;

                results = UnitOfWork.GroupRepository.GetAll().ToList();
                if (results != null)
                {
                    foreach (Group g in results)
                    {
                        Groups.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.GetBaseException().Message, FeedbackType.Error);
            }
        }

        public bool Save()
        {
            ClearFeedback();
            bool result;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewSession(SelectedSession, UnitOfWork);
                    if (result)
                    {
                        UpdateMySessionsList();
                        ShowFeedback($"Successfully created session: {SelectedSession.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to create session. please try again or contact support.", FeedbackType.Error);
                    return result;
                }
                else
                {
                    //Update
                    result = SelectedSession.ValidateSession(UnitOfWork);
                    if (result)
                    {
                        UnitOfWork.Complete();
                        ShowFeedback($"Successfully updated session: {SelectedSession.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to update session: {SelectedSession.Id}.", FeedbackType.Error);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.GetBaseException().Message, FeedbackType.Error);
                return false;
            }
        }

        private void UpdateMySessionsList()
        {
            switch (SubgridContext)
            {
                case SubgridContext.ActiveSessions:
                    GetAllMyCurrentSessions();
                    break;

                case SubgridContext.FutureSessions:
                    GetAllMyFutureSessions();
                    break;

                case SubgridContext.PreviousSessions:
                    GetAllMyPreviousSessions();
                    break;

                default:
                    throw new Exception("Invalid Case statement operation. Please contact support");
            }
        }

        public bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
            bool result = false;
            Session sessionToDelte = objToDelete as Session;
            if (sessionToDelte == null)
            {
                ShowFeedback("No session selected.", FeedbackType.Error);
                return result;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteSessionConfirmation.ToString(), null);
                if (IsConfirmationAccepted)
                {
                    int sId = sessionToDelte.Id;
                    UnitOfWork.SessionRepository.Remove(sessionToDelte);
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                    {
                        UpdateMySessionsList();
                        ShowFeedback($"Successfully deleted Session: {sId}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to delete Session: {sId}.", FeedbackType.Error);
                }

                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public void EnterNewMode()
        {
            FormContext = FormContext.Create;
            SelectedSession = new Session();
        }

        public bool AddLecturer()
        {
            ClearFeedback();
            try
            {
                //is lecturer selected
                if (SelectedLecturer == null)
                    throw new Exception("No lecturer selected.");
                //check if lecture already in session
                if (SelectedSession.Lecturers.Contains(SelectedLecturer))
                    throw new Exception("Lecturer already in session.");
                else
                {
                    SelectedSession.Lecturers.Add(SelectedLecturer);
                    ShowFeedback($"Added lecturer: {SelectedLecturer.Id}.", FeedbackType.Success);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool Remove(BaseEntity entityToRemove)
        {
            ClearFeedback();
            bool result = false;
            Lecturer lecturer = (Lecturer)entityToRemove;
            if (SelectedSession.Lecturers != null)
                //Saving done on update Save button clicked
                if (SelectedSession.Lecturers.Contains(lecturer))
                {
                    int lId = lecturer.Id;
                    result = SelectedSession.Lecturers.Remove(lecturer);
                    if (result)
                        ShowFeedback($"Lecturer {lId} removed from session {SelectedSession.Id}.", FeedbackType.Success);
                    else
                        ShowFeedback($"Failed to remove lecture {lId} from ", FeedbackType.Error);
                    return result;
                }
                else
                {
                    ShowFeedback("Lecturer not found in session.", FeedbackType.Error);
                    return result;
                }
            else
                return result;
        }

        public bool ChangeSubgridContext(SubgridContext subgridContext)
        {
            bool result = false;
            switch (subgridContext)
            {
                case SubgridContext.ActiveSessions:
                    result = GetAllMyCurrentSessions();
                    break;

                case SubgridContext.FutureSessions:
                    result = GetAllMyFutureSessions();
                    break;

                case SubgridContext.PreviousSessions:
                    result = GetAllMyPreviousSessions();
                    break;

                default:
                    ShowFeedback("Sub-grid type not supported. Contact support.", FeedbackType.Error);
                    break;
            }
            return result;
        }

        public bool GetAllMyCurrentSessions()
        {
            try
            {
                SubgridContext = SubgridContext.ActiveSessions;
                MySessions = new ObservableCollection<Session>();
                List<Session> result = User.GetAllMyCurrentSessions(DateTime.Now.Date, UnitOfWork);
                if (result != null)
                {
                    MySessions = new ObservableCollection<Session>(result);
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                    return true;
                }
                else
                {
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool GetAllMyPreviousSessions()
        {
            try
            {
                SubgridContext = SubgridContext.PreviousSessions;
                MySessions = new ObservableCollection<Session>();
                List<Session> result = User.GetAllMyPreviousSessions(DateTime.Now.Date, UnitOfWork);
                if (result != null)
                {
                    MySessions = new ObservableCollection<Session>(result);
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                    return true;
                }
                else
                {
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool GetAllMyFutureSessions()
        {
            try
            {
                SubgridContext = SubgridContext.FutureSessions;
                MySessions = new ObservableCollection<Session>();
                List<Session> result = User.GetAllMyFutureSessions(DateTime.Now.Date, UnitOfWork);

                if (result != null)
                {
                    MySessions = new ObservableCollection<Session>(result);
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                    return true;
                }
                else
                {
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool GoToEntity(BaseEntity entity)
        {
            Mediator.NotifyColleagues(MediatorChannels.GoToThisSession.ToString(), entity);
            return true;
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
                    int selectedId = SelectedSession.Id;
                    UnitOfWork.Reload(SelectedSession);
                    UpdateMySessionsList();
                    SelectedSession = MySessions.Where(S => S.Id == selectedId).FirstOrDefault();
                    OnPropertyChanged(nameof(MySessions));
                    OnPropertyChanged(nameof(SelectedSession));
                }
                catch
                {
                    EnterNewMode();
                    ShowFeedback("Unable to re-load selected Session. \n Going back to new mode...", FeedbackType.Error);

                    return false;
                }
            }
            return true;
        }
    }
}