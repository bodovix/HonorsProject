using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsStudentPageVM : BaseViewModel, IMySessionsPageVM
    {
        public MySessionsStudentPageVM(string dbcontextName) : base(dbcontextName)
        {
        }
    }
}