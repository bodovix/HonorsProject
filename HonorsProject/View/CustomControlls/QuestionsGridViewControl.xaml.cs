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
    /// Interaction logic for QuestionsGridViewControl.xaml
    /// </summary>
    public partial class QuestionsGridViewControl : UserControl
    {
        public RemoveEntityCmd RemoveEntityCmd
        {
            get { return (RemoveEntityCmd)GetValue(EntityToRemoveProperty); }
            set { SetValue(EntityToRemoveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EntityToRemoveProperty =
            DependencyProperty.Register(nameof(RemoveEntityCmd), typeof(RemoveEntityCmd), typeof(QuestionsGridViewControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Question SelectedQuestion
        {
            get { return (Question)GetValue(SelectedQuestionProperty); }
            set { SetValue(SelectedQuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for groupSele.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedQuestionProperty =
            DependencyProperty.Register(nameof(SelectedQuestion), typeof(Question), typeof(QuestionsGridViewControl), new FrameworkPropertyMetadata(new Question(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<Question> Questions
        {
            get { return (ObservableCollection<Question>)GetValue(QuestionsProperty); }
            set { SetValue(QuestionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Groups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuestionsProperty =
            DependencyProperty.Register(nameof(Questions), typeof(ObservableCollection<Question>), typeof(QuestionsGridViewControl), new PropertyMetadata(new ObservableCollection<Question>(), SetItemsSource));

        private static void SetItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            QuestionsGridViewControl controll = d as QuestionsGridViewControl;
            if (controll != null)
            {
                controll.QuestionsDataGrid.ItemsSource = e.NewValue as ObservableCollection<Question>;
            }
        }

        public QuestionsGridViewControl()
        {
            InitializeComponent();
        }
    }
}