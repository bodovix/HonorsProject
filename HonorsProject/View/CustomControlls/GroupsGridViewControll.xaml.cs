using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.Commands;
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
        public RemoveEntityCmd RemoveEntityCmd
        {
            get { return (RemoveEntityCmd)GetValue(GroupToRemoveProperty); }
            set { SetValue(GroupToRemoveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupToRemoveProperty =
            DependencyProperty.Register(nameof(RemoveEntityCmd), typeof(RemoveEntityCmd), typeof(GroupsGridViewControll), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public GoToEntityCmd GoToEntityCmd
        {
            get { return (GoToEntityCmd)GetValue(GoToEntityCmdProperty); }
            set { SetValue(GoToEntityCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToEntityCmdProperty =
            DependencyProperty.Register(nameof(GoToEntityCmd), typeof(GoToEntityCmd), typeof(GroupsGridViewControll), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Group SelectedGroup
        {
            get { return (Group)GetValue(groupSeleProperty); }
            set { SetValue(groupSeleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for groupSele.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty groupSeleProperty =
            DependencyProperty.Register(nameof(SelectedGroup), typeof(Group), typeof(GroupsGridViewControll), new FrameworkPropertyMetadata(new Group(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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