using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace FileWatcher.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return ((bool)value ^ Inverse) ? VisibilityBoxes.VisibleBox : VisibilityBoxes.CollapsedBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class VisibilityBoxes
    {
        public static readonly object VisibleBox = Visibility.Visible;
        public static readonly object HiddenBox = Visibility.Hidden;
        public static readonly object CollapsedBox = Visibility.Collapsed;

        public static object Box(Visibility value)
        {
            switch (value)
            {
                case Visibility.Visible:
                    return VisibleBox;
                case Visibility.Hidden:
                    return HiddenBox;
            }
            return CollapsedBox;
        }
    }
}
