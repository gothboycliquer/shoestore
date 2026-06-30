using System.Globalization;
using System.Windows.Data;

namespace ShoeStore.WPF.Converters;

public class NullToPlaceholderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string path && !string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
            return path;

        return "/Resources/Images/placeholder.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}