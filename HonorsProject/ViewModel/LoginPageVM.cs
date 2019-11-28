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

        private string _systemMessage;

        public string SystemMessage
        {
            get { return _systemMessage; }
            set
            {
                _systemMessage = value;
                OnPropertyChanged(nameof(SystemMessage));
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
        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        #endregion Properties

        public LoginPageVM(string dbContextName) : base(dbContextName)
        {
            LoginCmd = new LoginCmd(this);
        }

        internal void Login()
        {
            //clear error message
            SystemMessage = "";
            try
            {
                //attempt student login
                using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(dbContextResourceName)))
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    string hashedPassword = Cryptography.Hash(_password);
                    Student student = UoW.StudentRepo.FindById(_userId);
                    if (student != null)
                    {
                        //verify
                        if (Cryptography.Verify(_password, student.Password))
                        {
                            //success
                            SystemMessage = "Success";
                        }
                        else
                        {
                            SystemMessage = "Invalid Login. Please try again.";
                        }
                    }

                    //Student login Successful
                    if (student != null)
                    {
                        // try lecturer
                    }

                    //attempt lecturer login
                }
            }
            catch (Exception ex)
            {
                SystemMessage = ex.Message;
            }
        }
    }
}