using HonorsProject.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface ISaveVMFormCmd
    {
        SaveCmd SaveFormCmd { get; set; }

        bool Save();
    }
}