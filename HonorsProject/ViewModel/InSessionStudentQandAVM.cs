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
        private string _quesitonSearchTxt;

        public override string QuestionSearchTxt
        {
            get { return _quesitonSearchTxt; }
            set
            {
                _quesitonSearchTxt = value;
                UpdateQuestionsList(QuestionSearchTxt);
                OnPropertyChanged(nameof(QuestionSearchTxt));
            }
        }

        public InSessionStudentQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(appUser, dbcontextName)
        {
            //Setup
            FormContextQuestion = FormContext.Create;//Answers loaded when question selected
            SelectedSession = UnitOfWork.SessionRepository.Get(selectedSession.Id);
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
        }

        protected override bool UpdateQuestionsList(string questionSearchTxt)
        {
            if (SelectedSession != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSearchForSession(SelectedSession, questionSearchTxt));
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
                            UpdateQuestionsList(QuestionSearchTxt);
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
            if (SelectedSession != null)
                HeaderMessage = SelectedSession.Name;
        }
    }
}