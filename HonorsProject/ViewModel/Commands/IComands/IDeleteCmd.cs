using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IDeleteCmd
    {
        DeleteCmd DeleteCmd { get; set; }

        bool Delete(BaseEntity objToDelete);
    }
}