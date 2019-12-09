using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class MainWindowVM : BaseViewModel
    {
        private Role _userRole;

        public Role UserRole
        {
            get { return _userRole; }
            set { _userRole = value; }
        }

        public MainWindowVM(string dbContextName) : base(dbContextName)
        {
        }
    }
}