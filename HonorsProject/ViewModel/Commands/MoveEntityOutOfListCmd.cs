using HonorsProject.Model.Core;
using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class MoveEntityOutOfListCmd : ICommand
    {
        public IMoveEntityInList VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public MoveEntityOutOfListCmd(IMoveEntityInList vm)
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
            BaseEntity entityToRemove = (BaseEntity)parameter;
            VM.MoveEntityOutOfList(entityToRemove);
        }
    }
}