using HonorsProject.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public interface IEnterNewModeCmd
    {
        NewModeCmd NewModeCmd { get; set; }

        void EnterNewMode();
    }
}