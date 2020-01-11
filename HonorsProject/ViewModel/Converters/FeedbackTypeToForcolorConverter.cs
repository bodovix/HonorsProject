using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HonorsProject.ViewModel.Converters
{
    internal class FeedbackTypeToForcolorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FeedbackType feedbackType = (FeedbackType)value;
            switch (feedbackType)
            {
                case FeedbackType.Error:
                    return new SolidColorBrush(Colors.Red);

                case FeedbackType.Info:
                    return new SolidColorBrush(Colors.Blue);

                case FeedbackType.Success:
                    return new SolidColorBrush(Colors.Green);

                default:
                    throw new Exception("Invalid feedback type for converter. Please contact support.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}