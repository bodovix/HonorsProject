﻿using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using System.Collections.ObjectModel;
using HonorsProject.Model.HelperClasses;

namespace HonorsProject.Model.Entities
{
    public class Session : BaseEntity
    {
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
        public int CreatedByLecturerId { get; set; }

        public Session()
        {
            Questions = new List<Question>();
            Lecturers = new ObservableCollection<Lecturer>();
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
        }

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

                        if (!Counts.ContainsKey(sphrase))
                            Counts.Add(sphrase, 1);

                        Counts[sphrase]++;
                    }
                }
            }

            return Counts;
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