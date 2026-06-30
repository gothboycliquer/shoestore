using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ShoeStore.WPF.Converters;

public class StockToBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int quantity && quantity == 0)
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ADD8E6"));

        return new SolidColorBrush(Colors.White);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}