using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.TMDb;
using System.Windows.Data;

namespace ModuleMainModule.Converters
{
    public class GenresConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            try
            {
                IEnumerable<Genre> genres = value as IEnumerable<Genre>;
                for (int i = 0; i < genres.Count(); i++)
                {
                    result = System.IO.Path.Combine(result, " ", genres.ElementAt(i).Name.Trim('/'));
                }
                return result;
            }
            catch (ArgumentNullException e)
            {
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
