using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class LoginCmd : ICommand
    {
        public LoginPageVM VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public LoginCmd(LoginPageVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            VM.Login();
        }
    }
}