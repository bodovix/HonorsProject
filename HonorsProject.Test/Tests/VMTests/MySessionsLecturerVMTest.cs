using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test
{
    [TestClass]
    public class MySessionsLecturerVMTest : BaseTest
    {
        private MySessionsLecturerPageVM VM;
        private Lecturer _lecturer;

        public MySessionsLecturerVMTest() : base()
        {
            _lecturer = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234);
        }

        [TestMethod]
        public void LoadVM_Sucess()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            //Act
            // -- VM constructor is Act
            //Assert
            int expectedSessions = 2;
            int expectedGroups = 3;
            int expectedLecturers = 2;
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong expected {expectedSessions}  : actual {VM.MySessions.Count}");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, $"VM Groups Count Wrong. Expected {expectedGroups}   : actual {VM.Groups.Count}");//one null and the test one(s)
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count);
        }

        #region SaveSessons

        [TestMethod]
        public void Save_AddNew_Active_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            VM.SessionsContext = SessionsContext.Active;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 444).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date;
            DateTime createdOn = new DateTime(2019, 09, 29);
            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            int expectedSessions = 3;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_AddNew_Active_LecturerNotInList_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            VM.SessionsContext = SessionsContext.Active;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 555).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date;
            DateTime createdOn = DateTime.Now.Date.AddDays(-3);
            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            int expectedSessions = 3;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_AddNew_Future_LecturerNotInlist_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            VM.SessionsContext = SessionsContext.Future;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 555).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = DateTime.Now.AddDays(40).Date;
            DateTime endDate = DateTime.Now.AddDays(40).Date;
            DateTime createdOn = DateTime.Now.AddDays(40).Date;

            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            int expectedSessions = 3;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_AddNew_Future_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            VM.SessionsContext = SessionsContext.Future;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 444).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = DateTime.Now.AddDays(40).Date;
            DateTime endDate = DateTime.Now.AddDays(40).Date;
            DateTime createdOn = DateTime.Now.AddDays(40).Date;

            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            int expectedSessions = 3;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_AddNew_Previous_LecturerNotInlist_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            VM.SessionsContext = SessionsContext.Previous;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 555).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = DateTime.Now.AddDays(-40).Date;
            DateTime endDate = DateTime.Now.AddDays(-40).Date;
            DateTime createdOn = DateTime.Now.AddDays(-40).Date;

            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            int expectedSessions = 2;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_AddNew_Previous_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            VM.SessionsContext = SessionsContext.Previous;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 444).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = DateTime.Now.AddDays(-40).Date;
            DateTime endDate = DateTime.Now.AddDays(-40).Date;
            DateTime createdOn = DateTime.Now.AddDays(-40).Date;

            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            int expectedSessions = 2;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_UpdateSelected_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            //Act - update session details
            //select a session
            VM.SelectedSession = VM.MySessions.Where(s => s.Id == 1).FirstOrDefault();
            //update it
            VM.SelectedSession.Name = "UpdatedTo";
            VM.SelectedSession.Group = VM.Groups.Where(g => g.Id == 2).FirstOrDefault();
            VM.SelectedSession.Lecturers.Add(VM.AvailableLecturers.Where(l => l.Id == 555).FirstOrDefault());
            VM.SelectedSession.StartTime = new DateTime(2019, 12, 06);
            VM.SelectedSession.EndTime = new DateTime(2019, 12, 06);
            bool result = VM.Save();
            //Assert
            int expectedSessions = 2;
            int expectedLecturers = 2;
            int expectedGroups = 3;
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Update, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(expectedSessions, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {expectedSessions}");
            Assert.AreEqual(expectedLecturers, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(expectedGroups, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }

        #endregion SaveSessons

        #region DeleteSessions

        [TestMethod]
        public void Delete_Confirm_Success()
        {
        }

        [TestMethod]
        public void Delete_MissingObject_Fail()
        {
        }

        [TestMethod]
        public void Delete_Cancel_Fail()
        {
        }

        #endregion DeleteSessions
    }
}