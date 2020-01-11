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
                    return Brushes.Red;

                case FeedbackType.Info:
                    return Brushes.Blue;

                case FeedbackType.Success:
                    return Brushes.Green;

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