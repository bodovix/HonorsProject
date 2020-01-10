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

        public async override Task<bool> UploadImage(Image imageToUpload)
        {
            bool ftpResult;
            bool dbResult;
            bool finalResult = false;

            if (SelectedQuestion.AskedBy == User)
            {
                if (SelectedQuestion != null)
                {
                    if (String.IsNullOrEmpty(SelectedQuestion.ImageLocation))
                    {
                        //Add New Image to Question
                        if (openFileDialog.ShowDialog() == true)
                        {
                            QuestionImage = new BitmapImage(new Uri(openFileDialog.FileName));
                        }
                        if (QuestionImage != null)
                        {
                            //Save the file in FTP
                            SelectedQuestion.ImageLocation = String.Concat(SelectedQuestion.Id, "-", SelectedQuestion.AskedBy.Id);
                            ftpResult = SaveImageToFTPServer(SelectedQuestion.ImageLocation);
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
                                    DeleteImageFromFTPServer(SelectedQuestion.ImageLocation);
                                    SelectedQuestion.ImageLocation = null;
                                    UnitOfWork.Complete();
                                }
                            }
                        }
                    }
                    else
                    {
                        //Replace existing Question image
                    }
                }
                else
                    FeedbackMessage = "You can only add an image you a question you proposed.";
            }
            return finalResult;
        }

        private BitmapImage DownloadImageFromSFTP(string imageLocation)
        {
            string host = "mayar.abertay.ac.uk";
            int port = 22;
            string username = "1701267";
            string password = "123Haggis0nToast123$";

            BitmapEncoder encoder = new TiffBitmapEncoder();
            byte[] biteArray = ImageSourceToBytes(encoder, QuestionImage); // Function returns byte[] csv file

            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("public_html/honors/questions");
                    using (MemoryStream fileStream = new MemoryStream())
                    {
                        client.DownloadFile(client.WorkingDirectory + "/" + imageLocation, fileStream);
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = fileStream;
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                }
                else
                {
                    FeedbackMessage = "I couldn't connect";
                    return null;
                }
            }
        }

        private bool SaveImageToFTPServer(string imageLocation)
        {
            string host = "mayar.abertay.ac.uk";
            int port = 22;
            string username = "1701267";
            string password = "123Haggis0nToast123$";

            BitmapEncoder encoder = new TiffBitmapEncoder();
            byte[] biteArray = ImageSourceToBytes(encoder, QuestionImage); // Function returns byte[] csv file

            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("public_html/honors/questions");
                    using (var ms = new MemoryStream(biteArray))
                    {
                        client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                        client.UploadFile(ms, openFileDialog.FileName);
                        client.RenameFile(client.WorkingDirectory + "/" + openFileDialog.FileName, client.WorkingDirectory + "/" + imageLocation);
                        return true;
                    }
                }
                else
                {
                    FeedbackMessage = "I couldn't connect";
                    return false;
                }
            }
        }

        private byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        private bool DeleteImageFromFTPServer(string imageLocation)
        {
            string host = "mayar.abertay.ac.uk";
            int port = 22;
            string username = "1701267";
            string password = "123Haggis0nToast123$";

            BitmapEncoder encoder = new TiffBitmapEncoder();
            byte[] biteArray = ImageSourceToBytes(encoder, QuestionImage);

            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("public_html/honors/questions");
                    using (var ms = new MemoryStream(biteArray))
                    {
                        client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                        client.DeleteFile(client.WorkingDirectory + "/" + imageLocation);
                    }
                    return true;
                }
                else
                {
                    FeedbackMessage = "I couldn't connect to SFTP Server";
                    return false;
                }
            }
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

        public override bool Cancel()
        {
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
                    FeedbackMessage = "Unable to re-load selected Question. \n Going back to new mode.";
                    return false;
                }
            }
            return true;
        }
    }
}