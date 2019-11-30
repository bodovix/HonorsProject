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

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

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
            ErrorMessage = "";
            LoginCmd = new LoginCmd(this);
        }

        public bool Login()
        {
            if (ValidateLogin(UserId, _password))
            {
                try
                {
                    Student student = new Student();
                    Lecturer tmpLecturer = new Lecturer();
                    //
                    //tmpLecturer.Id = 444;
                    //tmpLecturer.Name = "Suzy";
                    //tmpLecturer.Email = "lecturer1@uad.ac.uk";
                    //tmpLecturer.Password = "password";
                    //tmpLecturer.CreatedOn = DateTime.Now;
                    //tmpLecturer.CreatedByLecturerId = 1234;
                    //tmpLecturer.Register(tmpLecturer, dbConName);

                    //
                    student = (Student)student.Login(_userId.Value, _password, dbConName);
                    if (student != null)
                    {
                        Mediator.NotifyColleagues("GoToMyScenarioPage", Role.Student);
                        return true;
                    }
                    else
                    {
                        //try lecturer
                        tmpLecturer = (Lecturer)tmpLecturer.Login(_userId.Value, _password, dbConName);
                        if (tmpLecturer != null)
                        {
                            Mediator.NotifyColleagues("GoToMyScenarioPage", Role.Lecturer);
                            return true;
                        }
                        else
                        {
                            ErrorMessage = "Invalid Login Credentials.";
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.GetBaseException().Message;
                    return false;
                }
            }
            else
                return false;
        }

        private bool ValidateLogin(int? userId, string password)
        {
            ErrorMessage = "";
            if (userId == null || userId == 0)
            {
                ErrorMessage = "ID Required";
                return false;
            }
            if (String.IsNullOrEmpty(password))
            {
                ErrorMessage = "Password Required";
                return false;
            }

            return true;
        }
    }
}