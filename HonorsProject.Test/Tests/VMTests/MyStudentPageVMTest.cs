using System;
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
    }
}