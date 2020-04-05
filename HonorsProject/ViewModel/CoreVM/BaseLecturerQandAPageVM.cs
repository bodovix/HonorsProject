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
            ImageHandler = new ImageHandler("public_html/honors/images");
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
                    FormContextAnswer = FormContext.Update;//selected item now has an id go to update
                    ShowFeedback($"Successfully created: {SelectedAnswer.Id}.", FeedbackType.Success);
                }
                else
                {
                    //Update Selected Answer
                    result = SelectedAnswer.ValidateAnswer(UnitOfWork);
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
                            UpdateQuestionsList(QuestionSearchTxt);
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

        public async override Task<bool> UploadImage(Image imageToUpload)
        {
            try
            {
                ClearFeedback();
                bool ftpResult = false;
                bool dbResult = false;
                bool finalResult = false;

                if (SelectedAnswer.AnsweredBy == User)
                {
                    if (SelectedAnswer != null)
                    {
                        if (SelectedAnswer.Id == 0)
                        {
                            //if not already saved, save it. then continue if all happy
                            bool initSave = Save();
                            if (!initSave)
                                return false;
                        }
                        if (String.IsNullOrEmpty(SelectedAnswer.ImageLocation))
                        {
                            await AddImage(ftpResult, dbResult);
                        }
                        else
                        {
                            //Replace existing Question image
                            bool result = await ImageHandler.DeleteFileFromFTPServer(SelectedAnswer.ImageLocation);
                            if (result)
                                await AddImage(ftpResult, dbResult);
                            else
                                ShowFeedback("Failed to Replace Existing image. Try again or contact support", FeedbackType.Error);
                        }
                    }
                    else
                        ShowFeedback("You can only add an image to a question or answer you posted.", FeedbackType.Error);
                }
                return finalResult;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        private async Task AddImage(bool ftpResult, bool dbResult)
        {
            string originalLabel = SelectedAnswer.ImageLocation;
            //Add New Image to Question
            if (openFileDialog.ShowDialog() == true)
            {
                AnswerImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            if (AnswerImage != null)
            {
                //Save the file in FTP
                SelectedAnswer.ImageLocation = String.Concat("A-" + SelectedAnswer.Id, "-", SelectedAnswer.AnsweredBy.Id, "-", DateTime.Now.ToString("yyyyMMddHHmmss"));
                AnswerImageLabel = "Uploading Image...";
                ftpResult = await ImageHandler.WriteImageSourceAsByteArraySFTP(AnswerImage, SelectedAnswer.ImageLocation);
                if (!ftpResult)
                {
                    //if FTP fails undo everything and run away.
                    SelectedAnswer.ImageLocation = null;
                    UnitOfWork.Complete();
                }
                else
                {
                    //Then Save the file location to the database
                    dbResult = (UnitOfWork.Complete() > 0) ? true : false;
                    if (!dbResult)
                    {
                        //undo Image location and FTP Step
                        await ImageHandler.DeleteFileFromFTPServer(SelectedAnswer.ImageLocation);
                        SelectedAnswer.ImageLocation = null;
                        UnitOfWork.Complete();
                        AnswerImageLabel = originalLabel;
                    }
                    else
                        AnswerImageLabel = SelectedAnswer.ImageLocation;
                }
            }
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
                    ShowFeedback("Rolled back unsaved changes.", FeedbackType.Info);
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
                ShowFeedback("Question not selected to answer.", FeedbackType.Info);
                return;
            }
            if (SelectedQuestion.Id == 0)
            {
                ShowFeedback("Question not selected to answer.", FeedbackType.Info);
                return;
            }
            //Lecturers can only create answers
            QandAMode = QandAMode.Answer;
            SelectedAnswer = new Answer((Lecturer)User);
            SelectedAnswer.Question = SelectedQuestion;
            FormContextAnswer = FormContext.Create;
        }

        protected abstract override bool UpdateQuestionsList(string questionSearchTxt);

        public override bool ToggleMarkQuestion(Question questionToMark)
        {
            ShowFeedback("Lecturers cannot mark questions as resolved.", FeedbackType.Error);
            return false;
        }

        protected override abstract void SetHeaderMessage();
    }
}