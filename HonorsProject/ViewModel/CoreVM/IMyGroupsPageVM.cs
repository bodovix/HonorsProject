using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.Commands.IComands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public interface IMyGroupsPageVM : ISaveVMFormCmd, IEnterNewModeCmd, IChangeSubgridCmd
    {
        Group SelectedGroup { get; set; }
        ObservableCollection<Group> Groups { get; set; }
        string GroupSearchTxt { get; set; }
        FormContext FormContext { get; set; }
        SubgridContext SubgridContext { get; set; }
    }
}