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
    public abstract class BaseMySessionsPageVM : BaseViewModel, ISaveVMFormCmd, IEnterNewModeCmd, IChangeSubgridCmd, IAddLecturerCmd, IRemoveEntityCmd, IDeleteCmd, IGoToEntityCmd, IAnalyseEntityCmd, ICancelmd
    {
        #region Properties

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

        public bool IsConfirmationAccepted { get; set; }

        #endregion Properties

        #region Commands

        public SaveCmd SaveFormCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public AddLecturerCmd AddLecturerCmd { get; set; }
        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }
        public AnalyseEntityCmd AnalyseEntityCmd { get; set; }

        #endregion Commands

        public BaseMySessionsPageVM(string dbcontextName) : base(dbcontextName)
        {
            //register commands
            NewModeCmd = new NewModeCmd(this);
            SaveFormCmd = new SaveCmd(this);
            AddLecturerCmd = new AddLecturerCmd(this);
            RemoveEntityCmd = new RemoveEntityCmd(this);
            DeleteCmd = new DeleteCmd(this);
            ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
            GoToEntityCmd = new GoToEntityCmd(this);
            AnalyseEntityCmd = new AnalyseEntityCmd(this);
            CancelCmd = new CancelCmd(this);
            //initial setup
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
                UpdateMySessionsList();
                GetAllLecturers();
            });
        }

        public abstract bool AddLecturer();

        public abstract bool Delete(BaseEntity objToDelete);

        public abstract void EnterNewMode();

        public abstract bool Remove(BaseEntity entity);

        public abstract bool Save();

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

        public bool GoToEntity(BaseEntity entity)
        {
            Mediator.NotifyColleagues(MediatorChannels.GoToThisSession.ToString(), entity);
            return true;
        }

        public bool GetAllMyCurrentSessions()
        {
            try
            {
                SubgridContext = SubgridContext.ActiveSessions;
                MySessions = new ObservableCollection<Session>(User.GetAllMyCurrentSessions(DateTime.Now, UnitOfWork));
                OnPropertyChanged(nameof(MySessions));
                OnPropertyChanged(nameof(SelectedSession));
                if (MySessions != null)
                    return true;
                else
                    return false;
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
                MySessions = new ObservableCollection<Session>(User.GetAllMyPreviousSessions(DateTime.Now, UnitOfWork));

                OnPropertyChanged(nameof(MySessions));
                OnPropertyChanged(nameof(SelectedSession));
                return true;
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
                List<Session> result = User.GetAllMyFutureSessions(DateTime.Now, UnitOfWork);
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

        protected void GetAllLecturers()
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

        protected void GetAllGroups()
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

        protected void UpdateMySessionsList()
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

        public abstract bool GoToAnalyseEntity(BaseEntity entity);
    }
}