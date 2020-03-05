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

        #region Comments

        [TestMethod]
        public void PostComment_Success()
        {
            //Arrange
            ClearDatabase();
            Session selectedSession = CreateInSessionTestData(SubgridContext.ActiveSessions);

            VM = new InSessionStudentQandAVM(_appUser, selectedSession, dbConName);
            VM.SelectedQuestion = VM.Questions.FirstOrDefault();
            VM.CommentText = "its a test comment";
            int expected = 5;
            //Act
            bool result = VM.Post();
            int actual = VM.Comments.Count;

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual, "wrong comment count");
        }

        [TestMethod]
        public void PostComment_NoComment_Fail()
        {
            //Arrange
            ClearDatabase();
            Session selectedSession = CreateInSessionTestData(SubgridContext.ActiveSessions);

            VM = new InSessionStudentQandAVM(_appUser, selectedSession, dbConName);
            VM.SelectedQuestion = VM.Questions.FirstOrDefault();
            VM.CommentText = "";
            int expected = 5;
            //Act
            bool result = VM.Post();
            int actual = VM.SelectedQuestion.Comments.Count;

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PostComment_NoQuesion_Fail()
        {
            //Arrange
            ClearDatabase();
            Session selectedSession = CreateInSessionTestData(SubgridContext.ActiveSessions);

            VM = new InSessionStudentQandAVM(_appUser, selectedSession, dbConName);
            VM.SelectedQuestion = null;
            VM.CommentText = "its a test comment";
            //Act
            bool result = VM.Post();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteComment_Success()
        {
            //Arrange
            ClearDatabase();
            Session selectedSession = CreateInSessionTestData(SubgridContext.ActiveSessions);

            VM = new InSessionStudentQandAVM(_appUser, selectedSession, dbConName);
            VM.SelectedQuestion = VM.Questions.FirstOrDefault();
            VM.SelectedComment = VM.Comments.Where(c => c.PostedById == _appUser.Id).FirstOrDefault();
            int expected = 3;
            //Act
            bool result = VM.DeleteComent(VM.SelectedComment);
            int actual = VM.Comments.Count;

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual, "wrong comment count");
        }

        [TestMethod]
        public void EditComment_Success()
        {
            //Arrange
            ClearDatabase();
            Session selectedSession = CreateInSessionTestData(SubgridContext.ActiveSessions);

            VM = new InSessionStudentQandAVM(_appUser, selectedSession, dbConName);
            VM.SelectedQuestion = VM.Questions.FirstOrDefault();
            VM.SelectedComment = VM.SelectedQuestion.Comments
                .Where(c => c.PostedById == _appUser.Id)
                .FirstOrDefault();
            VM.CommentText = "its a UPDATE test comment";
            int expected = 4;
            //Act
            bool result = VM.EditComment(VM.SelectedComment);
            int actual = VM.Comments.Count;

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual, "wrong comment count");
            Assert.IsTrue(VM.SelectedComment.CommentText.Contains("UPDATE"));
        }

        #endregion Comments
    }
}