﻿using System;
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
    public class MyStudentPageVMTest : BaseTest
    {
        private StudentsPageVM VM;
        private Lecturer _appUser;

        public MyStudentPageVMTest() : base()
        {
            _appUser = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", true, new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234); ;
        }

        [TestMethod]
        public void MyStudentPageVMInitialize_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);

            //Act
            //VM initialize is the act
            //Assert
            FormContext formContext = FormContext.Create;
            SubgridContext subgridContext = SubgridContext.Groups;
            bool isConfirmed = false;
            int ssId = 0;
            int lecId = 444;
            int availableGroupCount = 2;
            int studentCount = 3;
            Assert.AreEqual(formContext, VM.FormContext);
            Assert.AreEqual(isConfirmed, VM.IsConfirmed);
            Assert.AreEqual(subgridContext, VM.SubgridContext);
            Assert.AreEqual(ssId, VM.SelectedStudent.Id);
            Assert.AreEqual(lecId, VM.Lecturer.Id);
            Assert.AreEqual(availableGroupCount, VM.AvailableGroups.Count);
            Assert.AreEqual(studentCount, VM.Students.Count);
        }

        [TestMethod]
        public void Remove_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);

            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            Group studentsGroup = VM.SelectedStudent.Groups.Where(g => g.Name.Equals("Computing 19/20")).FirstOrDefault();
            //Act
            bool result = VM.Remove(studentsGroup);
            //Assert
            int remaingStudentGroupsCount = 1;
            int availableGroupsCount = 1;//was in 2, we removed 1
            Assert.IsTrue(result);
            Assert.AreEqual(remaingStudentGroupsCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(availableGroupsCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void Remove_GroupNotFound_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 12345678).FirstOrDefault();
            Group studentsGroup = VM.SelectedStudent.Groups.Where(g => g.Name.Equals("NonExitstantGroup")).FirstOrDefault();
            //Act
            bool result = VM.Remove(studentsGroup);
            //Assert
            int availableGroupsCount = 1;//nothing removed
            Assert.IsFalse(result);
            Assert.AreEqual(availableGroupsCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void EnterNewMode_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 12345678).FirstOrDefault();
            //Act
            VM.EnterNewMode();
            //Assert
            int ssId = 0;
            int availableGroupCount = 2;
            Assert.AreEqual(FormContext.Create, VM.FormContext);
            Assert.AreEqual(ssId, VM.SelectedStudent.Id);
            Assert.AreEqual(availableGroupCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void Save_New_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "Student", "Student@uad.ac.uk", "Password", DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Create;
            //Act
            bool result = VM.Save();
            int newScount = 4;
            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void Save_New_InvalidStudent_NoPassword_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "Student", "Student@uad.ac.uk", null, DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Create;
            //Act
            bool result = VM.Save();
            int newScount = 3;
            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void Save_New_InvalidStudent_NoName_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "", "Student@uad.ac.uk", "password", DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Create;
            //Act
            bool result = VM.Save();
            int newScount = 3;
            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void Save_New_InvalidStudent_NoEmail_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "Student", null, "password", DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Create;
            //Act
            bool result = VM.Save();
            int newScount = 3;
            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void Save_Update_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Update;
            //Act
            VM.SelectedStudent.Name = "UpdatedValue";
            bool result = VM.Save();
            int newScount = 3;
            string newName = "UpdatedValue";
            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newScount, VM.Students.Count);
            Assert.IsTrue(newName.Equals(VM.SelectedStudent.Name));
        }

        [TestMethod]
        public void Save_Update_InvalidStudent_NoPassword_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Update;
            //Act
            VM.SelectedStudent.Password = null;
            bool result = VM.Save();
            int newScount = 3;
            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void Save_Update_InvalidStudent_NoName_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "", "Student@uad.ac.uk", "password", DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Update;
            //Act
            bool result = VM.Save();
            int newScount = 3;
            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void BLANKTEST_RESET()
        {
        }

        [TestMethod]
        public void Save_Update_InvalidStudent_NoEmail_Fail()
        {
            //Arrange
            Lecturer _appUser2 = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", true, new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234); ;
            StudentsPageVM VM2 = new StudentsPageVM(dbConName, new Student(), _appUser2);

            ClearDatabase();
            CreateMySessionTestData(_appUser2);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM2.SelectedStudent = VM.UnitOfWork.StudentRepo.Get(444);
            VM2.SelectedStudent.Name = "Updated";
            VM2.FormContext = FormContext.Update;
            //Act
            bool result = VM2.Save();
            int newScount = 3;
            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newScount, VM.Students.Count);
        }

        [TestMethod]
        public void GenerateNewPasswordHash_Create_Random_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "Student", null, "password", DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Create;
            VM.IsConfirmed = true;
            //Act
            bool result = VM.GenerateNewPasswordHash("");//randomly generate it
            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(VM.SelectedStudent.Password.Contains("HASH"));
        }

        [TestMethod]
        public void GenerateNewPasswordHash_Rejected_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = new Student(999, "Student", null, "password", DateTime.Now.Date, 444);
            VM.FormContext = FormContext.Create;
            VM.IsConfirmed = false;
            //Act
            bool result = VM.GenerateNewPasswordHash("");//randomly generate it
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MoveEntityOutOfList_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityOutOfList(VM.SelectedStudent.Groups.First());
            //Assert
            int inCount = 1;
            int outCount = 1;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityOutOfListTwice_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityOutOfList(VM.SelectedStudent.Groups.First());
            VM.MoveEntityOutOfList(VM.SelectedStudent.Groups.First());
            //Assert
            int inCount = 0;
            int outCount = 2;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityOutOfList_GroupNotInList_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityOutOfList(new Group());
            //Assert
            int inCount = 2;
            int outCount = 0;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityOutOfList_NullGroup_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityOutOfList(null);
            //Assert
            int inCount = 2;
            int outCount = 0;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityInToList_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityInToList(VM.SelectedStudent.Groups.First());
            //Assert
            int inCount = 2;
            int outCount = 0;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityInToListTwice_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityInToList(VM.SelectedStudent.Groups.First());
            VM.MoveEntityInToList(VM.SelectedStudent.Groups.First());
            //Assert
            int inCount = 2;
            int outCount = 0;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityInToList_GroupAlreadyInList_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityInToList(VM.SelectedStudent.Groups.First());
            //Assert
            int inCount = 2;
            int outCount = 0;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void MoveEntityInToList_NullGroup_Fail()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            VM.FormContext = FormContext.Create;
            //Act
            VM.MoveEntityInToList(null);
            //Assert
            int inCount = 2;
            int outCount = 0;
            Assert.AreEqual(inCount, VM.SelectedStudent.Groups.Count);
            Assert.AreEqual(outCount, VM.AvailableGroups.Count);
        }

        [TestMethod]
        public void Delete_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            VM = new StudentsPageVM(dbConName, new Student(), _appUser);
            VM.IsConfirmed = true;
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 1701267).FirstOrDefault();
            //Act
            VM.Delete(VM.SelectedStudent);
            //Assert
            int studentCount = 2;
            Assert.AreEqual(studentCount, VM.Students.Count);
        }
    }
}