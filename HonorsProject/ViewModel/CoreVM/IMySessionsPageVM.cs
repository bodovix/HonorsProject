﻿using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public interface IMySessionsPageVM : ISaveVMFormCmd, IEnterNewModeCmd
    {
        FormContext FormContext { get; set; }
        string FormContextTxt { get; set; }
        Role UserRole { get; set; }
        ISystemUser User { get; set; }
        Session SelectedSession { get; set; }
        ObservableCollection<Session> MySessions { get; set; }
        ObservableCollection<Group> Groups { get; set; }
    }
}