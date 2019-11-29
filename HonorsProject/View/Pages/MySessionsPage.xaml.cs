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
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.CoreVM;
using HonorsProject.ViewModel;
using HonorsProject.Model.Data;

namespace HonorsProject.View.Pages
{
    /// <summary>
    /// Interaction logic for MySessionsPage.xaml
    /// </summary>
    public partial class MySessionsPage : Page
    {
        private IMySessionsPageVM VM;

        public MySessionsPage()
        {
            CreateMySesoinVM();
            InitializeComponent();
        }

        private void CreateMySesoinVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
                VM = new MySessionsLecturerPageVM(ConnectionConfigs.LiveConfig);
            else
                VM = new MySessionsStudentPageVM(ConnectionConfigs.LiveConfig);
        }
    }
}