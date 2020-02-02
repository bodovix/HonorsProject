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
                                .Select(tuple => new FrequentAskersTuple
                                {
                                    Student = tuple.Key,
                                    Count = tuple.Count()
                                }).OrderBy(tuple => tuple.Count).ToList();
        }

        public void CalcCommonPhraseIdentification()
        {
            throw new NotImplementedException("not implemented yet");
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