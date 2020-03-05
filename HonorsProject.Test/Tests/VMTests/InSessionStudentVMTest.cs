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
    public class InSessionStudentVMTest : BaseTest
    {
        private InSessionStudentQandAVM VM;
        private Student _appUser;

        public InSessionStudentVMTest() : base()
        {
            _appUser = new Student(1701267, "Gwydion", "1701267@uad.ac.uk", "password", new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234); ;
        }

        [TestMethod]
        public void MyStudentPageVMInitialize_Success()
        {
            //Arrange
            ClearDatabase();
            Session selectedSession = CreateInSessionTestData(SubgridContext.ActiveSessions);

            VM = new InSessionStudentQandAVM(_appUser, selectedSession, dbConName);
            VM.SelectedQuestion = VM.Questions.FirstOrDefault();
            VM.CommentText = "its a test comment";
            int expected = 3;
            int actual = VM.SelectedQuestion.Comments.Count;
            //Act
            bool result = VM.Post();

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual, "wrong comment count");
        }
    }
}