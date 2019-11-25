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

        private MainWindowVM parentVM;
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

        public LoginPageVM()
        {
        }

        internal void Login()
        {
            throw new NotImplementedException();
            //TODO: DECIDE HOW TO STORE app info (static in app.cs or properties in MainWindowVM - MainWindowVM)
            //attempt student login

            //attempt lecturer login
        }
    }
}