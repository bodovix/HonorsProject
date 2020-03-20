using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Data;

namespace HonorsProject.Test
{
    [TestClass]
    public class AnswerTest : BaseTest
    {
        [TestMethod]
        public void ShallowCopy_Success()
        {
            //Arrange
            Lecturer l = new Lecturer() { Id = 2 };
            Question q = new Question() { Id = 4 };
            DateTime date = new DateTime(2020, 01, 01);
            Answer original = new Answer("name", "answer", true, l, q, date);
            Answer expected = new Answer();
            //Act
            expected.ShallowCopy(original);
            //Assert
            Assert.AreNotEqual(expected, original);
            Assert.AreEqual(expected.Id, original.Id);
            Assert.AreEqual(expected.Name, original.Name);
            Assert.AreEqual(expected.AnswerTest, original.AnswerTest);
            Assert.AreEqual(expected.WasHelpfull, original.WasHelpfull);
            Assert.AreEqual(expected.AnsweredBy, original.AnsweredBy);
            Assert.AreEqual(expected.Question, original.Question);
            Assert.AreEqual(expected.ImageLocation, original.ImageLocation);
            Assert.AreEqual(expected.CreatedOn, original.CreatedOn);
        }

        [TestMethod]
        public void Constructor_Success()
        {
            //arrange
            Lecturer l = new Lecturer() { Id = 2 };
            Question q = new Question() { Id = 4 };
            DateTime date = new DateTime(2020, 01, 01);
            //act
            Answer expected = new Answer("name", "answer", true, l, q, date);
            //Assert
            Assert.AreEqual(expected.Id, 0);
            Assert.AreEqual(expected.Name, "name");
            Assert.AreEqual(expected.AnswerTest, "answer");
            Assert.AreEqual(expected.WasHelpfull, true);
            Assert.AreEqual(expected.AnsweredBy, l);
            Assert.AreEqual(expected.Question, q);
            Assert.AreEqual(expected.CreatedOn, date);
        }

        [TestMethod]
        public void ConstructorSmall_Success()
        {
            //Arrange
            Lecturer l = new Lecturer() { Id = 2 };
            DateTime date = new DateTime(2020, 01, 01);
            //Act
            Answer expected = new Answer(l);
            //Assert
            Assert.AreEqual(expected.AnsweredBy, l);
        }

        [TestMethod]
        public void Validate_Success()
        {
            //Arrange
            Lecturer l = new Lecturer() { Id = 2 };
            Question q = new Question() { Id = 4 };
            DateTime date = new DateTime(2020, 01, 01);
            Answer expected = new Answer("name", "answer", true, l, q, date);
            //Act
            using (UnitOfWork uow = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                bool result = expected.ValidateAnswer(uow);
                //Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Validate_NoName_Fail()
        {
            //Arrange
            Lecturer l = new Lecturer() { Id = 2 };
            Question q = new Question() { Id = 4 };
            DateTime date = new DateTime(2020, 01, 01);
            Answer expected = new Answer("", "answer", true, l, q, date);
            //Act
            using (UnitOfWork uow = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                bool result = expected.ValidateAnswer(uow);
                //Assert
                Assert.IsFalse(result);
            }
        }
    }
}