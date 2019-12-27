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
    public class MyGroupsStudentPageVM : BaseViewModel, IMyGroupsPageVM
    {
        #region Properties

        public int RowLimit { get; set; }
        public bool IsConfirmed { get; set; }

        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
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
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public SaveCmd SaveFormCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MoveEntityOutOfListCmd MoveEntityOutOfListCmd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MoveEntityInToListCmd MoveEntityInToListCmd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion Properties

        public MyGroupsStudentPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //Commands
            //Commands
            SaveFormCmd = new SaveCmd(this);
            NewModeCmd = new NewModeCmd(this);
            ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void EnterNewMode()
        {
            throw new NotImplementedException();
        }

        public bool ChangeSubgridContext(SubgridContext context)
        {
            throw new NotImplementedException();
        }

        public bool Delete(BaseEntity objToDelete)
        {
            throw new NotImplementedException();
        }

        public bool Remove(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool MoveEntityOutOfList(BaseEntity entityToRemove)
        {
            throw new NotImplementedException();
        }

        public bool MoveEntityInToList(BaseEntity entityToAdd)
        {
            throw new NotImplementedException();
        }
    }
}