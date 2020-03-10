using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.View.ExtensionMethods;
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
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class LecturerPage : Page
    {
        public LecturerPageVM VM { get; set; }

        public LecturerPage()
        {
            VM = new LecturerPageVM(ConnectionConfigs.LiveConfig, App.AppUser);
            this.SetMenuButtonColor(MenuButtonsSelection.LecturersPage);
            InitializeComponent();
            Mediator.Register(MediatorChannels.LecturerPageGeneratePasswordCheck.ToString(), ShowPasswordConfBox);
            Mediator.Register(MediatorChannels.LecturerPageNewPasswordDisplay.ToString(), ShowPasswordDisplay);
            Mediator.Register(MediatorChannels.DeleteLecturerConfirmation.ToString(), DeleteLecturerConfirmation);

            DataContext = VM;
        }

        private void ShowPasswordDisplay(object obj)
        {
            string pass = obj as string;
            MessageBox.Show(pass + "\n Keep this password safe.");
        }

        private void ShowPasswordConfBox(object obj)
        {
            VM.IsConfirmed = false;
            MessageBoxResult dialogResult = MessageBox.Show("Generate new password for user? \nThis action must be saved with rest of form", "Are you sure?", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
                VM.IsConfirmed = true;
            else
                VM.IsConfirmed = false;
        }

        private void DeleteLecturerConfirmation(object obj)
        {
            VM.IsConfirmed = false;
            Lecturer l = obj as Lecturer;
            MessageBoxResult dialogResult = MessageBox.Show($"Delete lecturer {l.Name}? \nThis action cannot be undone", "Are you sure?", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
                VM.IsConfirmed = true;
            else
                VM.IsConfirmed = false;
        }
    }
}