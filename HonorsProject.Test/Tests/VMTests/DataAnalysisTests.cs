using System;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test.VMTest
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
        public void TextRazorSucces()
        {
            //Arrange

            //Act
            string result = VM.GetKeyPhraseAPI();
            //Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result));
        }
    }
}