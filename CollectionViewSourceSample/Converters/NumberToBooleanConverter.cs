using System;
using System.Globalization;
using System.Windows.Data;

namespace CollectionViewSourceSample.Converters
{
    public class NumberToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool returnValue;
            try
            {
                if (value == null)
                {
                    return false;
                }
                int strValue = (int)value;
                returnValue = strValue > 1;
            }
            catch
            {
                returnValue = false;
            }
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
