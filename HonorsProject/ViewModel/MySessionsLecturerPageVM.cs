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

        #endregion CommandProperties

        public MySessionsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //register commands
            NewModeCmd = new NewModeCmd(this);
            SaveFormCmd = new SaveCmd(this);
            //initial setup
            User = (Lecturer)appUser;
            UserRole = Role.Lecturer;
            FormContext = FormContext.Create;
            SelectedSession = new Session();
            MySessions = new ObservableCollection<Session>();
            GetAllGroups(dbcontextName);
        }

        private void GetAllGroups(string dbcontextName)
        {
            Groups = new ObservableCollection<Group>();
            Groups.Add(new Group());
            List<Group> results;
            using (UnitOfWork u = new UnitOfWork(new LabAssistantContext(dbcontextName)))
                results = u.GroupRepository.GetAll().ToList();
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
                    return User.AddNewSession(SelectedSession, dbConName);
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
            throw new NotImplementedException();
        }

        public List<Session> GetAllMyPreviousSessions()
        {
            throw new NotImplementedException();
        }

        public List<Session> GetAllMyFutureSessions()
        {
            throw new NotImplementedException();
        }
    }
}