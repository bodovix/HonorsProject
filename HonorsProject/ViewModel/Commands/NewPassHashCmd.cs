using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class NewPassHashCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public INewPassHashCmd VM { get; set; }

        public NewPassHashCmd(INewPassHashCmd vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string password = parameter as string;
            VM.GenerateNewPasswordHash(password);
        }
    }
}