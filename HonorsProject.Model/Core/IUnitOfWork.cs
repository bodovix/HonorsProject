﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public interface IUnitOfWork
    {
        ILecturerRepository LecturerRepo { get; }

        int Complete();
    }
}