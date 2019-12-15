using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IChangeSubgridCmd
    {
        ChangeSubgridContextCmd GetChangeSubgridContextCmd { get; set; };

        bool ChangeSubgridContext(SubgridContext context);
    }
}