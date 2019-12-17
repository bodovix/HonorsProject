using HonorsProject.Model.Core;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    internal class MyGroupsStudentPageVM : BaseViewModel, IMyGroupsPageVM
    {
        public MyGroupsStudentPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
        }
    }
}