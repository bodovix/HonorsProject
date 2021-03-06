﻿using System;
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
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.CoreVM;
using HonorsProject.ViewModel;
using HonorsProject.Model.Data;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.HelperClasses;
using HonorsProject.View.ExtensionMethods;

namespace HonorsProject.View.Pages
{
    /// <summary>
    /// Interaction logic for MySessionsPage.xaml
    /// </summary>
    public partial class MySessionsPage : Page
    {
        //Polymorphic VM for Students or Lecturers
        private BaseMySessionsPageVM VM;

        public MySessionsPage(Session selectedSession)
        {
            CreateMySesoinVM();
            this.SetMenuButtonColor(MenuButtonsSelection.MySessionPage);
            InitializeComponent();
            MainContainer.DataContext = VM;
            try
            {
                if (selectedSession != null)
                    VM.SelectedSession = VM.UnitOfWork.SessionRepository.Get(selectedSession.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Mediator.Register(MediatorChannels.DeleteSessionConfirmation.ToString(), ShowDeleteConfMessage);
            Mediator.Register(MediatorChannels.GoToThisSession.ToString(), GoToSession);
            Mediator.Register(MediatorChannels.GoToAnalyseEntity.ToString(), GoToAnalyseEntity);
        }

        private void GoToAnalyseEntity(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GoToAnalyseEntity(entity);
        }

        private void GoToSession(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GoToQandAWithEntity(entity);
        }

        private void ShowDeleteConfMessage(object obj)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Deleting Sessions cannot be undone. \nAll questions and answers for this session will also be deleted.", "Are you sure?", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
                VM.IsConfirmationAccepted = true;
            else
                VM.IsConfirmationAccepted = false;
        }

        private void CreateMySesoinVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
                VM = new MySessionsLecturerPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
            else
                VM = new MySessionsStudentPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
        }
    }
}