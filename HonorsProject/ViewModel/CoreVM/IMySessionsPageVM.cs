using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public interface IMySessionsPageVM
    {
        BaseSystemUser User { get; set; }
        Session SelectedSession { get; set; }
        ObservableCollection<Session> MySessions { get; set; }
    }
}