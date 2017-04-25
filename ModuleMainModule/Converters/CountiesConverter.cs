using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.TMDb;
using System.Windows.Data;
using NLog;
#pragma warning disable 618

namespace ModuleMainModule.Converters
{
    public class CountiesConverter : IValueConverter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string ForExceptions = "Converters";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            try
            {
                IEnumerable<Country> countries = value as IEnumerable<Country>;
                for (int i = 0; i < countries.Count(); i++)
                {
                    result = System.IO.Path.Combine(result, " ", countries.ElementAt(i).Name.Trim('/'));
                }
                return result;
            }
            catch (ArgumentNullException e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
