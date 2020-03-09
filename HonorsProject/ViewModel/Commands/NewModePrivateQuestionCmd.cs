using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class NewModePrivateQuestionCmd : ICommand
    {
        public IEnterNewModePrivateQuestionCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public NewModePrivateQuestionCmd(IEnterNewModePrivateQuestionCmd vm)
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
            VM.EnterNewModePrivateQuestion();
        }
    }
}