using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IRemoveCmd
    {
        RemoveCmd RemoveCmd { get; set; }

        bool Remove(object objToRemove);
    }
}