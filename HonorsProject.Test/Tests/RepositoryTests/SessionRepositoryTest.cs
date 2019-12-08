using System;
using System.Collections.Generic;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HonorsProject.Test.Repositorytests
{
    [TestClass]
    public class SessionRepositoryTest : BaseTest
    {
        private Lecturer _lecturer;
        private SessionRepository sessionRepo;

        public SessionRepositoryTest()
        {
            _lecturer = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234);
            sessionRepo = new SessionRepository(new LabAssistantContext(dbConName));
            ClearDatabase();
            CreateMySessionTestData(_lecturer);
        }

        [TestMethod]
        public void GetCurentSessionsForLecturer_Success()
        {
            //arrange
            int expected = 2; //todays session and the long running one
            //act
            List<Session> results = sessionRepo.GetCurrentSessions(_lecturer, DateTime.Now.Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetCurentSessionsForLecturer_Date_Fail()
        {
            //arrange
            int expected = 0;
            //act
            List<Session> results = sessionRepo.GetCurrentSessions(_lecturer, DateTime.Now.AddYears(-2).Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetCurentSessionsForLecturer_Lecturer_Fail()
        {
            //arrange
            _lecturer.Id = -1;
            int expected = 0;
            //act
            List<Session> results = sessionRepo.GetCurrentSessions(_lecturer, DateTime.Now.Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetPreviousSessionsForLecturer_Success()
        {
            //arrange
            int expected = 1; //todays session and the long running one
            //act
            List<Session> results = sessionRepo.GetPreviousSessions(_lecturer, DateTime.Now.Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetPreviousSessionsForLecturer_Date_Fail()
        {
            //arrange
            int expected = 0;
            //act
            List<Session> results = sessionRepo.GetPreviousSessions(_lecturer, DateTime.Now.AddYears(-2).Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetPreviousSessionsForLecturer_Lecturer_Fail()
        {
            //arrange
            int expected = 0;
            _lecturer.Id = -1;
            //act
            List<Session> results = sessionRepo.GetPreviousSessions(_lecturer, DateTime.Now.Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetFutureSessionsForLecturer_Success()
        {
            //arrange
            int expected = 2;
            //act
            List<Session> results = sessionRepo.GetFutureSessions(_lecturer, DateTime.Now.Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetFutureSessionsForLecturer_Date_Fail()
        {
            //arrange
            int expected = 0;
            //act
            List<Session> results = sessionRepo.GetFutureSessions(_lecturer, DateTime.Now.AddYears(2).Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

        [TestMethod]
        public void GetFutureSessionsForLecturer_Lecturer_Fail()
        {
            //arrange
            int expected = 0;
            _lecturer.Id = -1;
            //act
            List<Session> results = sessionRepo.GetFutureSessions(_lecturer, DateTime.Now.Date);
            //assert
            Assert.AreEqual(expected, results.Count);
        }

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