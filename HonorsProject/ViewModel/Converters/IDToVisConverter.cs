using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HonorsProject.ViewModel.Converters
{
    public class IDToVisConverter : IValueConverter
    {
        public object Convert(object id, Type targetType, object parameter, CultureInfo culture)
        {
            //if roles match required make the item visible
            if ((int)id == App.AppUser.Id)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}