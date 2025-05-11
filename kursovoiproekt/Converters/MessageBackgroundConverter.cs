using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace kursovoiproekt.Converters
{
    public class MessageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMyMessage)
            {
                return isMyMessage
                    ? new SolidColorBrush(Color.FromRgb(63, 81, 181)) 
                    : new SolidColorBrush(Colors.White);              
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}