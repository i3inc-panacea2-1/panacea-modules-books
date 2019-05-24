using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Panacea.Modules.Books
{
    public class BoolToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ICommand)value[0]).CanExecute(value[1]) == true ? Application.Current.FindResource("ColorError") : Application.Current.FindResource("ColorInformation");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
