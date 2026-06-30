using System.Globalization;
using System.Windows.Data;

namespace ShoeStore.WPF.Converters;

public class PriceWithDiscountConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is decimal price && values[1] is decimal discount && discount > 0)
            return Math.Round(price * (1 - discount / 100), 2).ToString("N2") + " ₽";

        if (values[0] is decimal originalPrice)
            return originalPrice.ToString("N2") + " ₽";

        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}