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

namespace HonorsProject.Test.ViewModel
{
    [TestClass]
    public class LecturerVMTest : BaseTest
    {
        private LecturerPageVM VM;
        private Lecturer _lecturer;

        public LecturerVMTest() : base()
        {
            _lecturer = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", true, new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234);
        }

        [TestMethod]
        public void ToggleAdmin_ToFalse_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new LecturerPageVM(dbConName, _lecturer);
            VM.SelectedLecturer = VM.Lecturers.Where(l => l.Name.Equals("Gavin Hales")).FirstOrDefault();
            //Act
            bool result = VM.ToggleAdminRole(VM.SelectedLecturer);
            //Assert
            Assert.IsTrue(result);
            Assert.IsFalse(VM.SelectedLecturer.IsSuperAdmin);
        }

        [TestMethod]
        public void ToggleAdmin_ToTrue_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new LecturerPageVM(dbConName, _lecturer);
            VM.SelectedLecturer = VM.Lecturers.Where(l => l.Name.Equals("Gavin Hales")).FirstOrDefault();
            //Act
            bool result = VM.ToggleAdminRole(VM.SelectedLecturer);
            result = VM.ToggleAdminRole(VM.SelectedLecturer);
            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(VM.SelectedLecturer.IsSuperAdmin);
        }

        [TestMethod]
        public void ToggleAdmin_CantDoSelf_False()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
            VM = new LecturerPageVM(dbConName, _lecturer);
            VM.SelectedLecturer = VM.Lecturers.Where(l => l.Id == VM.User.Id).FirstOrDefault();
            //Act
            bool result = VM.ToggleAdminRole(VM.SelectedLecturer);
            result = VM.ToggleAdminRole(VM.SelectedLecturer);
            //Assert
            Assert.IsFalse(result);
            Assert.IsTrue(VM.SelectedLecturer.IsSuperAdmin);
        }
    }
}