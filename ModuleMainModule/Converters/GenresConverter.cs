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
    public class GenresConverter : IValueConverter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string ForExceptions = "Converters";

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
            catch (ArgumentNullException)
            {}
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
