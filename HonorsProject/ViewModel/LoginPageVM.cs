using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.Commands;
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

        public LoginPageVM(LabAssistantContext labAssistantContext) : base(labAssistantContext)
        {
            LoginCmd = new LoginCmd(this);
        }

        internal void Login()
        {
            //TODO: DECIDE HOW TO STORE App info (static in app.cs or properties in MainWindowVM - MainWindowVM)
            //attempt student login
            using (UnitOfWork UoW = new UnitOfWork(_labAssistantContext))
            {
                Student student = UoW.StudentRepo.Login(_userId, _password);
                UoW.Complete();
                //Student login Successful
                if (student != null)
                {
                    // try lecturer
                }

                //attempt lecturer login
            }
        }
    }
}