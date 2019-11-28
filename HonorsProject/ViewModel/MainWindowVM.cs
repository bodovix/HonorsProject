using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
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
        public MainWindowVM(string dbContextName) : base(dbContextName)
        {
        }
    }
}