using HonorsProject.Model.Core;
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
    /// Interaction logic for InOrOutControl.xaml
    /// </summary>
    public partial class InOrOutControl : UserControl
    {
        public string AvailableTxt
        {
            get { return (string)GetValue(AvailableTxtProperty); }
            set { SetValue(AvailableTxtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableTxt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableTxtProperty =
            DependencyProperty.Register(nameof(AvailableTxt), typeof(string), typeof(InOrOutControl), new PropertyMetadata("", SetAvailableTxt));

        private static void SetAvailableTxt(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InOrOutControl controll = d as InOrOutControl;
            if (controll != null)
            {
                controll.AvailableTxtBlock.Text = e.NewValue as string;
            }
        }

        public string ContainsTxt
        {
            get { return (string)GetValue(ContainsTxtProperty); }
            set { SetValue(ContainsTxtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableTxt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContainsTxtProperty =
            DependencyProperty.Register(nameof(ContainsTxt), typeof(string), typeof(InOrOutControl), new PropertyMetadata("", SetContainsTxt));

        private static void SetContainsTxt(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InOrOutControl controll = d as InOrOutControl;
            if (controll != null)
            {
                controll.ContainsTxtBlock.Text = e.NewValue as string;
            }
        }

        public IEnumerable<BaseEntity> EntitiesAvailable
        {
            get { return (IEnumerable<BaseEntity>)GetValue(EntitiesAvailableProperty); }
            set { SetValue(EntitiesAvailableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Entities.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EntitiesAvailableProperty =
            DependencyProperty.Register(nameof(EntitiesAvailable), typeof(IEnumerable<BaseEntity>), typeof(InOrOutControl));

        public IEnumerable<BaseEntity> EntitiesOwned
        {
            get { return (IEnumerable<BaseEntity>)GetValue(EntitiesOwnedProperty); }
            set { SetValue(EntitiesOwnedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Entities.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EntitiesOwnedProperty =
            DependencyProperty.Register(nameof(EntitiesOwned), typeof(IEnumerable<BaseEntity>), typeof(InOrOutControl), new PropertyMetadata(SetOwnedItemsSource));

        private static void SetAvailableItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InOrOutControl controll = d as InOrOutControl;
            if (controll != null)
            {
                controll.OutItemsLV.ItemsSource = e.NewValue as IEnumerable<BaseEntity>;
            }
        }

        private static void SetOwnedItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InOrOutControl controll = d as InOrOutControl;
            if (controll != null)
            {
                controll.InItemsLV.ItemsSource = e.NewValue as IEnumerable<BaseEntity>;
            }
        }

        //public SaveCmd SaveCmd
        //{
        //    get { return (SaveCmd)GetValue(SaveCmdProperty); }
        //    set { SetValue(SaveCmdProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SaveCmdProperty =
        //    DependencyProperty.Register(nameof(SaveCmd), typeof(int), typeof(InOrOutControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public InOrOutControl()
        {
            InitializeComponent();
        }
    }
}