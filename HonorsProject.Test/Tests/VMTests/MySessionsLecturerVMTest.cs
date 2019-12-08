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
            //Arrange
            //questions answers should cascade  - nothing else
            int expectedQuestionCount = 4;
            int expectedAnswerCount = 1;
            int expectedStudentCount = 3;
            int expectedGroupCount = 2;
            int expectedLectuerCount = 2;
            int expectedSessionCount = 4;

            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.SessionsContext = SessionsContext.Future;// were going to work with the future sessions
            VM.GetAllMyFutureSessions();
            VM.SelectedSession = VM.MySessions.Where(s => s.Name.Equals("Delete Sesh")).FirstOrDefault();
            //delete message box confirmed
            VM.IsConfirmationAccepted = true;
            //Act
            var result = VM.Delete(VM.SelectedSession);
            //Assert
            using (UnitOfWork u = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                Assert.IsTrue(result);
                Assert.AreEqual(expectedQuestionCount, u.QuestionRepository.Count(), "Question Count Wrong");
                Assert.AreEqual(expectedAnswerCount, u.AnswerRepository.Count(), "Answer count wrong");
                Assert.AreEqual(expectedStudentCount, u.StudentRepo.Count(), "Student count wrong");
                Assert.AreEqual(expectedGroupCount, u.GroupRepository.Count(), "Group count wrong");
                Assert.AreEqual(expectedLectuerCount, u.LecturerRepo.Count(), "Lecturer count wrong");
                Assert.AreEqual(expectedSessionCount, u.SessionRepository.Count(), "Session count wrong");
            }
        }

        [TestMethod]
        public void Delete_Confirm_DeletePreviousSesh_Success()
        {
            //Arrange
            //questions answers should cascade  - nothing else
            int expectedQuestionCount = 4;
            int expectedAnswerCount = 4;
            int expectedStudentCount = 3;
            int expectedGroupCount = 2;
            int expectedLectuerCount = 2;
            int expectedSessionCount = 4;

            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.SessionsContext = SessionsContext.Previous;// were going to work with the future sessions
            VM.GetAllMyPreviousSessions();
            VM.SelectedSession = VM.MySessions.Where(s => s.Name.Equals("Previous Sesh")).FirstOrDefault();
            //delete message box confirmed
            VM.IsConfirmationAccepted = true;
            //Act
            var result = VM.Delete(VM.SelectedSession);
            //Assert
            using (UnitOfWork u = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                Assert.IsTrue(result, VM.FeedbackMessage);
                Assert.AreEqual(expectedQuestionCount, u.QuestionRepository.Count(), "Question Count Wrong");
                Assert.AreEqual(expectedAnswerCount, u.AnswerRepository.Count(), "Answer count wrong");
                Assert.AreEqual(expectedStudentCount, u.StudentRepo.Count(), "Student count wrong");
                Assert.AreEqual(expectedGroupCount, u.GroupRepository.Count(), "Group count wrong");
                Assert.AreEqual(expectedLectuerCount, u.LecturerRepo.Count(), "Lecturer count wrong");
                Assert.AreEqual(expectedSessionCount, u.SessionRepository.Count(), "Session count wrong");
            }
        }

        [TestMethod]
        public void Delete_MissingObject_Fail()
        {
            //Arrange
            //nothing changes since no delete
            int expectedQuestionCount = 6;
            int expectedAnswerCount = 4;
            int expectedStudentCount = 3;
            int expectedGroupCount = 2;
            int expectedLectuerCount = 2;
            int expectedSessionCount = 5;

            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.SessionsContext = SessionsContext.Future;// were going to work with the future sessions
            VM.GetAllMyFutureSessions();
            VM.SelectedSession = VM.MySessions.Where(s => s.Id == 0).FirstOrDefault();
            //delete message box confirmed
            VM.IsConfirmationAccepted = true;
            //Act
            var result = VM.Delete(VM.SelectedSession);
            //Assert
            using (UnitOfWork u = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                Assert.IsFalse(result);
                Assert.AreEqual(expectedQuestionCount, u.QuestionRepository.Count(), "Question Count Wrong");
                Assert.AreEqual(expectedAnswerCount, u.AnswerRepository.Count(), "Answer count wrong");
                Assert.AreEqual(expectedStudentCount, u.StudentRepo.Count(), "Student count wrong");
                Assert.AreEqual(expectedGroupCount, u.GroupRepository.Count(), "Group count wrong");
                Assert.AreEqual(expectedLectuerCount, u.LecturerRepo.Count(), "Lecturer count wrong");
                Assert.AreEqual(expectedSessionCount, u.SessionRepository.Count(), "Session count wrong");
            }
        }

        [TestMethod]
        public void Delete_Cancel_Fail()
        {
            //Arrange
            //nothing changes since no delete
            int expectedQuestionCount = 6;
            int expectedAnswerCount = 4;
            int expectedStudentCount = 3;
            int expectedGroupCount = 2;
            int expectedLectuerCount = 2;
            int expectedSessionCount = 5;

            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.SessionsContext = SessionsContext.Future;// were going to work with the future sessions
            VM.GetAllMyFutureSessions();
            VM.SelectedSession = VM.MySessions.Where(s => s.Name.Equals("Delete Sesh")).FirstOrDefault();
            //delete message box CANCELED - NO DELETE
            VM.IsConfirmationAccepted = false;
            //Act
            var result = VM.Delete(VM.SelectedSession);
            //Assert
            using (UnitOfWork u = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                Assert.IsFalse(result);
                Assert.AreEqual(expectedQuestionCount, u.QuestionRepository.Count(), "Question Count Wrong");
                Assert.AreEqual(expectedAnswerCount, u.AnswerRepository.Count(), "Answer count wrong");
                Assert.AreEqual(expectedStudentCount, u.StudentRepo.Count(), "Student count wrong");
                Assert.AreEqual(expectedGroupCount, u.GroupRepository.Count(), "Group count wrong");
                Assert.AreEqual(expectedLectuerCount, u.LecturerRepo.Count(), "Lecturer count wrong");
                Assert.AreEqual(expectedSessionCount, u.SessionRepository.Count(), "Session count wrong");
            }
        }

        #endregion DeleteSessions
    }
}