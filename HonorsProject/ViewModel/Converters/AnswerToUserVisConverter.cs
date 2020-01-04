using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.DTO;
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
    internal class AnswerToUserVisConverter : IValueConverter
    {
        public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
        {
            AnswerStateConverterDTO dto = (AnswerStateConverterDTO)obj;
            if(dto.Answer == null || dto.User == null)
                return Visibility.Collapsed;
            if(dto.Answer.AnsweredBy == null)
                return Visibility.Collapsed;
            //if roles match required make the item visible
            if (dto.User.Id == dto.Answer.AnsweredBy.Id)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}
