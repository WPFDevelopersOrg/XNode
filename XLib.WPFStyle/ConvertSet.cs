using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace XLib.WPFStyle
{
    public class TagToTrackMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tagString = (string)value;
            if (tagString == null) return new Thickness();

            Thickness thickness = (Thickness)new ThicknessConverter().ConvertFromString(tagString);
            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}