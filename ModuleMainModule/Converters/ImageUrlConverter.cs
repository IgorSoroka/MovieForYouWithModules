using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ModuleMainModule.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "/Images/noImage3.jpg");
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
            else
            {
                string path = String.Concat(@"http://image.tmdb.org/t/p/original", (string)value);
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
