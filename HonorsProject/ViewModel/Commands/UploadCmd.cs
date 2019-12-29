using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class UploadCmd : ICommand
    {
        public IUploadCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public UploadCmd(IUploadCmd vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            //no checks needed
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Upload(parameter);
        }
    }
}