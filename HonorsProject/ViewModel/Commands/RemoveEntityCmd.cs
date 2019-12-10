using HonorsProject.Model.Core;
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
    public class RemoveEntityCmd : ICommand

    {
        public IRemoveEntityCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public RemoveEntityCmd(IRemoveEntityCmd vm)
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
            BaseEntity entity = (BaseEntity)parameter;
            VM.Remove(entity);
        }
    }
}