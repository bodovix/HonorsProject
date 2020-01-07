using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    internal class InSessionStudentQandAVM : BaseQandAPageVM
    {
        public InSessionStudentQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(dbcontextName)
        {
            //Setup
            User = UnitOfWork.StudentRepo.Get(appUser.Id);
            UserRole = Role.Student;
            IsConfirmed = false;
            QandAMode = QandAMode.Question;
            FormContextQuestion = FormContext.Create;
            SelectedSession = UnitOfWork.SessionRepository.Get(selectedSession.Id);//Might need to attach this to the UoW. not sure yet
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
            ///Answers loaded when question selected
        }

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                _user = (Student)value;
                QVisConDTO.User = value;
                AVisConDTO.User = value;
                OnPropertyChanged(nameof(QVisConDTO));
                OnPropertyChanged(nameof(AVisConDTO));
                OnPropertyChanged(nameof(User));
            }
        }

        public override bool Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            if (objToDelete is Question question)
            {
                bool result = false;
                try
                {
                    //check they can delete it.
                    if (question.AskedBy.Id != User.Id)
                        throw new Exception("Can only delete your question.");
                    //Run delete confirmation message
                    Mediator.NotifyColleagues(MediatorChannels.DeleteAnswerConfirmation.ToString(), null);
                    //delete it
                    if (IsConfirmed)
                    {
                        UnitOfWork.QuestionRepository.Remove(question);
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                            UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    FeedbackMessage = ex.Message;
                    return false;
                }
            }
            else
                return false;
        }

        public override void EnterNewMode()
        {
            FeedbackMessage = "";
            //Lecturers can only create answers
            QandAMode = QandAMode.Question;
            SelectedQuestion = new Question((Student)User);
            SelectedQuestion.Session = SelectedSession;
            FormContextQuestion = FormContext.Create;
        }

        public override bool Save()
        {
            FeedbackMessage = "";
            bool result = false;
            try
            {
                if (FormContextQuestion == FormContext.Create)
                {
                    //create new  answer
                    result = User.AskQuestion(SelectedQuestion, UnitOfWork);
                    UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                }
                else
                {
                    //Update Selected Answer
                    result = SelectedQuestion.Validate();
                    if (result)
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                }
            }
            catch (DbUpdateException e)
            {
                FeedbackMessage = e.Message;
            }
            catch (SqlException e)
            {
                FeedbackMessage = e.Message;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
            }
            return result;
        }

        public override bool UploadImage(Image imageToUpload)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateAnswersList(BaseEntity sQuestion, string answerSearchTxt)
        {
            Question selectedQuestion = (Question)sQuestion;
            if (selectedQuestion != null)
                Answers = new ObservableCollection<Answer>(UnitOfWork.AnswerRepository.GetFromSearchForQuestion(selectedQuestion, answerSearchTxt));
            else
                Answers = new ObservableCollection<Answer>();
            if (Answers.Count > 0)
                return true;
            else
                return false;
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