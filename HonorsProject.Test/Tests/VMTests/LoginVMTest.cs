using System;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test.VMTest
{
    [TestClass]
    public class LoginVMTest : BaseTest
    {
        private LoginPageVM VM;
        private ISystemUser _appUser;

        public LoginVMTest() : base()
        {
            _appUser = null;
            VM = new LoginPageVM(dbConName);
        }

        [TestMethod]
        public void LoginStudentSuccess()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = "password";
            VM.UserId = 1701267;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LoginStudentInvalidIDFail()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = "password";
            VM.UserId = 1701267234;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LoginStudentInvalidPasswordFail()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = "passwordNotThis";
            VM.UserId = 1701267;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LoginStudentNullPasswordFail()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = null;
            VM.UserId = 1701267;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LoginLecturerSuccess()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = "password";
            VM.UserId = 444;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LoginLecturerInvalidIDFail()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = "password";
            VM.UserId = 4449999;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LoginLecturerInvalidPasswordFail()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = "passwordNotCorrect";
            VM.UserId = 444;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LoginLecturerNullPasswordFail()
        {
            //Arrange
            ClearDatabase();
            CreateLoginTestData();
            VM.Password = null;
            VM.UserId = 444;
            //Act
            bool result = VM.Login(ref _appUser);
            //Assert
            Assert.IsFalse(result);
        }
    }
}