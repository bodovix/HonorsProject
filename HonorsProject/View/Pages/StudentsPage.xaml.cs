using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
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
    public partial class StudentsPage : Page
    {
        public StudentsPageVM VM { get; set; }

        public StudentsPage()
        {
            VM = new StudentsPageVM(ConnectionConfigs.LiveConfig, App.AppUser);
            InitializeComponent();
            Mediator.Register(MediatorChannels.StudentsPageGeneratePasswordCheck.ToString(), ShowPasswordConfBox);
            Mediator.Register(MediatorChannels.StudentsPageNewPasswordDisplay.ToString(), ShowPasswordDisplay);
            Mediator.Register(MediatorChannels.LoadGroupsSubgridForStudents.ToString(), ShowStudentsGroups);
            Mediator.Register(MediatorChannels.LoadQuestionsSubgridForStudents.ToString(), ShowStudentsQuestions);
            Mediator.Register(MediatorChannels.DeleteStudentConfirmation.ToString(), DeleteStudentConfrimation);
            DataContext = VM;
        }

        private void DeleteStudentConfrimation(object obj)
        {
            VM.IsConfirmed = false;
            Student s = obj as Student;
            MessageBoxResult dialogResult = MessageBox.Show($"Delete student {s.Name}? \nThis action cannot be undone", "Are you sure?", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
                VM.IsConfirmed = true;
            else
                VM.IsConfirmed = false;
        }

        private void ShowStudentsQuestions(object obj)
        {
            QuestionsGV.Visibility = Visibility.Visible;
            GroupsGV.Visibility = Visibility.Collapsed;
        }

        private void ShowStudentsGroups(object obj)
        {
            GroupsGV.Visibility = Visibility.Visible;
            QuestionsGV.Visibility = Visibility.Collapsed;
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
    }
}