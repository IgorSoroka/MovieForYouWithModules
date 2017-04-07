using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;

namespace TestModule.Model
{
    public class GetData
    {
        ServiceClient first = new ServiceClient("fa314d1331397149188e07fbec92930d");
        CancellationToken token = new CancellationToken();

        public async Task<List<Movie>> GetPopularMoviesData()
        {
            Movies movies = await first.Movies.GetPopularAsync("ru", 1, token);
            List<Movie> popularMovies = movies.Results.ToList<Movie>();
            return popularMovies;
        }

        public async Task<List<Movie>> GetNewMoviesData()
        {
            Movies movies = await first.Movies.GetNowPlayingAsync("ru", 1, token);
            List<Movie> newMovies = movies.Results.ToList<Movie>();
            return newMovies;
        }

        public async Task<List<Movie>> GetTopRatedMoviesData()
        {
            Movies movies = await first.Movies.GetTopRatedAsync("ru", 1, token);
            List<Movie> topMovies = movies.Results.ToList<Movie>();
            return topMovies;
        }

        public async Task<List<Movie>> GetUpCommingMoviesData()
        {
            Movies movies = await first.Movies.GetUpcomingAsync("ru", 1, token);
            List<Movie> upcomingMovies = movies.Results.ToList<Movie>();
            return upcomingMovies;
        }

        public async Task<Movie> GetDirectMoveData(int id)
        {
            Movie movie = await first.Movies.GetAsync(id, "ru", true, token);
            return movie;
        }

        public async Task<List<Show>> GetPopularShowsData()
        {
            Shows shows = await first.Shows.GetPopularAsync("ru", 1, token);
            List<Show> popularShows = shows.Results.ToList<Show>();
            return popularShows;
        }

        public async Task<List<Show>> GetNowShowsData()
        {
            Shows shows = await first.Shows.GetAiringAsync("ru", 1, null, token);
            List<Show> nowShows = shows.Results.ToList<Show>();
            return nowShows;
        }

        public async Task<List<Show>> GetTopRatedShowsData()
        {
            Shows shows = await first.Shows.GetTopRatedAsync("ru", 1, token);
            List<Show> topRatedShows = shows.Results.ToList<Show>();
            return topRatedShows;
        }

        public async Task<Show> GetDirectShowData(int id)
        {
            Show show = await first.Shows.GetAsync(id, "ru", true, token);
            return show;
        }

        public async Task<Person> GetDirectActorData(int id)
        {
            Person actor = await first.People.GetAsync(id, true, token);
            return actor;
        }

        public async Task<List<PersonCredit>> GetDirectActorMoviesList(int id)
        {
            IEnumerable<PersonCredit> movies = await first.People.GetCreditsAsync(id, "ru", (DataInfoType)1, token);
            List<PersonCredit> actorMovies = movies.ToList<PersonCredit>();

            List<PersonCredit> moviesTest = (from item in actorMovies
                                             let itemReleaseDate = item.ReleaseDate
                                             where itemReleaseDate != null
                                             orderby itemReleaseDate.Value descending
                                             select item).ToList();
            return moviesTest;
        }

        public async Task<List<string>> GetGenres()
        {
            IEnumerable<Genre> genres = await first.Genres.GetAsync((DataInfoType)1, token);
            List<string> stringGenres = new List<string>();
            foreach (var item in genres)
            {
                stringGenres.Add(item.Name);
            }
            return stringGenres;
        }

        //переделать, чтобы принимал любое множество
        public async Task DownloaderAsync(Movies movies)
        {
            foreach (var item in movies.Results)
            {
                if (!string.IsNullOrEmpty(item.Poster))
                {
                    string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "Pictures", item.Poster.TrimStart('/'));
                    await DownloadImage(item.Poster, filepath, token);
                }
            }
        }

        static async Task DownloadImage(string filename, string localpath, CancellationToken cancellationToken)
        {
            if (!File.Exists(localpath))
            {
                string folder = System.IO.Path.GetDirectoryName(localpath);
                Directory.CreateDirectory(folder);

                var storage = new StorageClient();
                using (var fileStream = new FileStream(localpath, FileMode.Create, FileAccess.Write, FileShare.None, short.MaxValue, true))
                {
                    try
                    {
                        await storage.DownloadAsync(filename, fileStream, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.ToString());
                    }
                }
            }
        }

        public async Task<List<Movie>> GetMoviesByName(string name)
        {
            Movies movies = await first.Movies.SearchAsync(name, "ru", true, null, true, 1, token);
            List<Movie> searchrMovies = movies.Results.ToList<Movie>();
            return searchrMovies;
        }

        public async Task<List<Person>> GetPopActors()
        {
            List<Person> popularPersons = new List<Person>();
            People firstPerson = await this.first.People.SearchAsync("Diesel", true, true, 1, token);
            popularPersons.Add(firstPerson.Results.FirstOrDefault());

            return popularPersons;
        }

        public async Task<List<Person>> GetActorsByName(string actorName)
        {
            People searchPeople = await first.People.SearchAsync(actorName, true, true, 1, token);
            return searchPeople.Results.ToList<Person>();
        }

        public async Task<List<Movie>> GetSearchedMovies(decimal selectedRating)
        {
            Movies searchedMovies = await first.Movies.DiscoverAsync(null, true, null, null, null, null, selectedRating, null, null, 1, token);
            List<Movie> list = searchedMovies.Results.ToList<Movie>();
            return list;
        }


        public async Task<List<Movie>> GetSearchedMovies(int? selectedYear, decimal selectedRating)
        {
            Movies searchedMovies = await first.Movies.DiscoverAsync(null, true, selectedYear, null, null, null, selectedRating, null, null, 1, token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year == selectedYear)).ToList<Movie>();

            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesFirstYear(int? selectedYear, decimal selectedRating)
        {
            DateTime firstTime = new DateTime((int)selectedYear, 8, 18);
            Movies searchedMovies = await first.Movies.DiscoverAsync(null, true, null, firstTime, null, null, selectedRating, null, null, 1, token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year == selectedYear)).ToList<Movie>();

            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesLastYear(int? selectedYear, decimal selectedRating)
        {
            DateTime lastTime = new DateTime((int)selectedYear, 8, 18);
            Movies searchedMovies = await first.Movies.DiscoverAsync(null, true, null, null, lastTime, null, selectedRating, null, null, 1, token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year == selectedYear)).ToList<Movie>();

            return list;
        }

        public async Task<List<Movie>> GetSearchedMovies(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            DateTime date1 = new DateTime(2010, 8, 18);
            DateTime firstTime = new DateTime((int)selectedFirstYear, 8, 18);
            DateTime secondTime = new DateTime((int)selectedLastYear, 12, 31);
            Movies searchedMovies = await first.Movies.DiscoverAsync(null, true, null, firstTime, secondTime, null, selectedRating, null, null, 1, token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value > firstTime && item.ReleaseDate.Value < secondTime)).ToList<Movie>();

            return list;

            //var find = list.Select(item => item.Genres.Where(it => it.Name == "Comedy"));
            //var find = (from item in list
            //            where item.ReleaseDate < moment2 && item.ReleaseDate > moment1 && item.Genres.Contains(it => it.Name == "Action")
            //            select item).ToList<Movie>();

            //Movies movies = await first.Genres.GetMoviesAsync(1, "ru", true, 1, token);
        }

        public async Task<List<Movie>> GetListOfMoviesByGenre(int genre)
        {
            Movies searched = await first.Genres.GetMoviesAsync(genre, "ru", true, 1, token);
            List<Movie> list = searched.Results.ToList<Movie>();

            return list;
        }

        public async Task<Video> GetTrailler(int id)
        {
            IEnumerable<Video> videos = await first.Movies.GetVideosAsync(id, "ru", token);
            return videos.FirstOrDefault();
        }
    }
}
