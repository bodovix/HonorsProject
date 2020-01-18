using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface ICSVCmd
    {
        CSVCmd CSVCmd { get; set; }

        bool ExecuteCSV(CSVAction action);
    }
}