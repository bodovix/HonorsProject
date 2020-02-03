using HonorsProject.Model.Data;
using HonorsProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        public LoginPage()
        {
            //Initialize window with View Model
            VM = new LoginPageVM(ConnectionConfigs.LiveConfig);
            InitializeComponent();
            MainContainer.DataContext = VM;
            this.DataContext = VM;
        }

        //manually binding secure string to VM
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginPageVM)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void LoginEnter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                VM.Login(ref App.AppUser);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
            {
                if (PassBox.ToolTip == null)
                {
                    ToolTip tt = new ToolTip();
                    tt.Content = "Warning: CapsLock is on";
                    tt.PlacementTarget = sender as UIElement;
                    tt.Placement = PlacementMode.Bottom;
                    PassBox.ToolTip = tt;
                    tt.IsOpen = true;
                }
            }
            else
            {
                var currentToolTip = PassBox.ToolTip as ToolTip;
                if (currentToolTip != null)
                {
                    currentToolTip.IsOpen = false;
                }

                PassBox.ToolTip = null;
            }
        }
    }
}