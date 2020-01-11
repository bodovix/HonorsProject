using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HonorsProject.ViewModel.Converters
{
    public class RoleToStringConverter : IValueConverter
    {
        public object Convert(object userRole, Type targetType, object parameter, CultureInfo culture)
        {
            switch (userRole)
            {
                case Role.Lecturer:
                    return "Lecturer";

                case Role.Student:
                    return "Student";

                default:
                    throw new Exception("Invalid Role for role to text converter. Please contact support.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}