using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
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
    /// Interaction logic for MyAccountPage.xaml
    /// </summary>
    public partial class MyAccountPage : Page
    {
        public BaseMyAccountPageVM VM { get; set; }

        public MyAccountPage()
        {
            CreateMySesoinVM();
            InitializeComponent();
            MainContainer.DataContext = VM;
        }

        private void CreateMySesoinVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
                VM = new MyAccountLecturerPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
            else
                VM = new MyAccountStudentPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
        }
    }
}