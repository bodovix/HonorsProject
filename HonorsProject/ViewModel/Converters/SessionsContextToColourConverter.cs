using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HonorsProject.ViewModel.Converters
{
    internal class SessionsContextToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SubgridContext sessionsContext = (SubgridContext)value;
            SubgridContext controlContext = (SubgridContext)parameter;
            //if the context is the same as the button set its colour to active
            if (sessionsContext == controlContext)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}