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
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsLecturerPageVM : BaseViewModel, IMySessionsPageVM
    {
        #region Properties

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

        public MySessionsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            SelectedSession = new Session();
            MySessions = new ObservableCollection<Session>();
            Groups = new ObservableCollection<Group>();
            Groups.Add(new Group());
            User = (Lecturer)appUser;
            UserRole = Role.Lecturer;
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
    }
}