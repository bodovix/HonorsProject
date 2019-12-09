using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HonorsProject.View.CustomControlls
{
    /// <summary>
    /// Interaction logic for GroupsGridViewControll.xaml
    /// </summary>
    public partial class GroupsGridViewControll : UserControl
    {
        public ObservableCollection<Group> Groups
        {
            get { return (ObservableCollection<Group>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Groups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register(nameof(Groups), typeof(ObservableCollection<Group>), typeof(GroupsGridViewControll), new PropertyMetadata(new ObservableCollection<Group>(), SetItemsSource));

        private static void SetItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GroupsGridViewControll controll = d as GroupsGridViewControll;
            if (controll != null)
            {
                controll.GroupDataGrid.ItemsSource = e.NewValue as ObservableCollection<Group>;
            }
        }

        public GroupsGridViewControll()
        {
            InitializeComponent();
        }
    }
}