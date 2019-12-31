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
            User = (Lecturer)appUser;
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
            throw new NotImplementedException();
        }

        public override bool MarkQuestion(Question questionToMark)
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            bool result = false;
            try
            {
                if (FormContextAnswer == FormContext.Create)
                {
                    //create new  answer
                   result =  User.AnswerQuestion(SelectedQuestion, UnitOfWork);
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
            catch(Exception ex)
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
            //Lecturers can only create answers
            QandAMode = QandAMode.Answer;
            SelectedAnswer = new Answer();
            FormContextAnswer = FormContext.Create;
        }
    }
}
