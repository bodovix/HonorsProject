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
            Assert.AreEqual(VM.Groups.Count, 2, "VM Groups Count Wrong");//one null and the test one(s)
            Assert.AreEqual(VM.AvailableLecturers.Count, 1);
        }

        [TestMethod]
        public void Save_AddNew_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            DateTime startDate = new DateTime(2019, 10, 01);
            DateTime endDate = new DateTime(2019, 10, 01);
            DateTime createdOn = new DateTime(2019, 09, 29);
            ObservableCollection<Lecturer> LecLst = new ObservableCollection<Lecturer>();
            LecLst.Add(VM.AvailableLecturers.Where(l => l.Id == 444).FirstOrDefault());
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            bool result = VM.Save();
            //Assert
            Assert.IsTrue(result, $"Save Returned False: Message: {VM.FeedbackMessage}");
            Assert.IsTrue(VM.FormContext == FormContext.Create, "FormContext in wrong initial mode");
            Assert.IsTrue(VM.UserRole == Role.Lecturer, "UserRole in wrong initial mode for lecturer");
            Assert.IsTrue(VM.MySessions.Count == 2, $"VM Sessions Count Wrong {VM.MySessions.Count} : should be {2}");
            Assert.IsTrue(VM.AvailableLecturers.Count == 1, "VM Lecturer Count Wrong");
            Assert.IsTrue(VM.Groups.Count == 2, "VM Groups Count Wrong");//one null and the test one(s)
        }
    }
}