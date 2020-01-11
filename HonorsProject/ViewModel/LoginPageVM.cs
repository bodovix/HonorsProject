using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class LoginPageVM : BaseViewModel
    {
        #region Properties

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public LoginCmd LoginCmd { get; set; }
        private int? _userId;

        public int? UserId
        {
            get { return _userId; }
            set
            {
                _userId = value.Value;
                OnPropertyChanged(nameof(Password));
            }
        }

        #endregion Properties

        public LoginPageVM(string dbContextName) : base(dbContextName)
        {
            ClearFeedback();
            LoginCmd = new LoginCmd(this);
        }

        public bool Login(ref ISystemUser appUser)
        {
            if (ValidateLogin(UserId, _password))
            {
                try
                {
                    Student student = new Student();
                    Lecturer lecturer = new Lecturer();

                    student = (Student)student.Login(_userId.Value, _password, dbConName);
                    if (student != null)
                    {
                        appUser = student;
                        Mediator.NotifyColleagues(MediatorChannels.LoginAsUserX.ToString(), Role.Student);
                        return true;
                    }
                    else
                    {
                        //try lecturer
                        lecturer = (Lecturer)lecturer.Login(_userId.Value, _password, dbConName);
                        if (lecturer != null)
                        {
                            appUser = lecturer;
                            Mediator.NotifyColleagues(MediatorChannels.LoginAsUserX.ToString(), Role.Lecturer);
                            return true;
                        }
                        else
                            return ErrorFeedback("Invalid Login Credentials.");
                    }
                }
                catch (Exception ex)
                {
                    return ErrorFeedback(ex.GetBaseException().Message);
                }
            }
            else
                return false;
        }

        private bool ValidateLogin(int? userId, string password)
        {
            FeedbackMessage = "";
            if (userId == null || userId == 0)
            {
                return ErrorFeedback("ID Required");
            }
            if (String.IsNullOrEmpty(password))
            {
                return ErrorFeedback("Password Required");
            }

            return true;
        }
    }
}