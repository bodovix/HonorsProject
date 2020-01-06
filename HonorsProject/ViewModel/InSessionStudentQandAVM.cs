using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
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
            SelectedSession = selectedSession;//Might need to attach this to the UoW. not sure yet
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
            throw new NotImplementedException();
        }

        public override void EnterNewMode()
        {
            throw new NotImplementedException();
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
                    result = User.AskQuestion(SelectedQuestion, UnitOfWork);
                    UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
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