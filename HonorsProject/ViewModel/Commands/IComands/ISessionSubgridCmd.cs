using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface ISessionSubgridCmd
    {
        GetActiveSessionsCmd GetActiveSessionsCmd { get; set; }
        GetFutureSessionsCmd GetFutureSessionsCmd { get; set; }
        GetPreviousSessionsCmd GetPreviousSessionsCmd { get; set; }

        bool GetAllMyCurrentSessions();

        bool GetAllMyPreviousSessions();

        bool GetAllMyFutureSessions();
    }
}