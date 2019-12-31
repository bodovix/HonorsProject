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
            throw new NotImplementedException();
        }

        public override bool UploadImage(Image imageToUpload)
        {
            throw new NotImplementedException();
        }
    }
}
