using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class DataAnalysisVM : BaseViewModel, IGoToEntityCmd
    {
        #region Properties

        private int rowLimit;
        private string _selectionTitle;

        public string SelectionTitle
        {
            get { return _selectionTitle; }
            set
            {
                _selectionTitle = value;
                OnPropertyChanged(nameof(SelectionTitle));
            }
        }

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
                UpdateHeader();
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
                            if (CommonPhrases.Count == 0)
                                CommonPhrases.Add("No Common Phrases detected.", 0);
                        }
                    UpdateHeader();
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

        #endregion Properties

        public GoToEntityCmd GoToEntityCmd { get; set; }

        public DataAnalysisVM(BaseEntity entityToFocusOn, string dbcontextName) : base(dbcontextName)
        {
            GoToEntityCmd = new GoToEntityCmd(this);
            UpdateHeader();
            try
            {
                User = (Lecturer)App.AppUser;
                UserRole = Role.Lecturer;
                rowLimit = 30;
                Groups = new ObservableCollection<Group>(UnitOfWork.GroupRepository.GetTopXFromSearch(GroupSearchTxt, rowLimit));
                if (entityToFocusOn is Group)
                    SelectedGroup = UnitOfWork.GroupRepository.Get(entityToFocusOn.Id);
                else if (entityToFocusOn is Session)
                {
                    Session session = (Session)entityToFocusOn;
                    SelectedGroup = UnitOfWork.GroupRepository.Get(session.Group.Id);
                    if (SelectedGroup != null)
                        SelectedSession = UnitOfWork.SessionRepository.Get(entityToFocusOn.Id);
                }
                //Register for pooling update signals
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
                UpdateGroupsList();
                if (SelectedGroup != null)
                    UpdateSessionsList();
                UpdateHeader();
            });
        }

        public bool GoToEntity(BaseEntity entity)
        {
            if (entity is Student)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToThisStudent.ToString(), entity);
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

        public string GetKeyPhraseAPI()
        {
            //using the TextRazor API
            string html = null;
            string url = @"https://api.textrazor.com";
            string content = "TEST: All endpoints expect the API Key to be passed as an HTTP header ";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Headers.Add("X-TextRazor-Key", "31a755e8066eebf713e7e402cca27c8c406d72f0b24a079525998cad");
            request.Headers.Add("extractors", "entities,entailments");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("text", content);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
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

        private void UpdateHeader()
        {
            if (SelectedGroup == null)
            {
                SelectionTitle = "No Groups or Sessions selected.";
                return;
            }
            else if (SelectedGroup.Id == 0)
            {
                SelectionTitle = "No Groups or Sessions selected.";
                return;
            }
            else if (SelectedSession == null)
            {
                SelectionTitle = $"{SelectedGroup.Name}";
                return;
            }
            else if (SelectedSession.Id == 0)
            {
                SelectionTitle = $"{SelectedGroup.Name}";
                return;
            }
            else
            {
                SelectionTitle = $"{SelectedGroup.Name} - {SelectedSession.Name}";
            }
        }
    }
}