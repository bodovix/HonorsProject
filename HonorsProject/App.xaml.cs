using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HonorsProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IUnitOfWork UnitOfWork;

        public App()
        {
            UnitOfWork = new UnitOfWork(new LabAssistantContext("LabAssistantContext"));
        }
    }
}