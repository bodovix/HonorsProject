using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.View.ExtensionMethods;
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
            this.SetMenuButtonColor(MenuButtonsSelection.MyAccountPage);
            InitializeComponent();
            this.DataContext = VM;
            Mediator.Register(MediatorChannels.ClearPropPassInput.ToString(), ClearProposedPassword);
        }

        private void ClearProposedPassword(object obj)
        {
            VM.ProposedPassword = "";
            ProposedPasswordTxt.Password = "";
            ProposedPasswordConfirmationTxt.Password = "";
        }

        private void CreateMySesoinVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
                VM = new MyAccountLecturerPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
            else
                VM = new MyAccountStudentPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Manually bind for password box
            if (this.DataContext != null)
            { ((BaseMyAccountPageVM)this.DataContext).ProposedPassword = ((PasswordBox)sender).Password; }
        }

        private void ProposedPasswordConfirmationTxt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((BaseMyAccountPageVM)this.DataContext).ProposedPasswordConf = ((PasswordBox)sender).Password; }
        }
    }
}