using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    internal class DataAnalysisVM : BaseViewModel
    {
        private int rowLimit;
        private Lecturer _user;

        public Lecturer User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
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
                UpdateGroupsList();
            }
        }

        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
                UpdateSessionsList();
            }
        }

        private ObservableCollection<Group> groups;

        public ObservableCollection<Group> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }

        private string _sessionSearchTxt;

        public string SessionSearchTxt
        {
            get { return _sessionSearchTxt; }
            set
            {
                _sessionSearchTxt = value;
                OnPropertyChanged(nameof(SessionSearchTxt));
                UpdateSessionsList();
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
                try
                {
                    //calculate
                    if (SelectedSession != null)
                        if (SelectedSession.Id != 0)
                        {
                            NumQuestionsAsked = SelectedSession.CalcNumberQuestionsAsked();
                            MostFrequentAskers = new ObservableCollection<FrequentAskersTuple>(SelectedSession.CalcMostFrequentAskers());
                            CommonPhrases = SelectedSession.CalcCommonPhraseIdentification();
                        }
                }
                catch (Exception ex)
                {
                    ShowFeedback(ex.Message, FeedbackType.Error);
                }
            }
        }

        private ObservableCollection<Session> _sessions;

        public ObservableCollection<Session> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                OnPropertyChanged(nameof(Sessions));
            }
        }

        private int _numbQuestionsAsked;

        public int NumQuestionsAsked
        {
            get { return _numbQuestionsAsked; }
            set
            {
                _numbQuestionsAsked = value;
                OnPropertyChanged(nameof(NumQuestionsAsked));
            }
        }

        private ObservableCollection<FrequentAskersTuple> _mostFrequentAskers;

        public ObservableCollection<FrequentAskersTuple> MostFrequentAskers
        {
            get { return _mostFrequentAskers; }
            set
            {
                _mostFrequentAskers = value;
                OnPropertyChanged(nameof(MostFrequentAskers));
            }
        }

        private Dictionary<string, int> _commonPhrases;

        public Dictionary<string, int> CommonPhrases
        {
            get { return _commonPhrases; }
            set
            {
                _commonPhrases = value;
                OnPropertyChanged(nameof(CommonPhrases));
            }
        }

        public DataAnalysisVM(string dbcontextName) : base(dbcontextName)
        {
            try
            {
                User = (Lecturer)App.AppUser;
                UserRole = Role.Lecturer;
                rowLimit = 30;
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTopXFromSearch(GroupSearchTxt, rowLimit));
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        private void UpdateGroupsList()
        {
            try
            {
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTopXFromSearch(GroupSearchTxt, rowLimit));
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        private void UpdateSessionsList()
        {
            try
            {
                if (SelectedGroup != null)
                {
                    if (SelectedGroup.Id != 0)
                        Sessions = new ObservableCollection<Session>(UnitOfWork.SessionRepository.GetTopXWithSearchForGroup(SelectedGroup, SessionSearchTxt, rowLimit));
                    else
                    {
                        Sessions = new ObservableCollection<Session>();
                        ShowFeedback("No Group selected", FeedbackType.Info);
                    }
                }
                else
                {
                    Sessions = new ObservableCollection<Session>();
                    ShowFeedback("No Group selected", FeedbackType.Info);
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }
    }
}