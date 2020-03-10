using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseMyGroupsPageVM : BaseViewModel, ISaveVMFormCmd, IEnterNewModeCmd, IChangeSubgridCmd, IDeleteCmd, IRemoveEntityCmd, IMoveEntityInList, IGoToEntityCmd, IAnalyseEntityCmd, ICancelmd
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
                RefreshAvailableStudents(SelectedGroup);
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

        public abstract ISystemUser User { get; set; }

        #endregion Properties

        #region Commands

        public SaveCmd SaveFormCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public MoveEntityOutOfListCmd MoveEntityOutOfListCmd { get; set; }
        public MoveEntityInToListCmd MoveEntityInToListCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }
        public AnalyseEntityCmd AnalyseEntityCmd { get; set; }

        #endregion Commands

        public BaseMyGroupsPageVM(string dbcontextName) : base(dbcontextName)
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
            AnalyseEntityCmd = new AnalyseEntityCmd(this);
            CancelCmd = new CancelCmd(this);
            try
            {
                Mediator.Register(MediatorChannels.PoolingUpdate.ToString(), PoolingUpdate);
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        private void PoolingUpdate(object obj)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                //Temp group holds the selected Group's properties so they don't get cleared with pooling
                Group tempGroup = new Group();
                tempGroup.ShallowCopy(SelectedGroup);

                UpdateMyGroupsList(RowLimit);
                RefreshAvailableStudents(SelectedGroup);
                SelectedGroup.ShallowCopy(tempGroup);

                foreach (Group g in Groups)
                {//safe way to get record value updates from database
                    if (g.Id != SelectedGroup.Id)
                        UnitOfWork.Reload(g);
                    if (g.Id == SelectedGroup.Id)//reload the groups list
                        SelectedGroup.Students = new ObservableCollection<Student>(UnitOfWork.StudentRepo.GetStudentsFromGroup(g));
                }
                OnPropertyChanged(nameof(SelectedGroup));
            });
        }

        public abstract bool GoToEntity(BaseEntity entity);

        public abstract bool GoToAnalyseEntity(BaseEntity entity);

        public abstract bool MoveEntityInToList(BaseEntity entityToAdd);

        public abstract bool MoveEntityOutOfList(BaseEntity entityToRemove);

        public abstract bool Remove(BaseEntity entity);

        public abstract bool Save();

        public abstract bool Delete(BaseEntity objToDelete);

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
                    ShowFeedback("Rolled back unsaved changes.", FeedbackType.Info);
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

        public bool ChangeSubgridContext(SubgridContext context)
        {
            bool result = true;
            SubgridContext = context;

            switch (context)
            {
                case SubgridContext.ActiveSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetCurrentSessions(SelectedGroup, DateTime.Now));
                    Mediator.NotifyColleagues(MediatorChannels.LoadActiveSessionsSubgrid.ToString(), null);
                    break;

                case SubgridContext.FutureSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetFutureSessions(SelectedGroup, DateTime.Now));
                    //update the view to show future sessions
                    Mediator.NotifyColleagues(MediatorChannels.LoadFutureSessionsSubgrid.ToString(), null);
                    break;

                case SubgridContext.PreviousSessions:
                    //load required Sessions..
                    FilteredSessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetPreviousSessions(SelectedGroup, DateTime.Now));
                    //update the view to show previous
                    Mediator.NotifyColleagues(MediatorChannels.LoadPreviousSessionsSubgrid.ToString(), null);
                    break;

                case SubgridContext.Groups:
                    throw new NotImplementedException("Groups subgrid not required. Contact Support.");
                case SubgridContext.Questions:
                    throw new NotImplementedException("Questions subgrid not required. Contact Support.");
                case SubgridContext.Answers:
                    throw new NotImplementedException("Answers subgrid not required. Contact Support.");
                case SubgridContext.Students:
                    //update the view to show Students Subgrid
                    Mediator.NotifyColleagues(MediatorChannels.LoadStudentsSubgrid.ToString(), null);
                    break;

                default:
                    throw new AggregateException("Invalid subgrid option. Please contact support.");
            }
            return result;
        }

        public void EnterNewMode()
        {
            SelectedGroup = new Group();
            FormContext = FormContext.Create;
        }

        protected void RefreshAvailableStudents(Group group)
        {
            //if (group == null || group.Id == 0) // If in New Mode
            //    students = UnitOfWork.StudentRepo.GetAll().ToList();
            //else // if student already selected
            StudentsNotInGroup = new ObservableCollection<Student>(UnitOfWork.StudentRepo.GetStudentsNotInGroup(group).ToList());
        }

        protected void UpdateMyGroupsList(int rows)
        {
            Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTopXFromSearch(GroupSearchTxt, rows));
            ChangeSubgridContext(SubgridContext);//refresh the sub-grid content
        }
    }
}