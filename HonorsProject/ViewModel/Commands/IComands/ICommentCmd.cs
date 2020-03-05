using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface ICommentCmd
    {
        PostCmd PostCmd { get; set; }

        bool Post();

        DeleteCommentCmd DeleteCommentCmd { get; set; }

        bool DeleteComent(Comment commentToDelete);
    }
}