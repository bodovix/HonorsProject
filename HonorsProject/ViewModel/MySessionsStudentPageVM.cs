using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsStudentPageVM : BaseViewModel, IMySessionsPageVM
    {
        #region Properties

        public bool IsConfirmationAccepted { get; set; }

        private FormContext formContext;

        public FormContext FormContext
        {
            get { return formContext; }
            set
            {
                formContext = value;
                OnPropertyChanged(nameof(FormContext));
            }
        }

        private SubgridContext _sessionsContext;

        public SubgridContext SubgridContext
        {
            get { return _sessionsContext; }
            set
            {
                _sessionsContext = value;
                OnPropertyChanged(nameof(SubgridContext));
            }
        }

        private ISystemUser _user;

        public ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Student)value;
                OnPropertyChanged(nameof(User));
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

        #region Commands

        public NewModeCmd NewModeCmd { get; set; }
        public SaveCmd SaveFormCmd { get; set; }
        public AddLecturerCmd AddLecturerCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }

        #endregion Commands

        public MySessionsStudentPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
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
            User = (Student)appUser;
            UserRole = Role.Student;

            GetAllLecturers();
            SelectedSession = new Session();
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

        public bool Save()
        {
            throw new NotImplementedException("Students Cannot Save Sessions.");
        }

        public bool Delete(BaseEntity objectToDelete)
        {
            FeedbackMessage = "Students Cannot Delete Sessions";
            return false;
        }

        public void EnterNewMode()
        {
            throw new NotImplementedException("Students Cannot Create New Sessions");
        }

        public bool AddLecturer()
        {
            throw new NotImplementedException("Students cannot add lecturers to sessions");
        }

        public bool Remove(BaseEntity entityToRemove)
        {
            throw new NotImplementedException("Students cannot remove lecturers from sessions");
        }

        public bool ChangeSubgridContext(SubgridContext subgridContext)
        {
            bool result = false;
            switch (subgridContext)
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
                    FeedbackMessage = "Sub-grid type not supported. Contact support.";
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
                FeedbackMessage = ex.Message;
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
                FeedbackMessage = ex.Message;
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
                FeedbackMessage = ex.Message;
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
                    FeedbackMessage = "Unable to re-load selected Session. \n Going back to new mode...";
                    return false;
                }
            }
            return true;
        }
    }
}