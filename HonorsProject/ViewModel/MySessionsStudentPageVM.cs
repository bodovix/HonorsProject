using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsStudentPageVM : BaseMySessionsPageVM
    {
        #region Properties

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Student)value;
                OnPropertyChanged(nameof(User));
            }
        }

        #endregion Properties

        public MySessionsStudentPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //initial setup
            User = (Student)appUser;
            UserRole = Role.Student;

            GetAllLecturers();
            SelectedSession = new Session();
            FormContext = FormContext.Create;
            GetAllGroups();
            //initially loads current sessions
            GetAllMyCurrentSessions();
        }

        public override bool Save()
        {
            ShowFeedback("Students Cannot Save Sessions.", FeedbackType.Error);
            return false;
        }

        public override bool Delete(BaseEntity objectToDelete)
        {
            ShowFeedback("Students Cannot Delete Sessions", FeedbackType.Error);
            return false;
        }

        public override void EnterNewMode()
        {
            ShowFeedback("Students Cannot Create New Sessions", FeedbackType.Error);
        }

        public override bool AddLecturer()
        {
            ShowFeedback("Students cannot add lecturers to sessions", FeedbackType.Error);
            return false;
        }

        public override bool Remove(BaseEntity entityToRemove)
        {
            ShowFeedback("Students cannot remove lecturers from sessions", FeedbackType.Error);
            return false;
        }

        public override bool GoToAnalyseEntity(BaseEntity entity)
        {
            ShowFeedback("Students Cannot Analyse Sessions.", FeedbackType.Error);
            return false;
        }
    }
}