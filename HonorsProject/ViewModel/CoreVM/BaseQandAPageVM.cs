using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseQandAPageVM : BaseViewModel
    {
        public BaseQandAPageVM(string dbcontextName) : base(dbcontextName)
        {
        }

    }
}
