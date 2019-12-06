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
            Assert.IsTrue(VM.FormContext == FormContext.Create, "FormContext in wrong initial mode");
            Assert.IsTrue(VM.UserRole == Role.Lecturer, "UserRole in wrong initial mode for lecturer");
            Assert.IsTrue(VM.MySessions.Count == 1, "VM Sessions Count Wrong");
            Assert.AreEqual(3, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
            Assert.AreEqual(2, VM.AvailableLecturers.Count);
        }

        [TestMethod]
        public void Save_AddNew_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;

            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 444).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            DateTime startDate = new DateTime(2019, 10, 01);
            DateTime endDate = new DateTime(2019, 10, 01);
            DateTime createdOn = new DateTime(2019, 09, 29);
            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Create, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(2, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {2}");
            Assert.AreEqual(2, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(3, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
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
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.AreEqual(FormContext.Update, VM.FormContext, "FormContext in wrong initial mode");
            Assert.AreEqual(Role.Lecturer, VM.UserRole, "UserRole in wrong initial mode for lecturer");
            Assert.AreEqual(1, VM.MySessions.Count, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {2}");
            Assert.AreEqual(2, VM.AvailableLecturers.Count, "VM Lecturer Count Wrong");
            Assert.AreEqual(3, VM.Groups.Count, "VM Groups Count Wrong");//one null and the test one(s)
        }
    }
}