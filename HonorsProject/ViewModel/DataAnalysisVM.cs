﻿using HonorsProject.Model.Core;
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
    public class DataAnalysisVM : BaseViewModel, IGoToEntityCmd, IRemoveCmd, IAddCmd
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

        public Dictionary<string, int> KeyWordsAPI()
        {
            try
            {
                return SelectedSession.CalcKeyWordsAPI();
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return null;
            }
        }

        private Session _selectedSession;

        public Session SelectedSession
        {
            get { return _selectedSession; }
            set
            {
                ClearFeedback();
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
                            BlacklistList = new ObservableCollection<string>(SelectedSession.Blacklist.Split(' '));
                            CommonPhrases = SelectedSession.CalcCommonPhraseIdentification();
                            KeyWords = SelectedSession.CalcKeyWordsAPI();
                        }
                    UpdateHeader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error with API responce: " + ex.Message);
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
                //set defaults for common phrases
                if (CommonPhrases == null)
                    CommonPhrases = new Dictionary<string, int>();
                if (CommonPhrases.Count == 0)
                    CommonPhrases.Add("No Common Phrases detected.", 0);
                //update view
                OnPropertyChanged(nameof(CommonPhrases));
            }
        }

        private ObservableCollection<string> blacklistList;

        public ObservableCollection<string> BlacklistList
        {
            get { return blacklistList; }
            set
            {
                blacklistList = value;
                OnPropertyChanged(nameof(BlacklistList));
            }
        }

        private string proposedBlacklist;

        public string ProposedBlacklist
        {
            get { return proposedBlacklist; }
            set
            {
                proposedBlacklist = value;
                OnPropertyChanged(nameof(ProposedBlacklist));
            }
        }

        private Dictionary<string, int> _keyWords;

        public Dictionary<string, int> KeyWords
        {
            get { return _keyWords; }
            set
            {
                if (value == null)
                    value = new Dictionary<string, int>();
                _keyWords = value;
                if (KeyWords.Count == 0)
                    KeyWords.Add("No Key Words detected.", 0);
                OnPropertyChanged(nameof(KeyWords));
            }
        }

        #endregion Properties

        public GoToEntityCmd GoToEntityCmd { get; set; }
        public RemoveCmd RemoveCmd { get; set; }
        public AddCmd AddCmd { get; set; }

        public DataAnalysisVM(BaseEntity entityToFocusOn, string dbcontextName) : base(dbcontextName)
        {
            //commands
            GoToEntityCmd = new GoToEntityCmd(this);
            RemoveCmd = new RemoveCmd(this);
            AddCmd = new AddCmd(this);

            //initialize
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
            if (entity is Group)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToThisGroup.ToString(), entity);
                return true;
            }
            if (entity is Session)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToThisSession.ToString(), entity);
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

        /// <summary>
        /// Remove command used to remove items from blacklist
        /// </summary>
        /// <param name="objToRemove">String word to remove from blacklist</param>
        /// <returns></returns>
        public bool Remove(object objToRemove)
        {
            if (SelectedSession == null)
            {
                ShowFeedback("No Session selected", FeedbackType.Info);
                return false;
            }
            try
            {
                string wordToRemove = objToRemove as string;
                string feedback = "";
                bool result = SelectedSession.RemoveFromBlacklist(UnitOfWork, wordToRemove, ref feedback);
                BlacklistList = new ObservableCollection<string>(SelectedSession.Blacklist.Split(' '));
                CommonPhrases = SelectedSession.CalcCommonPhraseIdentification();
                if (result)
                {
                    ShowFeedback($"{wordToRemove} Removed from blacklist.", FeedbackType.Success);
                    return true;
                }
                else
                {
                    ShowFeedback(feedback, FeedbackType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        /// <summary>
        /// Add Command used to add items to the blacklist
        /// </summary>
        /// <param name="objToAdd">String word to add to blacklist</param>
        /// <returns></returns>
        public bool Add(object objToAdd)
        {
            if (SelectedSession == null)
            {
                ShowFeedback("No Session selected", FeedbackType.Info);
                return false;
            }

            try
            {
                string wordToAdd = objToAdd as string;
                string feedback = "";
                //set new blacklist and update the VM collection
                bool result = SelectedSession.AddToBlacklist(UnitOfWork, wordToAdd, ref feedback);
                BlacklistList = new ObservableCollection<string>(SelectedSession.Blacklist.Split(' '));
                CommonPhrases = SelectedSession.CalcCommonPhraseIdentification();
                //show appropriate feedback
                if (result)
                {
                    ShowFeedback($"{wordToAdd} added to blacklist.", FeedbackType.Success);
                    return true;
                }
                else
                {
                    ShowFeedback(feedback, FeedbackType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }
    }
}