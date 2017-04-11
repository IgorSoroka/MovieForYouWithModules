using System.Collections.Generic;
using System.Linq;

namespace ModuleMainModule.Model
{
    public class RepositoryGenres
    {
        private static readonly IDictionary<int, string> Genres;

        static RepositoryGenres()
        {
            Genres = new Dictionary<int, string>
            {
                {28, "Боевик"},
                {12, "Приключения"},
                {16, "Мультфильм"},
                {35, "Комедия"},
                {80, "Криминал"},
                {99, "Документальный"},
                {18, "Драма"},
                {10751, "Семейный"},
                {36, "Исторический"},
                {27, "Ужасы"},
                {10402, "Мюзикл"},
                {14, "Фэнтэзи"},
                {10749, "Мелодрама"},
                {878, "Научная-фантастика"},
                {10770, "Телевизионный"},
                {53, "Триллер"},
                {10752, "Военный"},
                {37, "Вестерн"},
                {9648, "Мистика"}
            };
        }

        public static int GetGenreId(string name)
        {
            return Genres.FirstOrDefault(x => x.Value == name).Key;
        }

        public static List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (var item in Genres)
            {
                names.Add(item.Value);
            }
            return names;
        }
    }
}