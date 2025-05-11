using System;
using System.Globalization;
using System.Windows.Data;

namespace kursovoiproekt.Converters
{
    public class DateTimeToLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.Kind == DateTimeKind.Utc 
                    ? dateTime.ToLocalTime() 
                    : DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}