using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class MyAnswersQandAVM : BaseLecturerQandAPageVM
    {
        private string _quesitonSearchTxt;

        public override string QuestionSearchTxt
        {
            get { return _quesitonSearchTxt; }
            set
            {
                _quesitonSearchTxt = value;
                UpdateQuestionsList((BaseEntity)User, QuestionSearchTxt);
                OnPropertyChanged(nameof(QuestionSearchTxt));
            }
        }

        public MyAnswersQandAVM(ISystemUser appUser, Answer selectedAnswer, string dbcontextName) : base(appUser, dbcontextName)
        {
            //Setup
            if (selectedAnswer.Id == 0)
                FormContextAnswer = FormContext.Create;
            else
                FormContextAnswer = FormContext.Update;
            SelectedQuestion = selectedAnswer.Question;
            SelectedAnswer = selectedAnswer;//Might need to attach this to the UoW. not sure yet
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetAllWithAnswersByLecturer(User, QuestionSearchTxt).ToList());
        }

        protected override bool UpdateQuestionsList(BaseEntity user, string questionSearchTxt)
        {
            Lecturer u = (Lecturer)user;
            if (u != null)
                Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetAllWithAnswersByLecturer(u, QuestionSearchTxt));
            else
                Questions = new ObservableCollection<Question>();
            return (Questions.Count > 0) ? true : false;
        }

        protected override void SetHeaderMessage()
        {
            if (SelectedQuestion != null)
                SelectedSession = SelectedQuestion.Session;
            if (SelectedSession != null)
                HeaderMessage = SelectedSession.Name;
        }

        //all answers for selected question are loaded - makes sense to be able to see other
        //lecturers answers before you create /update yours.
    }
}