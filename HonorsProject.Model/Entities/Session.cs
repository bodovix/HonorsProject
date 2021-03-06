﻿using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using System.Collections.ObjectModel;
using HonorsProject.Model.HelperClasses;
using Newtonsoft.Json.Linq;
using ParallelDots;
using Newtonsoft.Json;
using HonorsProject.Model.DTO;

namespace HonorsProject.Model.Entities
{
    public class Session : BaseEntity
    {
        #region Properties

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return DefaultDate(ref _startTime); }
            set
            {
                _startTime = DefaultDate(ref value);
            }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return DefaultDate(ref _endTime); }
            set { _endTime = DefaultDate(ref value); }
        }

        public virtual ObservableCollection<Lecturer> Lecturers { get; set; }
        public virtual Group Group { get; set; }
        public virtual List<Question> Questions { get; set; }

        //blacklists are tokenized by spaces
        private string blacklist;

        public string Blacklist
        {
            get { return blacklist; }
            set
            {
                //prevent prepending spaces
                if (value == " ")
                    value = "";
                blacklist = value.ToLower();
            }
        }

        //space at the end of the default list is important
        private string blaclistOriginalSource = "and i the this or it is a to be me that want ";

        public int CreatedByLecturerId { get; set; }

        #endregion Properties

        #region Constructors

        public Session()
        {
            Questions = new List<Question>();
            Lecturers = new ObservableCollection<Lecturer>();
            Blacklist = blaclistOriginalSource;
        }

        public void ShallowCopy(Session sessionToShallowCopy)
        {
            Id = sessionToShallowCopy.Id;
            Name = sessionToShallowCopy.Name;
            StartTime = sessionToShallowCopy.StartTime;
            EndTime = sessionToShallowCopy.EndTime;
            Lecturers = sessionToShallowCopy.Lecturers;
            Group = sessionToShallowCopy.Group;
            Questions = sessionToShallowCopy.Questions;
            CreatedByLecturerId = sessionToShallowCopy.CreatedByLecturerId;
            Blacklist = sessionToShallowCopy.Blacklist;
        }

        public Session(string name, DateTime startTime, DateTime endTime, ObservableCollection<Lecturer> lecturers, Group group, List<Question> questions, DateTime createdOn, int createdByLecturerId)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Lecturers = lecturers;
            Group = group;
            Questions = questions;
            CreatedOn = createdOn;
            CreatedByLecturerId = createdByLecturerId;
            Blacklist = blaclistOriginalSource;
        }

        #endregion Constructors

        private DateTime DefaultDate(ref DateTime value)
        {
            //if date is default set it to today
            if (DateTime.Compare(value.Date, new DateTime(0001, 01, 01)) == 0)
                value = DateTime.Now.Date;

            return value;
        }

        public int CalcNumberQuestionsAsked()
        {
            return Questions.Count();
        }

        public List<FrequentAskersTuple> CalcMostFrequentAskers()
        {
            return Questions.GroupBy(s => s.AskedBy)
                                .Select(sesh => new FrequentAskersTuple
                                {
                                    Student = sesh.Key,
                                    Count = sesh.Count()
                                }).OrderByDescending(tuple => tuple.Count).ToList();
        }

        public Dictionary<string, int> CalcKeyWordsAPI()
        {
            List<string> allQuestionsTextArray = new List<string>();
            //get all question text into array format
            foreach (Question q in Questions)
                allQuestionsTextArray.Add(q.QuestionText.ToLower());
            if (allQuestionsTextArray.Count == 0)
                return null;
            //get keywords for each question
            List<string> keywordsList = new List<string>();
            foreach (string text in allQuestionsTextArray)
            {
                string responceJson = KeyWordsAPI(text);
                if (!String.IsNullOrEmpty(responceJson))
                {
                    KeywordsDTO keywordsDTO = JsonConvert.DeserializeObject<KeywordsDTO>(responceJson);
                    foreach (Keyword word in keywordsDTO.Keywords)
                    {
                        keywordsList.Add(word.Key.ToString());
                    }
                }
            }
            if (keywordsList.Count == 0)
                return null;
            //tally up the keywords and return the most common ones
            Dictionary<String, int> results = new Dictionary<string, int>();
            foreach (string key in keywordsList)
            {
                if (!String.IsNullOrEmpty(key))
                {
                    if (!results.ContainsKey(key))
                        results.Add(key, 1);
                    else
                        results[key]++;
                }
            }
            return results;
        }

        /// <summary>
        /// https://user.apis.paralleldots.com/
        /// Key:82ByzdYGAOwfVQwLoF76lcj1BgqbYQ2FMwd37BU0BbU
        /// this api is free for a month
        /// return keywords from the above api
        /// </summary>
        /// <returns>an array of all the keywords</returns>
        private string KeyWordsAPI(string phraseToAnalise)
        {
            if (String.IsNullOrEmpty(phraseToAnalise) || String.IsNullOrWhiteSpace(phraseToAnalise))
            {
                //no phrase to analise return empty
                return "";
            }
            // Initialize instance of api class
            paralleldots pd = new paralleldots("82ByzdYGAOwfVQwLoF76lcj1BgqbYQ2FMwd37BU0BbU");

            // for single sentences
            String keywords = pd.keywords(phraseToAnalise);
            return keywords;

            //// for multiple sentence
            //JArray text_list = JArray.Parse("[\"" + phraseToAnalise + "\"]");
            //String keywords_batch = pd.keywords_batch(text_list);
            //return keywords_batch;
        }

        public Dictionary<string, int> CalcCommonPhraseIdentification()
        {
            string allText = new string(Questions.SelectMany(q => q.QuestionText.ToLower() + " ").ToArray());
            List<string> multiWords = new List<string>();
            List<string> list = allText.Split(',', '.', ';', ' ', '\n', '\r').ToList();
            list = list.Where(x => !string.IsNullOrEmpty(x)).ToList();
            string[] splitTxt = list.ToArray();

            const int MaxPhraseLength = 20;
            Dictionary<string, int> Counts = new Dictionary<string, int>();

            for (int phraseLen = MaxPhraseLength; phraseLen >= 2; phraseLen--)
            {
                for (int i = 0; i < splitTxt.Length - 1; i++)
                {
                    string[] phrase = GetPhrase(splitTxt, i, phraseLen);
                    string sphrase = string.Join(" ", phrase);

                    int index = FindPhraseIndex(splitTxt, i + phrase.Length, phrase);

                    if (index > -1)
                    {
                        Console.WriteLine("Phrase : {0} found at {1}", sphrase, index);
                        //check the sphrase isnt entirely blacklisted before adding to dictionary
                        if (ApproveFilterCommonPhrases(sphrase))
                        {
                            if (!Counts.ContainsKey(sphrase))
                                Counts.Add(sphrase, 1);
                            Counts[sphrase]++;
                        }
                    }
                }
            }
            return Counts;
        }

        private bool ApproveFilterCommonPhrases(string proposedKey)
        {
            bool isBlacklisted = true;
            string[] blacklist = Blacklist.Split(' ');

            //get all the words in the phrase
            string[] wordsInKey = proposedKey.Split(' ');
            //loop through the blacklisted words
            foreach (string word in wordsInKey)
                if (!blacklist.Contains(word.ToLower()))
                {
                    //if all words in record are blacklisted remove it from the dictionary
                    isBlacklisted = false;
                    break;
                }
            if (isBlacklisted)
                return false;
            else
                return true;
        }

        public bool AddToBlacklist(UnitOfWork u, string newWord, ref string feedback)
        {
            //validate new word
            if (String.IsNullOrEmpty(newWord))
            {
                feedback = "New word required cannot contain spaces.";
                return false;
            }

            if (Blacklist.Contains(" " + newWord.ToLower() + " "))
            {
                feedback = "Word already blacklisted.";
                return false;
            }
            if (newWord.Contains(' '))
            {
                feedback = "New word cannot contain spaces.";
                return false;
            }

            if (String.IsNullOrWhiteSpace(newWord))
            {
                feedback = "New word required cannot contain whitespace.";
                return false;
            }
            //apply new word with appended whitespace
            Blacklist = Blacklist + newWord + " ";
            //save changes
            bool result = (u.Complete() > 0) ? true : false;
            if (result)
                return true;
            else
                return false;
        }

        public bool RemoveFromBlacklist(UnitOfWork u, string w, ref string feedback)
        {
            string wordToRemove = w.ToLower();
            string[] blacklistArray = Blacklist.Split(' ');
            if (blacklistArray.Contains(wordToRemove))
            {
                //remove word and appended whitespace
                Blacklist = Blacklist.Replace(wordToRemove + " ", string.Empty);
                bool result = (u.Complete() > 0) ? true : false;
                if (result)
                    return true;
                else
                    return false;
            }
            else
            {
                feedback = $"Word '{wordToRemove}' not found in blacklist.";
                return false;
            }
        }

        private static string[] GetPhrase(string[] words, int startpos, int len)
        {
            return words.Skip(startpos).Take(len).ToArray();
        }

        private static int FindPhraseIndex(string[] words, int startIndex, string[] matchWords)
        {
            for (int i = startIndex; i < words.Length; i++)
            {
                int j;

                for (j = 0; j < matchWords.Length && (i + j) < words.Length; j++)
                    if (matchWords[j] != words[i + j])
                        break;

                if (j == matchWords.Length)
                    return startIndex;
            }

            return -1;
        }

        public bool ValidateSession(UnitOfWork u)
        {
            if (String.IsNullOrEmpty(this.Name))
                throw new ArgumentException("Name required.");
            if (Name.Length > nameSizeLimit)
                throw new ArgumentException($"Name cannot exceed {nameSizeLimit} characters.");
            if (StartTime == null)
                throw new ArgumentException("Start time required.");
            if (EndTime == null)
                throw new ArgumentException("End time required.");
            if (Group == null)
                throw new ArgumentException("Sessions must belong to a group.");
            if (Group.Id == 0)
                throw new ArgumentException("Sessions must belong to a group.");

            if (CreatedByLecturerId == 0)
                throw new ArgumentException("Session created by Id required.");
            if (Lecturers == null)
                throw new ArgumentException("Session must have lecturers");
            if (Lecturers.Count == 0)
                throw new ArgumentException("Session must have lecturers");
            if (u.SessionRepository.CheckSessionNameAlreadyExistsForGroup(this))
                throw new ArgumentException("Session name already exists for this group.");
            return true;
        }
    }
}