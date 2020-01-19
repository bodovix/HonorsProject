using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HonorsProject.Model.Enums;
using System.Windows.Media;

namespace HonorsProject.View.ExtensionMethods
{
    public static class PagesExtenstionMethods
    {
        public static void SetMenuButtonColor(this Page page, MenuButtonsSelection menuButtonsSelection)
        {
            //reset all buttons
            Clear();
            //highlight selected button
            SolidColorBrush highlighted = Brushes.LightGreen;

            switch (menuButtonsSelection)
            {
                case MenuButtonsSelection.DataAnalysisPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).DataAnalysisBtn.Background = highlighted;
                    break;

                case MenuButtonsSelection.GroupPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GroupsBtn.Background = highlighted;
                    break;

                case MenuButtonsSelection.MyAccountPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).MyAccountBtn.Background = highlighted;
                    break;

                case MenuButtonsSelection.QuesstionsPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).MyQuestoins.Background = highlighted;
                    break;

                case MenuButtonsSelection.AnswersPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).MyAnswers.Background = highlighted;
                    break;

                case MenuButtonsSelection.MySessionPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).MySessionsBtn.Background = highlighted;
                    break;

                case MenuButtonsSelection.StudentsPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsBtn.Background = highlighted;
                    break;

                case MenuButtonsSelection.LecturersPage:
                    ((MainWindow)System.Windows.Application.Current.MainWindow).LecturersBtn.Background = highlighted;
                    break;

                case MenuButtonsSelection.ClearAll:
                    Clear();
                    break;

                default:
                    throw new Exception("Invalid MenuButtonColor selection. Please contact support.");
            }
        }

        private static void Clear()
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).DataAnalysisBtn.Background = Brushes.LightGray;
            ((MainWindow)System.Windows.Application.Current.MainWindow).GroupsBtn.Background = Brushes.LightGray;
            ((MainWindow)System.Windows.Application.Current.MainWindow).MyAccountBtn.Background = Brushes.LightGray;
            ((MainWindow)System.Windows.Application.Current.MainWindow).MyAnswers.Background = Brushes.LightGray;
            ((MainWindow)System.Windows.Application.Current.MainWindow).MyQuestoins.Background = Brushes.LightGray;
            ((MainWindow)System.Windows.Application.Current.MainWindow).MySessionsBtn.Background = Brushes.LightGray;
            ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsBtn.Background = Brushes.LightGray;
        }
    }
}