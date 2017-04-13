using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.TMDb;
using System.Windows.Data;

namespace ModuleMainModule.Converters
{
    public class NetworkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            try
            {
                IEnumerable<Network> networks = value as IEnumerable<Network>;
                for (int i = 0; i < networks.Count(); i++)
                {
                    result = System.IO.Path.Combine(result, " ", networks.ElementAt(i).Name);
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
