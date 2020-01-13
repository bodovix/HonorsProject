using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.CoreVM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HonorsProject.ViewModel
{
    internal class InSessionStudentQandAVM : BaseStudentQandA
    {
        public InSessionStudentQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(appUser, dbcontextName)
        {
            //Setup
            SelectedSession = UnitOfWork.SessionRepository.Get(selectedSession.Id);//Might need to attach this to the UoW. not sure yet
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
        }

        protected override bool UpdateQuestionsList(BaseEntity sSession, string questionSearchTxt)
        {
            Session selectedSession = (Session)sSession;
            if (selectedSession != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSearchForSession(selectedSession, questionSearchTxt));
            else
                Questions = new ObservableCollection<Question>();
            if (Questions.Count > 0)
                return true;
            else
                return false;
        }
    }
}