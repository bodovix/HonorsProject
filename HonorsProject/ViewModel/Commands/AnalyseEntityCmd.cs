using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class AnalyseEntityCmd : ICommand
    {
        public IAnalyseEntityCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public AnalyseEntityCmd(IAnalyseEntityCmd vm)
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
            BaseEntity entity = (BaseEntity)parameter;
            VM.GoToAnalyseEntity(entity);
        }
    }
}