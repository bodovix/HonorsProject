using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsStudentPageVM : BaseViewModel, IMySessionsPageVM
    {
        #region Properties

        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get { return _feedbackMessage; }
            set { _feedbackMessage = value; }
        }

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

        #endregion Commands

        public MySessionsStudentPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            User = (Student)appUser;
            UserRole = Role.Student;
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void EnterNewMode()
        {
            throw new NotImplementedException();
        }
    }
}