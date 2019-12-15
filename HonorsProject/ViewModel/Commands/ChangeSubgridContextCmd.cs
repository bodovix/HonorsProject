using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class ChangeSubgridContextCmd : ICommand
    {
        public IChangeSubgridCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public ChangeSubgridContextCmd(IChangeSubgridCmd vm)
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
            SubgridContext context = (SubgridContext)parameter;
            VM.ChangeSubgridContext(context);
        }
    }
}