using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        internal void Login()
        {
            //clear error message
            ErrorMessage = "";
            ErrorMessage = ValidateLogin(UserId, _password);
            if (String.IsNullOrEmpty(ErrorMessage))
                try
                {
                    //attempt student login
                    using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(dbContextResourceName)))
                    {
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                        string hashedPassword = Cryptography.Hash(_password);
                        Student student = UoW.StudentRepo.FindById(_userId.Value);
                        if (student != null)
                        {
                            //verify
                            if (Cryptography.Verify(_password, student.Password))
                            {
                                //clear error message
                                ErrorMessage = "";
                                //TODO: go to next window
                            }
                            else
                            {
                                ErrorMessage = "Invalid Login.";
                            }
                        }
                        else
                            ErrorMessage = "Invalid user ID.";

                        if (student == null)
                        {
                            // try lecturer
                        }

                        //attempt lecturer login
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
        }

        private string ValidateLogin(int? userId, string password)
        {
            string errorMsg = "";
            if (userId == null || userId == 0)
            {
                errorMsg = "ID Required";
            }
            if (String.IsNullOrEmpty(password))
            {
                errorMsg = "Password Required";
            }

            return errorMsg;
        }
    }
}