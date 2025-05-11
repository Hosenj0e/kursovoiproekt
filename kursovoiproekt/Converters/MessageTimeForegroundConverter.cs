using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace kursovoiproekt.Converters
{
    public class MessageTimeForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMyMessage)
            {
                return isMyMessage
                    ? new SolidColorBrush(Colors.LightGray)  
                    : new SolidColorBrush(Colors.DarkGray);  
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}