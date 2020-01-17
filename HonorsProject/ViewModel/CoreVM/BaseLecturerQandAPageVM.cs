using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseLecturerQandAPageVM : BaseQandAPageVM
    {
        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                _user = (Lecturer)value;
                QVisConDTO.User = value;
                AVisConDTO.User = value;
                OnPropertyChanged(nameof(QVisConDTO));
                OnPropertyChanged(nameof(AVisConDTO));
                OnPropertyChanged(nameof(User));
            }
        }

        public abstract override string QuestionSearchTxt { get; set; }

        public BaseLecturerQandAPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            QandAMode = QandAMode.Question;
            UserRole = Role.Lecturer;
            User = UnitOfWork.LecturerRepo.Get(appUser.Id);
            IsConfirmed = false;
            QandAMode = QandAMode.Answer;
            ImageHandler = new ImageHandler("public_html/honors/questions");
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

        public override Task<bool> UploadImage(Image imageToUpload)
        {
            throw new NotImplementedException();
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

        protected abstract override bool UpdateQuestionsList(BaseEntity entToSearchFrom, string questionSearchTxt);
    }
}