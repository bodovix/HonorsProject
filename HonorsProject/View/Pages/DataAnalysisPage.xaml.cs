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

namespace HonorsProject.View.Pages
{
    /// <summary>
    /// Interaction logic for DataAnalysisPage.xaml
    /// </summary>
    public partial class DataAnalysisPage : Page
    {
        public DataAnalysisPage()
        {
            this.SetMenuButtonColor(MenuButtonsSelection.DataAnalysisPage);
            InitializeComponent();
        }
    }
}