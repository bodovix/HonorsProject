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

namespace HonorsProject.ViewModel
{
    class InSessoinLecturerQandAVM : BaseQandAPageVM
    {
        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Lecturer)value;
                OnPropertyChanged(nameof(User));
            }
        }


        public InSessoinLecturerQandAVM(ISystemUser appUser,Session selectedSession ,string dbcontextName) : base(dbcontextName)
        {
            //Setup
            User = UnitOfWork.LecturerRepo.Get(appUser.Id);
            UserRole = Role.Lecturer;
            IsConfirmed = false;
            QandAMode = QandAMode.Question;
            FormContextQuestion = FormContext.Create;
            SelectedSession = selectedSession;//Might need to attach this to the UoW. not sure yet
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
            ///Answers loaded when question selected
        }
        protected override bool UpdateQuestionsList(Session SelectedSession,string QuestionSearchTxt)
        {
            if (SelectedSession != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSearchForSession(SelectedSession, QuestionSearchTxt));
            else
                Questions = new ObservableCollection<Question>();
            if (Questions.Count > 0)
                return true;
            else
                return false;
        }

        protected override bool UpdateAnswersList(Question selectedQuestion, string anserSearchTxt)
        {
            if (selectedQuestion != null)
                Answers = new ObservableCollection<Answer>(UnitOfWork.AnswerRepository.GetFromSearchForQuestion(selectedQuestion, anserSearchTxt));
            else
                Answers = new ObservableCollection<Answer>();
            if (Answers.Count > 0)
                return true;
            else
                return false;
        }

        public override bool Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            //IsConfirmed is set to false in code behind for testability
            bool result = false;
            try
            {
                if (objToDelete is Question question)
                {
                    //check they can delete it.
                    //-- any lecturer can delete questions - since this is lecture VM -done.
                    //Run delete confirmation message
                    Mediator.NotifyColleagues(MediatorChannels.DeleteQuestionConfirmation.ToString(), null);
                    //delete it
                    if (IsConfirmed)
                    {
                        UnitOfWork.QuestionRepository.Remove(question);
                        result = (UnitOfWork.Complete() > 0)? true : false;
                        if (result)
                            UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                    }
                   
                }
                else if (objToDelete is Answer answer)
                {
                    //check they can delete it
                    if (answer.AnsweredBy != User)
                        throw new Exception("Only the answerer can delete this answer");
                    //run delete confirmation message
                    Mediator.NotifyColleagues(MediatorChannels.DeleteAnswerConfirmation.ToString(), null);
                    //delete it
                    if (IsConfirmed)
                    {
                        UnitOfWork.AnswerRepository.Remove(answer);
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                            UpdateAnswersList(SelectedQuestion,AnswerSearchTxt);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public override bool Save()
        {
            FeedbackMessage = "";
            bool result = false;
            try
            {
                if (FormContextAnswer == FormContext.Create)
                {
                    //create new  answer
                   result =  User.AnswerQuestion(SelectedAnswer, UnitOfWork);
                   UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                }
                else
                {
                    //Update Selected Answer
                    result = SelectedAnswer.ValidateAnswer();
                    if (result)
                       result = (UnitOfWork.Complete() >0)? true: false;
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

        public override void EnterNewMode()
        {
            FeedbackMessage = "";
            if (SelectedQuestion == null)
            {
                FeedbackMessage = "Question not selected to answer.";
                return;
            }
            if (SelectedQuestion.Id == 0)
            { 
                FeedbackMessage = "Question not selected to answer.";
                return;
            }
            //Lecturers can only create answers
            QandAMode = QandAMode.Answer;
            SelectedAnswer = new Answer();
            SelectedAnswer.Question = SelectedQuestion;
            FormContextAnswer = FormContext.Create;
        }
    }
}
