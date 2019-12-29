using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class UploadImageCmd : ICommand
    {
        public IUploadImageCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public UploadImageCmd(IUploadImageCmd vm)
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
            Image image = (Image)parameter;
            VM.UploadImage(image);
        }
    }
}