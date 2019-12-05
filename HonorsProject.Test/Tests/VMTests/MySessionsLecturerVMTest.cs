using System;
using System.Collections.Generic;
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
            Assert.IsTrue(VM.Groups.Count == 2, "VM Groups Count Wrong");//one null and the test one(s)
        }

        [TestMethod]
        public void Save_AddNew_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new MySessionsLecturerPageVM(_lecturer, dbConName);
            VM.FormContext = FormContext.Create;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime createdOn = new DateTime();
            List<Lecturer> LecLst = new List<Lecturer>();
            Group group = VM.Groups.Where(g => g.Id == 1).FirstOrDefault();
            VM.SelectedSession = new Session("Test Session", startDate, endDate, LecLst, group, null, createdOn, _lecturer.Id);

            //Act
            VM.Save();
            //Assert
            Assert.IsTrue(VM.FormContext == FormContext.Create, "FormContext in wrong initial mode");
            Assert.IsTrue(VM.UserRole == Role.Lecturer, "UserRole in wrong initial mode for lecturer");
            Assert.IsTrue(VM.MySessions.Count == 2, "VM Sessions Count Wrong");
            Assert.IsTrue(VM.Groups.Count == 2, "VM Groups Count Wrong");//one null and the test one(s)
        }
    }
}