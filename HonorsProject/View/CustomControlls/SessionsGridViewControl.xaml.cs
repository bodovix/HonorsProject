using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
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
    public partial class SessionsGridViewControl : UserControl
    {
        public RemoveEntityCmd RemoveEntityCmd
        {
            get { return (RemoveEntityCmd)GetValue(RemoveEntityCmdProperty); }
            set { SetValue(RemoveEntityCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveEntityCmdProperty =
            DependencyProperty.Register(nameof(RemoveEntityCmd), typeof(RemoveEntityCmd), typeof(SessionsGridViewControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Role UserRole
        {
            get { return (Role)GetValue(UserRoleProperty); }
            set { SetValue(UserRoleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserRoleProperty =
            DependencyProperty.Register("MyProperty", typeof(Role), typeof(SessionsGridViewControl), new PropertyMetadata(0));


        public Student SelectedSession
        {
            get { return (Student)GetValue(SelectedSessionProperty); }
            set { SetValue(SelectedSessionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for groupSele.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSessionProperty =
            DependencyProperty.Register(nameof(SelectedSession), typeof(Session), typeof(SessionsGridViewControl), new FrameworkPropertyMetadata(new Session(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable<Session> Sessions
        {
            get { return (IEnumerable<Session>)GetValue(SessionsProperty); }
            set { SetValue(SessionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Groups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SessionsProperty =
            DependencyProperty.Register(nameof(Sessions), typeof(IEnumerable<Session>), typeof(SessionsGridViewControl));

        public SessionsGridViewControl()
        {
            InitializeComponent();
        }
    }
}