using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class RemoveLecturerCmd : ICommand
    {
        public IRemoveLecturerCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public RemoveLecturerCmd(IRemoveLecturerCmd vm)
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
            Lecturer l = (Lecturer)parameter;
            VM.RemoveLecturer(l);
        }
    }
}