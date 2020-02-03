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
    public class GoToEntityCmd : ICommand
    {
        public IGoToEntityCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public GoToEntityCmd(IGoToEntityCmd vm)
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
            BaseEntity entity;
            if (parameter is FrequentAskersTuple)
                entity = ((FrequentAskersTuple)parameter).Student;
            else
                entity = (BaseEntity)parameter;
            VM.GoToEntity(entity);
        }
    }
}