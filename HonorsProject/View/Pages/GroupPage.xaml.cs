using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel;
using HonorsProject.ViewModel.CoreVM;
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

namespace HonorsProject.View.Pages
{
    /// <summary>
    /// Interaction logic for GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        private IMyGroupsPageVM VM;
        private Group _selectedGroup;

        public GroupPage(Group selectedGroup)
        {
            _selectedGroup = selectedGroup;
            CreateMyGroupsVM();
            InitializeComponent();
            Mediator.Register(MediatorChannels.DeleteGroupConfirmation.ToString(), DeleteGroupConfirmation);
            Mediator.Register(MediatorChannels.DeleteSessionConfirmation.ToString(), DeleteSessionConfirmation);
            Mediator.Register(MediatorChannels.LoadActiveSessionsSubgrid.ToString(), LoadActiveSessionsSubgrid);
            Mediator.Register(MediatorChannels.LoadPreviousSessionsSubgrid.ToString(), LoadPreviousSessionsSubgrid);
            Mediator.Register(MediatorChannels.LoadFutureSessionsSubgrid.ToString(), LoadFutureSessionsSubgrid);
            Mediator.Register(MediatorChannels.LoadStudentsSubgrid.ToString(), LoadStudentsSubgrid);
            Mediator.Register(MediatorChannels.GoToThisSession.ToString(), GoToThisSession);
            Mediator.Register(MediatorChannels.GoToThisStudent.ToString(), GoToThisStudent);

            DataContext = VM;
        }

        private void GoToThisStudent(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GoToStudentPageWithStudent(entity);
        }

        private void GoToThisSession(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GoToQandAWithEntity(entity);
        }

        private void LoadStudentsSubgrid(object obj)
        {
            StudentsGridViewControl.Visibility = Visibility.Visible;
            SessionsGridViewControl.Visibility = Visibility.Collapsed;
        }

        private void LoadFutureSessionsSubgrid(object obj)
        {
            StudentsGridViewControl.Visibility = Visibility.Collapsed;
            SessionsGridViewControl.Visibility = Visibility.Visible;
        }

        private void LoadPreviousSessionsSubgrid(object obj)
        {
            StudentsGridViewControl.Visibility = Visibility.Collapsed;
            SessionsGridViewControl.Visibility = Visibility.Visible;
        }

        private void LoadActiveSessionsSubgrid(object obj)
        {
            StudentsGridViewControl.Visibility = Visibility.Collapsed;
            SessionsGridViewControl.Visibility = Visibility.Visible;
        }

        private void DeleteGroupConfirmation(object obj)
        {
            Group g = obj as Group;
            MessageBoxResult dialogResult = MessageBox.Show($"Delete group: {g.Name}? \nThis action cannot be undone", "Are you sure?", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
                VM.IsConfirmed = true;
            else
                VM.IsConfirmed = false;
        }

        private void DeleteSessionConfirmation(object obj)
        {
            Session s = obj as Session;
            MessageBoxResult dialogResult = MessageBox.Show($"Delete session: {s.Name}? \nThis action cannot be undone", "Are you sure?", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
                VM.IsConfirmed = true;
            else
                VM.IsConfirmed = false;
        }

        private void CreateMyGroupsVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
                VM = new MyGroupsLecturerPageVM(App.AppUser, _selectedGroup, ConnectionConfigs.LiveConfig);
            else
                VM = new MyGroupsStudentPageVM(App.AppUser, _selectedGroup, ConnectionConfigs.LiveConfig);
        }
    }
}