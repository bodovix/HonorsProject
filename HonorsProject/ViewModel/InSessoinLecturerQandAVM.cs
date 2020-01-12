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
    internal class InSessoinLecturerQandAVM : BaseQandAPageVM
    {
        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Lecturer)value;
                QVisConDTO.User = value;
                AVisConDTO.User = value;
                OnPropertyChanged(nameof(QVisConDTO));
                OnPropertyChanged(nameof(AVisConDTO));
                OnPropertyChanged(nameof(User));
            }
        }

        public InSessoinLecturerQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(dbcontextName)
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
            ImageHandler = new ImageHandler("public_html/honors/answers");
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

        protected override bool UpdateAnswersList(BaseEntity sQuestion, string anserSearchTxt)
        {
            Question selectedQuestion = (Question)sQuestion;
            if (selectedQuestion != null)
                Answers = new ObservableCollection<Answer>(UnitOfWork.AnswerRepository.GetFromSearchForQuestion(selectedQuestion, anserSearchTxt));
            else
                Answers = new ObservableCollection<Answer>();
            if (Answers.Count > 0)
                return true;
            else
                return false;
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
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
                        int id = question.Id;
                        UnitOfWork.QuestionRepository.Remove(question);
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                        {
                            UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                            ShowFeedback($"Deleted question: {id}", FeedbackType.Success);
                        }
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
                        int id = answer.Id;
                        UnitOfWork.AnswerRepository.Remove(answer);
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                        {
                            UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                            ShowFeedback($"Deleted answer: {id}", FeedbackType.Success);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool Save()
        {
            ClearFeedback();
            bool result = false;
            try
            {
                if (FormContextAnswer == FormContext.Create)
                {
                    //create new  answer
                    result = User.AnswerQuestion(SelectedAnswer, UnitOfWork);
                    UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                    ShowFeedback($"Successfully created: {SelectedAnswer.Id}.", FeedbackType.Success);
                }
                else
                {
                    //Update Selected Answer
                    result = SelectedAnswer.ValidateAnswer();
                    if (result)
                    {
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        ShowFeedback($"Successfully updated: {SelectedAnswer.Id}", FeedbackType.Success);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
            catch (SqlException ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }

            return result;
        }

        public override Task<bool> UploadImage(Image imageToUpload)
        {
            throw new NotImplementedException();
        }

        public override void EnterNewMode()
        {
            ClearFeedback();
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
            SelectedAnswer = new Answer((Lecturer)User);
            SelectedAnswer.Question = SelectedQuestion;
            FormContextAnswer = FormContext.Create;
        }

        public override bool Cancel()
        {
            ClearFeedback();
            if (FormContextAnswer == FormContext.Create)
                EnterNewMode();
            else
            {
                try
                {
                    UnitOfWork.Reload(SelectedAnswer);
                    UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                    OnPropertyChanged(nameof(SelectedAnswer));
                }
                catch
                {
                    EnterNewMode();
                    ShowFeedback("Unable to re-load selected Answer. \n Going back to new mode.", FeedbackType.Info);
                    return false;
                }
            }
            return true;
        }
    }
}