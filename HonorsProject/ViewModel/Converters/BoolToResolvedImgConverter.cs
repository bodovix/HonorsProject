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
    internal class BoolToResolvedImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isResolved = (bool)value;

            if (isResolved)
                return  new Uri("../../View/Images/tickIcon.png", UriKind.Relative); 
            return new Uri("../../View/Images/crossIcon.png", UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}