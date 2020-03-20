using System;
using HonorsProject.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test
{
    [TestClass]
    public class CommentTest
    {
        [TestMethod]
        public void Validate_Success()
        {
            //Arrange
            Question q = new Question() { Id = 4 };
            Comment expected = new Comment("comment", "ted", 1234, q);
            //Act
            bool result = expected.Validate();
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Validate_NoName_Fail()
        {
            //Arrange
            Question q = new Question() { Id = 4 };
            Comment expected = new Comment("", "ted", 1234, q);
            //Act
            bool result = expected.Validate();
            //Assert
            Assert.IsFalse(result);
        }
    }
}