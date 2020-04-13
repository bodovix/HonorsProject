using HonorsProject.Model.Enums;
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
using HonorsProject.View.ExtensionMethods;
using HonorsProject.ViewModel;
using HonorsProject.Model.Data;
using HonorsProject.ViewModel.CoreVM;
using HonorsProject.Model.HelperClasses;
using HonorsProject.Model.Core;

namespace HonorsProject.View.Pages
{
    /// <summary>
    /// Interaction logic for DataAnalysisPage.xaml
    /// </summary>
    public partial class DataAnalysisPage : Page
    {
        public BaseViewModel VM;
        private BaseEntity entity;

        public DataAnalysisPage(BaseEntity entityToFocusOn)
        {
            entity = entityToFocusOn;
            this.SetMenuButtonColor(MenuButtonsSelection.DataAnalysisPage);
            InitializeComponent();
            VM = new DataAnalysisVM(entity, ConnectionConfigs.LiveConfig);
            Mediator.Register(MediatorChannels.GoToThisStudent.ToString(), GoToThisStudent);
            Mediator.Register(MediatorChannels.GoToThisSession.ToString(), GoToThisSession);
            Mediator.Register(MediatorChannels.GoToThisGroup.ToString(), GoToThisGroup);
            MainContainer.DataContext = VM;
        }

        private void GoToThisGroup(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GotoGroupPageWithGroup(entity);
        }

        private void GoToThisSession(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GoToSessionPageWithSession(entity);
        }

        private void GoToThisStudent(object obj)
        {
            Mediator.ClearMediator();

            BaseEntity entity = (BaseEntity)obj;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GoToStudentPageWithStudent(entity);
        }

        private void SearchGroupsResultsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void SearchSessionResultsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}