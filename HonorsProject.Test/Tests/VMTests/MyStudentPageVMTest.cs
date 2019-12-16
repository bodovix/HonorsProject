using System;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test
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
        public void LoginStudentSuccess()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData(_appUser);
            //Act
            //Assert
        }
    }
}