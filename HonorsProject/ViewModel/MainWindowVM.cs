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
        public Role LoggedInAs { get; set; }

        public MainWindowVM()
        {
        }
    }
}