using System;
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

        private SessionsContext _sessionsContext;

        public SessionsContext SessionsContext
        {
            get { return _sessionsContext; }
            set
            {
                _sessionsContext = value;
                OnPropertyChanged(nameof(SessionsContext));
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

        private Role _userRole;

        public Role UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged(nameof(UserRole));
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
        public RemoveLecturerCmd RemoveLecturerCmd { get; set; }
        public GetActiveSessionsCmd GetActiveSessionsCmd { get; set; }
        public GetFutureSessionsCmd GetFutureSessionsCmd { get; set; }
        public GetPreviousSessionsCmd GetPreviousSessionsCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }

        #endregion CommandProperties

        public MySessionsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //register commands
            NewModeCmd = new NewModeCmd(this);
            SaveFormCmd = new SaveCmd(this);
            AddLecturerCmd = new AddLecturerCmd(this);
            RemoveLecturerCmd = new RemoveLecturerCmd(this);
            GetActiveSessionsCmd = new GetActiveSessionsCmd(this);
            GetFutureSessionsCmd = new GetFutureSessionsCmd(this);
            GetPreviousSessionsCmd = new GetPreviousSessionsCmd(this);
            DeleteCmd = new DeleteCmd(this);

            //initial setup
            User = (Lecturer)appUser;
            GetAllLecturers();
            SelectedSession = new Session();
            UserRole = Role.Lecturer;
            FormContext = FormContext.Create;
            GetAllGroups(dbcontextName);
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
                FeedbackMessage = ex.Message;
            }
        }

        private void GetAllGroups(string dbcontextName)
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
                FeedbackMessage = ex.GetBaseException().Message;
            }
        }

        public bool Save()
        {
            FeedbackMessage = "";
            bool result;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewSession(SelectedSession, UnitOfWork);
                    if (result)
                        UpdateMySessionsList();
                    return result;
                }
                else
                {
                    //Update
                    result = SelectedSession.ValidateSession();
                    if (result)
                        UnitOfWork.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.GetBaseException().Message;
                return false;
            }
        }

        private void UpdateMySessionsList()
        {
            switch (SessionsContext)
            {
                case SessionsContext.Active:
                    GetAllMyCurrentSessions();
                    break;

                case SessionsContext.Future:
                    GetAllMyFutureSessions();
                    break;

                case SessionsContext.Previous:
                    GetAllMyPreviousSessions();
                    break;

                default:
                    throw new Exception("Invalid Case statement operation. Please contact support");
            }
        }

        public bool Delete(object objToDelete)
        {
            Session sessionToDelte = objToDelete as Session;
            if (sessionToDelte == null)
            {
                FeedbackMessage = "No session selected.";
                return false;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteSessionConfirmation.ToString(), null);
                if (IsConfirmationAccepted)
                {
                    UnitOfWork.SessionRepository.Remove(sessionToDelte);
                    UnitOfWork.Complete();
                    UpdateMySessionsList();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public void EnterNewMode()
        {
            FormContext = FormContext.Create;
            SelectedSession = new Session();
        }

        public bool GetAllMyCurrentSessions()
        {
            try
            {
                SessionsContext = SessionsContext.Active;
                MySessions = new ObservableCollection<Session>();
                List<Session> result = User.GetAllMyCurrentSessions(DateTime.Now.Date, UnitOfWork);
                if (result != null)
                {
                    MySessions = new ObservableCollection<Session>(result);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public bool GetAllMyPreviousSessions()
        {
            try
            {
                SessionsContext = SessionsContext.Previous;
                MySessions = new ObservableCollection<Session>();
                List<Session> result = User.GetAllMyPreviousSessions(DateTime.Now.Date, UnitOfWork);
                if (result != null)
                {
                    MySessions = new ObservableCollection<Session>(result);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public bool GetAllMyFutureSessions()
        {
            try
            {
                SessionsContext = SessionsContext.Future;
                MySessions = new ObservableCollection<Session>();
                List<Session> result = User.GetAllMyFutureSessions(DateTime.Now.Date, UnitOfWork);
                if (result != null)
                {
                    MySessions = new ObservableCollection<Session>(result);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public bool AddLecturer()
        {
            FeedbackMessage = "";
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
                    return true;
                }
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public bool RemoveLecturer(Lecturer lecturer)
        {
            FeedbackMessage = "";
            if (SelectedSession.Lecturers != null)
                //Saving done on update Save button clicked
                if (SelectedSession.Lecturers.Contains(lecturer))
                {
                    SelectedSession.Lecturers.Remove(lecturer);
                    return true;
                }
                else
                {
                    FeedbackMessage = "Lecturer not found in session.";
                    return false;
                }
            else
                return false;
        }
    }
}