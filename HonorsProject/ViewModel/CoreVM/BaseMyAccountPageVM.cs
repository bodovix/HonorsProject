using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.Commands;
using HonorsProject.Model.HelperClasses;
using HonorsProject.Model.Enums;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseMyAccountPageVM : BaseViewModel, ISaveVMFormCmd, ICancelmd, INewPassHashCmd
    {
        #region Properties

        public abstract ISystemUser User { get; set; }
        private string _proposedPassword;

        public string ProposedPassword
        {
            get { return _proposedPassword; }
            set
            {
                _proposedPassword = value;
                OnPropertyChanged(nameof(ProposedPassword));
            }
        }

        #endregion Properties

        #region Commands

        public SaveCmd SaveFormCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }
        public NewPassHashCmd NewPassHashCmd { get; set; }

        #endregion Commands

        protected BaseMyAccountPageVM(string dbcontextName) : base(dbcontextName)
        {
            ProposedPassword = "";
            SaveFormCmd = new SaveCmd(this);
            NewPassHashCmd = new NewPassHashCmd(this);
            CancelCmd = new CancelCmd(this);
        }

        public bool Cancel()
        {
            try
            {
                UnitOfWork.Reload((BaseEntity)User);
                OnPropertyChanged(nameof(User));
            }
            catch
            {
                FeedbackMessage = "Unable to refresh user account. " +
                                    "\n Navigate away then come back. " +
                                    "\n If this does not solve your issue please contact support.";
                return false;
            }
            return true;
        }

        public bool GenerateNewPasswordHash(string optionalNewPassword)
        {
            bool result;
            try
            {
                result = User.GenerateNewPasswordHash(ref optionalNewPassword);
                if (result)
                {
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    Mediator.NotifyColleagues(MediatorChannels.ClearPropPassInput.ToString(), null);
                    if (result)
                    {
                        OnPropertyChanged(nameof(User));
                        result = true;
                    }
                    else
                        FeedbackMessage = "Failed to save changes to database. Please try again or contact support.";
                }
                else
                    FeedbackMessage = "Failed to generate new hash for password. Please try again or contact support.";
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                result = false;
            }
            return result;
        }

        public abstract bool Save();
    }
}