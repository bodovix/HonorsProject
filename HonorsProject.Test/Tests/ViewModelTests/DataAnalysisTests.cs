using System;
using System.Collections.Generic;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test.ViewModel
{
    [TestClass]
    public class DataAnalysisTests : BaseTest
    {
        private DataAnalysisVM VM;
        private BaseEntity entityToFucusOn = null;

        public DataAnalysisTests() : base()
        {
            VM = new DataAnalysisVM(entityToFucusOn, dbConName);
        }

        [TestMethod]
        public void Constructor_Empty_Success()
        {
            //Arrange
            //--already constructed
            //Act
            //--act in the constructor
            //Assert
            Assert.IsNull(VM.CommonPhrases);
            Assert.AreEqual(2, VM.Groups.Count);
            Assert.IsNull(VM.MostFrequentAskers);
            Assert.AreEqual(0, VM.NumQuestionsAsked);
            Assert.IsNull(VM.SelectedSession);
            Assert.IsNull(VM.SelectedGroup);
            Assert.AreEqual("No Groups or Sessions selected.", VM.SelectionTitle);
        }

        [TestMethod]
        public void Constructor_Success()
        {
            //Arrange
            ClearDatabase();
            CreateInSessionTestData(SubgridContext.ActiveSessions);
            Session selected = VM.UnitOfWork.SessionRepository.Get(1);
            VM = new DataAnalysisVM((BaseEntity)selected, dbConName);
            //Act
            //--act in the constructor
            //Assert
            Assert.AreEqual(3, VM.CommonPhrases.Count);
            Assert.AreEqual(2, VM.Groups.Count);
            Assert.AreEqual(1, VM.MostFrequentAskers.Count);
            Assert.AreEqual(2, VM.NumQuestionsAsked);
            Assert.AreEqual(1, VM.SelectedSession.Id);
            Assert.AreEqual(1, VM.SelectedGroup.Id);
            Assert.AreEqual("Computing 19/20", VM.SelectionTitle);
        }

        [TestMethod]
        public void KeyWordsAPI_Success()
        {
            //Arrange
            ClearDatabase();

            CreateInSessionTestDataForKeyWordsAPI();
            Session selected = VM.UnitOfWork.SessionRepository.Get(1);
            VM = new DataAnalysisVM((BaseEntity)selected, dbConName);
            //Act
            Dictionary<string, int> result = VM.KeyWordsAPI();
            //Assert
            Assert.IsTrue(result.Count == 8);
        }

        [TestMethod]
        public void KeyWordsAPI_NoOrEmpty_Fail()
        {
            //Arrange
            ClearDatabase();

            CreateInSessionTestDataForKeyWordsAPINoKEYWORDS();
            Session selected = VM.UnitOfWork.SessionRepository.Get(1);
            VM = new DataAnalysisVM((BaseEntity)selected, dbConName);
            //Act
            Dictionary<string, int> result = VM.KeyWordsAPI();
            //Assert
            Assert.IsNull(result);
        }
    }
}