using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    internal class SaveCmd : ICommand
    {
        public ISaveVMForm VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public SaveCmd(ISaveVMForm vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            //no pre checks needed
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Save();
        }
    }
}