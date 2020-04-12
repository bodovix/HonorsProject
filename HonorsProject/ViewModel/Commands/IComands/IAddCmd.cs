using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IAddCmd
    {
        AddCmd AddCmd { get; set; }

        bool Add(object objToAdd);
    }
}