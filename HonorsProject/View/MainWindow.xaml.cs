﻿using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.View.Pages;
using HonorsProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HonorsProject.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool HasLoggedInOnce { get; set; }
        public MainWindowVM VM { get; set; }

        public MainWindow()
        {
            //Initialize window with View Model
            VM = new MainWindowVM(ConnectionConfigs.LiveConfig);
            InitializeComponent();
            ContainerDockPannel.DataContext = VM;
            InitiallyHideNavigation();
            if (App.AppUser == null)
                MakeWindowSmall();
            else if (App.AppUser.Id == 0)
                MakeWindowSmall();
            else
                MakeWindowBig();

            Mediator.Register(MediatorChannels.LoginAsUserX.ToString(), LoggInAsX);
        }

        private void MakeWindowBig()
        {
            WindowState = WindowState.Maximized;
        }

        private void MakeWindowSmall()
        {
            Height = 300;
            Width = 300;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void InitiallyHideNavigation()
        {
            LecturersBtn.Visibility = Visibility.Collapsed;
            StudentsBtn.Visibility = Visibility.Collapsed;
            GroupsBtn.Visibility = Visibility.Collapsed;
            MySessionsBtn.Visibility = Visibility.Collapsed;
            MyQuestoins.Visibility = Visibility.Collapsed;
            MyAnswers.Visibility = Visibility.Collapsed;
            DataAnalysisBtn.Visibility = Visibility.Collapsed;
            MyAccountBtn.Visibility = Visibility.Collapsed;
        }

        private void ShowAppropriateNavigation()
        {
            switch (App.LoggedInAs)
            {
                case Role.Student:
                    LecturersBtn.Visibility = Visibility.Collapsed;
                    StudentsBtn.Visibility = Visibility.Collapsed;
                    GroupsBtn.Visibility = Visibility.Visible;
                    MySessionsBtn.Visibility = Visibility.Visible;
                    MyQuestoins.Visibility = Visibility.Visible;
                    MyAnswers.Visibility = Visibility.Collapsed;
                    DataAnalysisBtn.Visibility = Visibility.Collapsed;
                    MyAccountBtn.Visibility = Visibility.Visible;
                    break;

                case Role.Lecturer:
                    LecturersBtn.Visibility = Visibility.Visible;
                    StudentsBtn.Visibility = Visibility.Visible;
                    GroupsBtn.Visibility = Visibility.Visible;
                    MySessionsBtn.Visibility = Visibility.Visible;
                    MyQuestoins.Visibility = Visibility.Collapsed;
                    MyAnswers.Visibility = Visibility.Visible;
                    DataAnalysisBtn.Visibility = Visibility.Visible;
                    MyAccountBtn.Visibility = Visibility.Visible;
                    break;

                default:
                    throw new Exception("log in as Role not accounted for");
            }
        }

        private void LoggInAsX(object obj)
        {
            Role userRole = (Role)obj;
            App.LoggedInAs = userRole;
            ShowAppropriateNavigation();
            if (!HasLoggedInOnce)
            {
                MakeWindowBig();
                HasLoggedInOnce = true;
            }
            MainContent.Content = new MySessionsPage(null);
        }

        internal void GoToSessionPageWithSession(BaseEntity session)
        {
            Mediator.ClearMediator();
            MainContent.Content = new MySessionsPage((Session)session);
        }

        internal void GoToStudentPageWithStudent(BaseEntity student)
        {
            Mediator.ClearMediator();
            MainContent.Content = new StudentsPage((Student)student);
        }

        internal void GotoGroupPageWithGroup(BaseEntity group)
        {
            Mediator.ClearMediator();
            MainContent.Content = new GroupPage((Group)group);
        }

        public void GoToQandAWithEntity(BaseEntity entityTofocusOn)
        {
            Mediator.ClearMediator();
            MainContent.Content = new QandAPage(entityTofocusOn);
        }

        internal void GoToAnalyseEntity(BaseEntity entity)
        {
            Mediator.ClearMediator();
            MainContent.Content = new DataAnalysisPage(entity);
        }

        private void StudentsBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new StudentsPage(new Student());
        }

        private void LecturersBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new LecturerPage();
        }

        private void GroupsBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new GroupPage(new Group());
        }

        private void MySessionsBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new MySessionsPage(null);
        }

        private void MyQuestoins_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new QandAPage(new Question());
        }

        private void MyAnswers_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new QandAPage(new Answer());
        }

        private void DataAnalysisBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new DataAnalysisPage(null);
        }

        private void MyAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            MainContent.Content = new MyAccountPage();
        }

        private void ExitAppBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            return;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Mediator.ClearMediator();
            InitiallyHideNavigation();
            App.AppUser = null;
            Mediator.Register(MediatorChannels.LoginAsUserX.ToString(), LoggInAsX);

            MainContent.Content = new LoginPage();
        }
    }
}