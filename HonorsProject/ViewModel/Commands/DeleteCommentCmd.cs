﻿using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HonorsProject.ViewModel.Commands
{
    public class DeleteCommentCmd : ICommand
    {
        public ICommentCmd VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public DeleteCommentCmd(ICommentCmd vm)
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
            Comment commentToDelete = parameter as Comment;
            VM.DeleteComent(commentToDelete);
        }
    }
}