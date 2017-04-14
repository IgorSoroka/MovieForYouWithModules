using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.TMDb;
using System.Windows.Data;

namespace ModuleMainModule.Converters
{
    public class CompaniesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            try
            {
                IEnumerable<Company> companies = value as IEnumerable<Company>;
                for (int i = 0; i < companies.Count(); i++)
                {
                    string item = companies.ElementAt(i).Name.TrimStart('\\');
                    result = System.IO.Path.Combine(result, " ", item);
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
