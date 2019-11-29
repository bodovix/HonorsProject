using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsLecturerPageVM : BaseViewModel, IMySessionsPageVM
    {
        public MySessionsLecturerPageVM(string dbcontextName) : base(dbcontextName)
        {
        }
    }
}