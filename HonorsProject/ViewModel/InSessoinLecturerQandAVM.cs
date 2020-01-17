using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;
using HonorsProject.Model.Enums;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using HonorsProject.Model.HelperClasses;
using HonorsProject.Model.DTO;

namespace HonorsProject.ViewModel
{
    internal class InSessoinLecturerQandAVM : BaseLecturerQandAPageVM
    {
        private string _quesitonSearchTxt;

        public override string QuestionSearchTxt
        {
            get { return _quesitonSearchTxt; }
            set
            {
                _quesitonSearchTxt = value;
                UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                OnPropertyChanged(nameof(QuestionSearchTxt));
            }
        }

        public InSessoinLecturerQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(appUser, dbcontextName)
        {
            try
            {
                //Setup
                FormContextAnswer = FormContext.Create;
                SelectedSession = selectedSession;//Might need to attach this to the UoW. not sure yet
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
                ///Answers loaded when question selected
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        public InSessoinLecturerQandAVM(ISystemUser appUser, Question selectedQuestion, string dbcontextName) : base(appUser, dbcontextName)
        {
            try
            {
                //Setup
                if (selectedQuestion != null)
                    if (selectedQuestion.Id != 0)
                    {
                        SelectedSession = UnitOfWork.SessionRepository.GetSessionWithQuestion(selectedQuestion);//Might need to attach this to the UoW. not sure yet
                        SelectedQuestion = UnitOfWork.QuestionRepository.Get(selectedQuestion.Id);
                        QandAMode = QandAMode.Question;
                        Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
                        ///Answers loaded when question selected
                    }
                    else
                    {
                        ShowFeedback($"Cannot load session for question with id value: {selectedQuestion.Id}", FeedbackType.Error);
                    }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        protected override bool UpdateQuestionsList(BaseEntity sSession, string QuestionSearchTxt)
        {
            Session selectedSession = (Session)sSession;
            if (selectedSession != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSearchForSession(selectedSession, QuestionSearchTxt));
            else
                Questions = new ObservableCollection<Question>();
            if (Questions.Count > 0)
                return true;
            else
                return false;
        }
    }
}