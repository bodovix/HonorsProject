﻿using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
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
    /// Interaction logic for QandAPage.xaml
    /// </summary>
    public partial class QandAPage : Page
    {
        public BaseQandAPageVM VM;
        private BaseEntity entity;
        public QandAPage(BaseEntity entityToFocusOn)
        {
            entity = entityToFocusOn;
            PickVM();
            InitializeComponent();
            MainContainer.DataContext = VM;
        }

        private void PickVM()
        {
            if (App.LoggedInAs == Role.Lecturer)
            {
                if (entity is Session)
                {
                   VM = new InSessoinLecturerQandAVM(App.AppUser,(Session)entity, ConnectionConfigs.LiveConfig);
                }
                else if (entity is Question)
                {

                }
                else if (entity is Answer)
                {

                }
                else if(entity == null)
                {
                    Console.WriteLine("null");
                }
                else
                {
                    throw new InvalidCastException("Invalid Cast exception in QandA Page. Please contact support.");
                }
            }
            else
            {
                if (entity is Session)
                {

                }
                if (entity is Question)
                {

                }
                if (entity is Answer)
                {

                }
                else if (entity == null)
                {
                    Console.WriteLine("null");
                }
                else
                {
                    throw new InvalidCastException("Invalid Cast exception in QandA Page. Please contact support.");
                }
            }
        }

        private void searchAResultsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item =(sender as ListView).SelectedItem;
            if (item != null)
            {
                AnswerContainer.Visibility = Visibility.Visible;
                QuestionContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void searchQResultsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                AnswerContainer.Visibility = Visibility.Collapsed;
                QuestionContainer.Visibility = Visibility.Visible;
            }
        }
    }
}
