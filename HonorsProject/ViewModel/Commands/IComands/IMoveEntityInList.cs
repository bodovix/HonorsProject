using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IMoveEntityInList
    {
        MoveEntityOutOfListCmd MoveEntityOutOfListCmd { get; set; }

        bool MoveEntityOutOfList(BaseEntity entityToRemove);

        MoveEntityOutOfListCmd MoveEntityInToListCmd { get; set; }

        bool MoveEntityInToList(BaseEntity entityToAdd);
    }
}