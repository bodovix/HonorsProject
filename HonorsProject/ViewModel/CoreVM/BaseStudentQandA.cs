﻿using System;
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
    public abstract class BaseStudentQandA : BaseQandAPageVM
    {
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

        public abstract override string QuestionSearchTxt { get; set; }

        public BaseStudentQandA(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            UserRole = Role.Student;
            User = UnitOfWork.StudentRepo.Get(appUser.Id);
            IsConfirmed = false;
            QandAMode = QandAMode.Question;
            ImageHandler = new ImageHandler("public_html/honors/images");
        }

        public override bool Cancel()
        {
            ClearFeedback();
            if (FormContextQuestion == FormContext.Create)
                EnterNewMode();
            else
            {
                try
                {
                    UnitOfWork.Reload(SelectedQuestion);
                    UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                    OnPropertyChanged(nameof(SelectedQuestion));
                }
                catch
                {
                    EnterNewMode();
                    ShowFeedback("Unable to re-load selected Question. \n Going back to new mode.", FeedbackType.Info);
                    return false;
                }
            }
            return true;
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
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
                        int id = question.Id;
                        UnitOfWork.QuestionRepository.Remove(question);
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                        {
                            UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                            ShowFeedback($"Deleted {id}.", FeedbackType.Success);
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
            else
                return false;
        }

        public override void EnterNewMode()
        {
            ClearFeedback();
            //Lecturers can only create answers
            QandAMode = QandAMode.Question;
            SelectedQuestion = new Question((Student)User);
            SelectedQuestion.Session = SelectedSession;
            FormContextQuestion = FormContext.Create;
        }

        public override bool Save()
        {
            ClearFeedback();
            bool result = false;
            try
            {
                if (FormContextQuestion == FormContext.Create)
                {
                    //create new  answer
                    result = User.AskQuestion(SelectedQuestion, UnitOfWork);
                    UpdateQuestionsList(SelectedSession, QuestionSearchTxt);
                    ShowFeedback($"Added question: {SelectedQuestion.Name}.", FeedbackType.Success);
                }
                else
                {
                    //Update Selected Answer
                    result = SelectedQuestion.Validate();
                    if (result)
                    {
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        ShowFeedback($"Updated question: {SelectedQuestion.Name}", FeedbackType.Success);
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

        public async override Task<bool> UploadImage(Image imageToUpload)
        {
            ClearFeedback();
            bool ftpResult = false;
            bool dbResult = false;
            bool finalResult = false;

            if (SelectedQuestion.AskedBy == User)
            {
                if (SelectedQuestion != null)
                {
                    if (String.IsNullOrEmpty(SelectedQuestion.ImageLocation))
                    {
                        AddImage(ftpResult, dbResult);
                    }
                    else
                    {
                        //Replace existing Question image
                        bool result = ImageHandler.DeleteFileFromFTPServer(SelectedQuestion.ImageLocation);
                        if (result)
                            AddImage(ftpResult, dbResult);
                        else
                            ShowFeedback("Failed to Replace Existing image. Try again or contact support", FeedbackType.Error);
                    }
                }
                else
                    FeedbackMessage = "You can only add an image to a question you proposed.";
            }
            return finalResult;
        }

        private void AddImage(bool ftpResult, bool dbResult)
        {
            //Add New Image to Question
            if (openFileDialog.ShowDialog() == true)
            {
                QuestionImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            if (QuestionImage != null)
            {
                //Save the file in FTP
                SelectedQuestion.ImageLocation = String.Concat("Q-" + SelectedQuestion.Id, "-", SelectedQuestion.AskedBy.Id, "-", DateTime.Now.ToString("yyyyMMddHHmmss"));
                ftpResult = ImageHandler.WriteImageSourceAsByteArraySFTP(QuestionImage, SelectedQuestion.ImageLocation);
                if (!ftpResult)
                {
                    //if FTP fails undo everything and run away.
                    SelectedQuestion.ImageLocation = null;
                    UnitOfWork.Complete();
                }
                else
                {
                    //Then Save the file location to the database
                    dbResult = (UnitOfWork.Complete() > 0) ? true : false;
                    if (!dbResult)
                    {
                        //undo Image location and FTP Step
                        ImageHandler.DeleteFileFromFTPServer(SelectedQuestion.ImageLocation);
                        SelectedQuestion.ImageLocation = null;
                        UnitOfWork.Complete();
                    }
                }
            }
        }

        protected override abstract bool UpdateQuestionsList(BaseEntity entToSearchFrom, string questionSearchTxt);

        public abstract override bool ToggleMarkQuestion(Question questionToMark);

        protected override abstract void SetHeaderMessage();
    }
}