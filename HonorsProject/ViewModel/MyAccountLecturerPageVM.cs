using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class MyAccountLecturerPageVM : BaseMyAccountPageVM
    {
        private Lecturer _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Lecturer)value;
                OnPropertyChanged(nameof(User));
            }
        }

        public MyAccountLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //Initial Setup
            try
            {
                User = UnitOfWork.LecturerRepo.Get(appUser.Id);
                UserRole = Role.Lecturer;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }
    }
}