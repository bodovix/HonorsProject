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
        public MainWindowVM VM { get; set; }

        public MainWindow()
        {
            //Initialize window with View Model
            VM = new MainWindowVM();
            InitializeComponent();
            ContainerDockPannel.DataContext = VM;
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