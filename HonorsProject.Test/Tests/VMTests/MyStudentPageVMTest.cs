using System;
using System.Linq;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test.VMTest
{
    [TestClass]
    public class MyStudentPageVMTest : BaseTest
    {
        private StudentsPageVM VM;
        private Lecturer _appUser;

        public MyStudentPageVMTest() : base()
        {
            _appUser = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234); ;
            VM = new StudentsPageVM(dbConName, _appUser);
        }

        [TestMethod]
        public void MyStudentPageVMInitialize_Success()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
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
            VM.SelectedStudent = VM.Students.Where(s => s.Id == 12345678).FirstOrDefault();
            Group studentsGroup = VM.SelectedStudent.Groups.Where(g => g.Name.Equals("NonExitstantGroup")).FirstOrDefault();
            //Act
            bool result = VM.Remove(studentsGroup);
            //Assert
            int availableGroupsCount = 1;//nothing removed
            Assert.IsFalse(result);
            Assert.AreEqual(availableGroupsCount, VM.AvailableGroups.Count);
        }
    }
}