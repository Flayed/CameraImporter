using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ImporterGui.ValueConverters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? visibility = value as bool?;
            if (visibility.HasValue && visibility.Value) return (parameter == null ? Visibility.Visible : Visibility.Hidden);
            return (parameter == null ? Visibility.Hidden : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
