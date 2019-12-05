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
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsLecturerPageVM : BaseViewModel, IMySessionsPageVM
    {
        #region Properties

        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get { return _feedbackMessage; }
            set
            {
                _feedbackMessage = value;
                OnPropertyChanged(nameof(FeedbackMessage));
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

        private Session _selectedSession;

        public Session SelectedSession
        {
            get { return _selectedSession; }
            set
            {
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

        #endregion CommandProperties

        public MySessionsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //register commands
            NewModeCmd = new NewModeCmd(this);
            SaveFormCmd = new SaveCmd(this);
            AddLecturerCmd = new AddLecturerCmd(this);
            RemoveLecturerCmd = new RemoveLecturerCmd(this);
            //initial setup
            User = (Lecturer)appUser;
            GetAllLecturers();
            UserRole = Role.Lecturer;
            FormContext = FormContext.Create;
            GetAllGroups(dbcontextName);
            SelectedSession = new Session();
            List<Session> sessions = GetAllMyCurrentSessions();
            MySessions = new ObservableCollection<Session>(sessions);
        }

        private void GetAllLecturers()
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

        private void GetAllGroups(string dbcontextName)
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

        public bool Save()
        {
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    bool result = User.AddNewSession(SelectedSession, UnitOfWork);
                    MySessions.Add(SelectedSession);
                    return result;
                }
                else
                {
                    //Update
                    throw new NotImplementedException("Not worked on update yet");
                }
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.GetBaseException().Message;
                return false;
            }
        }

        public void EnterNewMode()
        {
            FormContext = FormContext.Create;
            SelectedSession = new Session();
        }

        public List<Session> GetAllMyCurrentSessions()
        {
            try
            {
                return User.GetAllMyCurrentSessions(UnitOfWork);
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.GetBaseException().Message;
                return null;
            }
        }

        public List<Session> GetAllMyPreviousSessions()
        {
            throw new NotImplementedException();
        }

        public List<Session> GetAllMyFutureSessions()
        {
            throw new NotImplementedException();
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
        }
    }
}