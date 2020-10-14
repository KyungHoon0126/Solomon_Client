using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Solomon_Client.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string author = parameter.ToString();

            var writer = App.bulletinData.bulletinViewModel.BulletinItems.Where(x => x.Writer == author).FirstOrDefault();

            bool flag = (value is bool) ? (bool)value : false;
            Visibility retval = flag ? Visibility.Visible : Visibility.Collapsed;

            if (writer.ToString() == App.loginData.loginViewModel.Id)
            {
                retval = Visibility.Visible;
            }
            else
            {
                retval = Visibility.Collapsed;
            }

            return retval;

            //if ((bool)value == false && writer == App.loginData.loginViewModel.Id)
            //{
            //    return Visibility.Visible;
            //}
            //else
            //{
            //    return Visibility.Collapsed;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
