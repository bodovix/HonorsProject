using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HonorsProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Role LoggedInAs;
        public static ISystemUser AppUser;
        private Thread poolingThread;

        public App()
        {
            poolingThread = new Thread(SignalPoolingUpdate);
            poolingThread.IsBackground = true;
            poolingThread.Start();
        }

        private void SignalPoolingUpdate()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(5000);
                Mediator.NotifyColleagues(MediatorChannels.PoolingUpdate.ToString(), null);
                Console.WriteLine($"pooling update signaled {DateTime.Now.TimeOfDay}");
            }
        }
    }
}