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
    public class KeywordsConverter : IValueConverter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string ForExceptions = "Converters";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            try
            {
                Keywords keywords = value as Keywords;
                IEnumerable<Keyword> key = keywords.Results;
                for (int i = 0; i < key.Count(); i++)
                {
                    result = System.IO.Path.Combine(result, " ", key.ElementAt(i).Name.Trim('/'));
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
