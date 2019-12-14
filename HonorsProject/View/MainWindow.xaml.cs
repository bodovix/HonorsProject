using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
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
        //TODO: implement these into main window navigation - might cause problems with references /garbage collector.
        //TODO: take time and test properly
        public MainWindowVM VM { get; set; }

        private StudentsPage studentsPage = new StudentsPage();
        private GroupPage groupPage = new GroupPage();
        private MySessionsPage sessionsPage = new MySessionsPage();
        private QandAPage questionsPage = new QandAPage();
        private QandAPage answerPage = new QandAPage();
        private DataAnalysisPage dataAnalysisPage = new DataAnalysisPage();
        private MyAccountPage accountPage = new MyAccountPage();

        public MainWindow()
        {
            //Initialize window with View Model
            VM = new MainWindowVM(ConnectionConfigs.LiveConfig);
            InitializeComponent();
            ContainerDockPannel.DataContext = VM;
            InitiallyHideNavigation();

            Mediator.Register(MediatorChannels.LoginAsUserX.ToString(), LoggInAsX);
        }

        private void InitiallyHideNavigation()
        {
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
                    StudentsBtn.Visibility = Visibility.Collapsed;
                    GroupsBtn.Visibility = Visibility.Visible;
                    MySessionsBtn.Visibility = Visibility.Visible;
                    MyQuestoins.Visibility = Visibility.Visible;
                    MyAnswers.Visibility = Visibility.Visible;
                    DataAnalysisBtn.Visibility = Visibility.Visible;
                    MyAccountBtn.Visibility = Visibility.Visible;
                    break;

                case Role.Lecturer:
                    StudentsBtn.Visibility = Visibility.Visible;
                    GroupsBtn.Visibility = Visibility.Visible;
                    MySessionsBtn.Visibility = Visibility.Visible;
                    MyQuestoins.Visibility = Visibility.Visible;
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
            MainContent.Content = new MySessionsPage();
        }

        private void StudentsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StudentsPage();
        }

        private void GroupsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new GroupPage();
        }

        private void MySessionsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new MySessionsPage();
        }

        private void MyQuestoins_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QandAPage();
        }

        private void MyAnswers_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QandAPage();
        }

        private void DataAnalysisBtn_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DataAnalysisPage();
        }

        private void MyAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new MyAccountPage();
        }
    }
}