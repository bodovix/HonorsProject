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
        private string _quesitonSearchTxt;

        public override string QuestionSearchTxt
        {
            get { return _quesitonSearchTxt; }
            set
            {
                _quesitonSearchTxt = value;
                UpdateQuestionsList((BaseEntity)User, QuestionSearchTxt);
                OnPropertyChanged(nameof(QuestionSearchTxt));
            }
        }

        public MyQuestionsQandAVM(ISystemUser appUser, Question selectedQuestion, string dbcontextName) : base(appUser, dbcontextName)
        {
            //Setup
            //set selected question if there is one
            if (selectedQuestion != null)
                if (selectedQuestion.Id != 0)
                {
                    SelectedQuestion = UnitOfWork.QuestionRepository.Get(selectedQuestion.Id);
                    FormContextQuestion = FormContext.Update;
                }
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetAllForStudent((Student)User, null).ToList());
        }

        protected override bool UpdateQuestionsList(BaseEntity sStudent, string questionSearchTxt)
        {
            Student student = (Student)sStudent;
            if (student != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetAllForStudent((Student)student, questionSearchTxt));
            else
                Questions = new ObservableCollection<Question>();
            if (Questions.Count > 0)
                return true;
            else
                return false;
        }

        public override bool ToggleMarkQuestion(Question questionToMark)
        {
            ClearFeedback();
            try
            {
                bool result = false;
                if (questionToMark != null)
                {
                    if (questionToMark.Id > 0)
                    {
                        //toggle is Resolved for question
                        questionToMark.IsResolved = !questionToMark.IsResolved;
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        string output = (questionToMark.IsResolved) ? "resolved" : "still open";
                        if (result == false)
                            ShowFeedback($"Unable to mark question as {output}.", FeedbackType.Error);
                        else
                        {
                            UpdateQuestionsList((Student)User, QuestionSearchTxt);
                            OnPropertyChanged(nameof(SelectedQuestion));
                            ShowFeedback($"Marked as {output}.", FeedbackType.Success);
                        }
                    }
                    else
                        ShowFeedback("New questions cannot be marked.", FeedbackType.Error);
                }
                else
                    ShowFeedback("No question selected.", FeedbackType.Error);
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        protected override void SetHeaderMessage()
        {
            if (SelectedQuestion != null)
                SelectedSession = SelectedQuestion.Session;
            if (SelectedSession != null)
                HeaderMessage = SelectedSession.Name;
        }
    }
}