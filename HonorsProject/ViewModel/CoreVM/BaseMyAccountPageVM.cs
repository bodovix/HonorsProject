using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.Commands;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseMyAccountPageVM : BaseViewModel, ISaveVMFormCmd, ICancelmd
    {
        #region Properties

        public abstract ISystemUser User { get; set; }

        #endregion Properties

        #region Commands

        public SaveCmd SaveFormCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }

        #endregion Commands

        protected BaseMyAccountPageVM(string dbcontextName) : base(dbcontextName)
        {
            SaveFormCmd = new SaveCmd(this);
            CancelCmd = new CancelCmd(this);
        }

        public abstract bool Cancel();

        public abstract bool Save();
    }
}