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
    internal class QuestionToUserInvertedBoolConverter : IValueConverter
    {
        public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
        {
            QuestionStateConverterDTO dto = (QuestionStateConverterDTO)obj;
            if (dto.Question == null || dto.User == null)
                return true;
            if (dto.Question.AskedBy == null)
                return true;

            //if roles match required make the item visible
            if (dto.User.Id == dto.Question.AskedBy.Id)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}