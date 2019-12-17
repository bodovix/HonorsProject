﻿using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel;
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
    /// Interaction logic for GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        private IMyGroupsPageVM VM;

        public GroupPage()
        {
            InitializeComponent();
            DataContext = VM;
        }

        private void CreateMyGroupsVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
                VM = new MyGroupsLecturerPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
            else
                VM = new MyGroupsStudentPageVM(App.AppUser, ConnectionConfigs.LiveConfig);
        }
    }
}