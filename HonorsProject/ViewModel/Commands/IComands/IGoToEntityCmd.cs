﻿using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IGoToEntityCmd
    {
        GoToEntityCmd GoToEntityCmd { get; set; }
        bool GoToEntity(BaseEntity entity);
    }
}
