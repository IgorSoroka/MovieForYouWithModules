using System;
using System.Globalization;
using System.Windows.Data;

namespace ModuleMainModule.Converters
{
    class VideoUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = (string)value;
            if (url != null)
            {
                string embedUrl = url.Substring(0, url.IndexOf("&")).Replace("watch?v=", "embed/");
                return embedUrl;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
