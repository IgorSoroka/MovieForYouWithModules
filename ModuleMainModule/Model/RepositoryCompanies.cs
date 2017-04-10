using System.Collections.Generic;
using System.Linq;

namespace ModuleMainModule.Model
{
    public class RepositoryCompanies
    {
        private static readonly IDictionary<int, string> Companies;

        static RepositoryCompanies()
        {
            Companies = new Dictionary<int, string>
            {
                {5, "Columbia Pictures"},
                {25, "20th Century Fox"},
                {4, "Paramount Pictures"},
                {33, "Universal Pictures"},
                {2, "Walt Disney Studios"},
                {174, "Warner Bros."},
                {1632, "Lionsgate"},
                {21, "Metro-Goldwyn-Mayer"},
                {308, "The Weinstein Company"},
                {14, "Miramax"},
                {7295, "Relativity Media"}
            };
        }

        public static int GetCompanyId(string name)
        {
            return Companies.FirstOrDefault(x => x.Value == name).Key;
        }

        public static List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (var item in Companies)
            {
                names.Add(item.Value);
            }
            return names;
        }
    }
}
