using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IPostCmd
    {
        PostCmd PostCmd { get; set; }

        bool Post();
    }
}