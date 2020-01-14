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
    internal class InSessoinLecturerQandAVM : BaseLecturerQandAPageVM
    {
        public InSessoinLecturerQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(appUser, dbcontextName)
        {
            //Setup
            FormContextAnswer = FormContext.Create;
            SelectedSession = selectedSession;//Might need to attach this to the UoW. not sure yet
            Questions = new ObservableCollection<Question>(UnitOfWork.QuestionRepository.GetFromSession(SelectedSession).ToList());
            ///Answers loaded when question selected
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
    }
}