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
    internal class FormContextToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FormContext formContext = (FormContext)value;
            string parameterString = parameter as string;
            if (string.IsNullOrEmpty(parameterString))
                throw new Exception("Parameters required for FormContextConverter");
            //New | Update
            string[] newOrUpdate = parameterString.Split('|');
            if (newOrUpdate.Length != 2)
                throw new Exception("Converter must only have 2 parameters for true and false");

            //if lecturer make the item visible
            if (formContext == FormContext.Create)
                return newOrUpdate[0];
            return newOrUpdate[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back not required");
        }
    }
}