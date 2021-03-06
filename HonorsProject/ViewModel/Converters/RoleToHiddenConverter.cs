﻿using HonorsProject.Model.Enums;
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
    internal class RoleToHiddenConverter : IValueConverter
    {
        public object Convert(object userRole, Type targetType, object parameter, CultureInfo culture)
        {
            Role paramiterToMach = (Role)parameter;
            //if roles match required make the item visible
            if ((Role)userRole == paramiterToMach)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}
