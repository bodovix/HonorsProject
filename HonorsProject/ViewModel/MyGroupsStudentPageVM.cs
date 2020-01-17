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
                if (value == null)
                    value = new Group();
                _selectedGroup = value;
                ChangeSubgridContext(SubgridContext);
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
                _user = (Student)value;
                OnPropertyChanged(nameof(User));
            }
        }

        public SaveCmd SaveFormCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public MoveEntityOutOfListCmd MoveEntityOutOfListCmd { get; set; }
        public MoveEntityInToListCmd MoveEntityInToListCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }

        #endregion Properties

        public MyGroupsStudentPageVM(ISystemUser appUser, Group selectedGroup, string dbcontextName) : base(dbcontextName)
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
                User = (Student)appUser;
                UserRole = Role.Student;
                RowLimit = 10;
                //if group passed int opage select it. otherwise go into new mode.
                if (selectedGroup.Id != 0)
                    SelectedGroup = UnitOfWork.GroupRepository.Get(selectedGroup.Id);
                else
                    SelectedGroup = new Group();
                SubgridContext = SubgridContext.Students;
                ChangeSubgridContext(SubgridContext);
                GroupSearchTxt = "";
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetForStudent((Student)User, RowLimit).ToList());
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        public bool Save()
        {
            ShowFeedback("Students cannot update or create groups.", FeedbackType.Error);
            return false;
        }

        public void EnterNewMode()
        {
            SelectedGroup = new Group();
            FormContext = FormContext.Create;
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
                    throw new NotImplementedException("Ansers subgrid not required. Contact Support.");
                case SubgridContext.Students:
                    //update the view to show Students Subgrid
                    Mediator.NotifyColleagues(MediatorChannels.LoadStudentsSubgrid.ToString(), null);
                    break;

                default:
                    throw new Exception("Invalid subgrid navigation performed. Please contact support.");
            }
            return result;
        }

        public bool Delete(BaseEntity objToDelete)
        {
            ShowFeedback("Students cannot delete groups.", FeedbackType.Error);
            return false;
        }

        public bool Remove(BaseEntity entity)
        {
            ShowFeedback("Students cannot remove students from groups.", FeedbackType.Error);
            return false;
        }

        public bool MoveEntityOutOfList(BaseEntity entityToRemove)
        {
            ShowFeedback("Students cannot remove students from groups.", FeedbackType.Error);
            return false;
        }

        public bool MoveEntityInToList(BaseEntity entityToAdd)
        {
            ShowFeedback("Students cannot add students to groups.", FeedbackType.Error);
            return false;
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
                ShowFeedback("Students cannot view Students profiles.", FeedbackType.Error);
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

        private void UpdateMyGroupsList(int rows)
        {
            Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetForStudentSearch((Student)User, GroupSearchTxt, rows));
            ChangeSubgridContext(SubgridContext);//refresh the sub-grid content
        }
    }
}