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
    internal class MyQuestionsQandAVM : BaseStudentQandA
    {
        public MyQuestionsQandAVM(ISystemUser appUser, Question selectedQuestion, string dbcontextName) : base(appUser, dbcontextName)
        {
            //Setup
            FormContextQuestion = FormContext.Update;
            //set selected question if there is one
            if (selectedQuestion != null)
                if (selectedQuestion.Id != 0)
                    SelectedQuestion = UnitOfWork.QuestionRepository.Get(selectedQuestion.Id);
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetAllForStudent((Student)User, null).ToList());
        }

        protected override bool UpdateQuestionsList(BaseEntity sSession, string questionSearchTxt)
        {
            Session selectedSession = (Session)sSession;
            if (selectedSession != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetAllForStudent((Student)User, questionSearchTxt));
            else
                Questions = new ObservableCollection<Question>();
            if (Questions.Count > 0)
                return true;
            else
                return false;
        }
    }
}