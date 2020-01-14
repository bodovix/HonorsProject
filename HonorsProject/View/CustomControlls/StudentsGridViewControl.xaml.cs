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
    public partial class StudentsGridViewControl : UserControl
    {
        public RemoveEntityCmd RemoveEntityCmd
        {
            get { return (RemoveEntityCmd)GetValue(StudentToRemoveProperty); }
            set { SetValue(StudentToRemoveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentToRemoveProperty =
            DependencyProperty.Register(nameof(RemoveEntityCmd), typeof(RemoveEntityCmd), typeof(StudentsGridViewControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public GoToEntityCmd GoToEntityCmd
        {
            get { return (GoToEntityCmd)GetValue(GoToEntityCmdProperty); }
            set { SetValue(GoToEntityCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToEntityCmdProperty =
            DependencyProperty.Register(nameof(GoToEntityCmd), typeof(GoToEntityCmd), typeof(StudentsGridViewControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Student SelectedStudent
        {
            get { return (Student)GetValue(SelectedStudentProperty); }
            set { SetValue(SelectedStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for groupSele.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStudentProperty =
            DependencyProperty.Register(nameof(SelectedStudent), typeof(Student), typeof(StudentsGridViewControl), new FrameworkPropertyMetadata(new Student(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable<Student> Students
        {
            get { return (IEnumerable<Student>)GetValue(StudentsProperty); }
            set { SetValue(StudentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Groups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentsProperty =
            DependencyProperty.Register(nameof(Students), typeof(IEnumerable<Student>), typeof(StudentsGridViewControl));

        public StudentsGridViewControl()
        {
            InitializeComponent();
        }
    }
}