using System;
using HonorsProject.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test.Repositorytests
{
    [TestClass]
    public class SessionRepositoryTest : BaseTest
    {
        public SessionRepositoryTest()
        {
            ClearDatabase();
            CreateLoginTestData();
        }

        //[TestMethod]
        //public void GetCurentSessionsForLecturer_Success()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetPreviousSessionsForLecturer_Success()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetFutureSessionsForLecturer_Success()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetCurentSessionsForLecturer_Fail()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetPreviousSessionsForLecturer_Fail()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetFutureSessionsForLecturer_Fail()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetCurentSessionsForStudent_Success()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetPreviousSessionsForStudent_Success()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetFutureSessionsForStudent_Success()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetCurentSessionsForStudent_Fail()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetPreviousSessionsForStudent_Fail()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}

        //[TestMethod]
        //public void GetFutureSessionsForStudent_Fail()
        //{
        //    //arrange
        //    //act
        //    //assert
        //}
    }
}