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
    public class CSVCmd : ICommand
    {
        public ICSVCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public CSVCmd(ICSVCmd vm)
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
            CSVAction action = (CSVAction)parameter;
            VM.ExecuteCSV(action);
        }
    }
}