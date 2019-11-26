using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class LoginPageVM : BaseViewModel
    {
        #region Properties

        private MainWindowVM _parentVM;
        private SecureString _securePassword;

        public SecureString SecurePassword
        {
            get { return _securePassword; }
            set
            {
                _securePassword = value;
                OnPropertyChanged(nameof(SecurePassword));
            }
        }

        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(SecurePassword));
            }
        }

        #endregion Properties

        public LoginPageVM(MainWindowVM parentVM)
        {
            _parentVM = parentVM;
        }

        internal void Login()
        {
            //TODO: DECIDE HOW TO STORE app info (static in app.cs or properties in MainWindowVM - MainWindowVM)
            //attempt student login
            Student student = new Student();
            student.Login();
            //Student login Successful
            if (student != null)
            {
                // try lecturer
            }

            //attempt lecturer login
        }
    }
}