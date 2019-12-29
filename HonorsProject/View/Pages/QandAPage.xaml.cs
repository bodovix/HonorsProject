using HonorsProject.Model.Enums;
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
    /// Interaction logic for QandAPage.xaml
    /// </summary>
    public partial class QandAPage : Page
    {

        private BaseQandAPageVM VM;
        public QandAPage()
        {
            PickVM();
            InitializeComponent();
            MainContainer.DataContext = VM;
        }

        private void PickVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
            {

            }
            else
            {

            }
        }
    }
}
