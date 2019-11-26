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

namespace HonorsProject.View.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPageVM VM { get; set; }

        public LoginPage(MainWindowVM mainWindowVM)
        {
            //Initialize window with View Model
            VM = new LoginPageVM(mainWindowVM);
            InitializeComponent();
            MainContainer.DataContext = VM;
            this.DataContext = VM;
        }

        //manually binding secure string to VM
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginPageVM)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        }
    }
}