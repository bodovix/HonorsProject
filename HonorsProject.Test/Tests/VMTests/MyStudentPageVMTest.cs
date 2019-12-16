using System;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test
{
    [TestClass]
    public class MyStudentPageVMTest : BaseTest
    {
        private StudentsPageVM VM;
        private ISystemUser _appUser;

        public MyStudentPageVMTest() : base()
        {
            _appUser = null;
            VM = new StudentsPageVM(dbConName);
        }

        [TestMethod]
        public void LoginStudentSuccess()
        {
            //Arrange
            ClearDatabase();
            CreateMySessionTestData();
            Lecturer lecturer =

            //Act
            //Assert
            Assert.IsTrue(result);
        }
    }
}