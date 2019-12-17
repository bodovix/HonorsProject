using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public interface IMyGroupsPageVM
    {
        Group SelectedGroup { get; set; }
        ObservableCollection<Group> Groups { get; set; }
        string GroupSearchTxt { get; set; }
    }
}