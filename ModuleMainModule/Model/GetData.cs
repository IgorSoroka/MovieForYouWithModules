using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;

namespace MainModule
{
    public class GetData
    {
        readonly ServiceClient _first = new ServiceClient("fa314d1331397149188e07fbec92930d");
        readonly CancellationToken _token = new CancellationToken();

        public async Task<List<Movie>> GetPopularMoviesData(int page)
        {
            var movies = await _first.Movies.GetPopularAsync("ru", page, _token);
            List<Movie> popularMovies = movies.Results.ToList();
            return popularMovies;
        }

        public async Task<List<Movie>> GetNewMoviesData(int page)
        {
            var movies = await _first.Movies.GetNowPlayingAsync("ru", page, _token);
            List<Movie> newMovies = movies.Results.ToList();
            return newMovies;
        }

        public async Task<List<Movie>> GetTopRatedMoviesData(int page)
        {
            var movies = await _first.Movies.GetTopRatedAsync("ru", page, _token);
            List<Movie> topMovies = movies.Results.ToList();
            return topMovies;
        }

        public async Task<List<Movie>> GetUpCommingMoviesData(int page)
        {
            var movies = await _first.Movies.GetUpcomingAsync(null, page, _token);
            List<Movie> upcomingMovies = movies.Results.ToList();
            //upcomingMovies.OrderByDescending(item => item.ReleaseDate.Value.Date);
            return upcomingMovies;
        }

        public async Task<Movie> GetDirectMoveData(int id)
        {
            var movie = await _first.Movies.GetAsync(id, "ru", true, _token);
            return movie;
        }

        public async Task<List<Show>> GetPopularShowsData(int page)
        {
            var shows = await _first.Shows.GetPopularAsync("ru", page, _token);
            List<Show> popularShows = shows.Results.ToList();
            return popularShows;
        }

        public async Task<List<Show>> GetNowShowsData(int page)
        {
            var shows = await _first.Shows.GetAiringAsync("ru", page, null, _token);
            List<Show> nowShows = shows.Results.ToList();
            return nowShows;
        }

        public async Task<List<Show>> GetTopRatedShowsData(int page)
        {
            var shows = await _first.Shows.GetTopRatedAsync("ru", page, _token);
            List<Show> topRatedShows = shows.Results.ToList();
            return topRatedShows;
        }

        public async Task<Show> GetDirectShowData(int id)
        {
            var show = await _first.Shows.GetAsync(id, "ru", true, _token);
            return show;
        }

        public async Task<Person> GetDirectActorData(int id)
        {
            var actor = await _first.People.GetAsync(id, true, _token);
            return actor;
        }

        public async Task<List<PersonCredit>> GetDirectActorMoviesList(int id)
        {
            IEnumerable<PersonCredit> movies = await _first.People.GetCreditsAsync(id, "ru", (DataInfoType)1, _token);
            List<PersonCredit> actorMovies = movies.ToList();
            List<PersonCredit> moviesTest = (from item in actorMovies
                                             let itemReleaseDate = item.ReleaseDate
                                             where itemReleaseDate != null
                                             orderby itemReleaseDate.Value descending
                                             select item).ToList();
            return moviesTest;
        }

        public async Task<List<string>> GetGenres()
        {
            IEnumerable<Genre> genres = await _first.Genres.GetAsync((DataInfoType)1, _token);
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
                    string filepath = Path.Combine(Environment.CurrentDirectory, "Pictures", item.Poster.TrimStart('/'));
                    await DownloadImage(item.Poster, filepath, _token);
                }
            }
        }

        static async Task DownloadImage(string filename, string localpath, CancellationToken cancellationToken)
        {
            if (!File.Exists(localpath))
            {
                string folder = Path.GetDirectoryName(localpath);
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
                        Trace.TraceError(ex.ToString());
                    }
                }
            }
        }

        public async Task<List<Movie>> GetMoviesByName(string name)
        {
            var movies = await _first.Movies.SearchAsync(name, "ru", true, null, true, 1, _token);
            List<Movie> searchrMovies = movies.Results.ToList();
            return searchrMovies;
        }

        public async Task<List<Person>> GetPopActors()
        {
            List<Person> popularPersons = new List<Person>();
            var firstPerson = await _first.People.SearchAsync("Diesel", true, true, 1, _token);
            popularPersons.Add(firstPerson.Results.FirstOrDefault());
            return popularPersons;
        }

        public async Task<List<Person>> GetActorsByName(string actorName)
        {
            var searchPeople = await _first.People.SearchAsync(actorName, true, true, 1, _token);
            return searchPeople.Results.ToList();
        }

        public async Task<List<Movie>> GetSearchedMovies(decimal selectedRating)
        {
            var searchedMovies = await _first.Movies.DiscoverAsync(null, true, null, null, null, null, selectedRating, null, null, 1, _token);
            List<Movie> list = searchedMovies.Results.ToList();
            return list;
        }


        public async Task<List<Movie>> GetSearchedMovies(int? selectedYear, decimal selectedRating)
        {
            var searchedMovies = await _first.Movies.DiscoverAsync(null, true, selectedYear, null, null, null, selectedRating, null, null, 1, _token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year == selectedYear)).ToList();
            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesFirstYear(int? selectedYear, decimal selectedRating)
        {
            var firstTime = new DateTime((int)selectedYear, 1, 1);
            var searchedMovies = await _first.Movies.DiscoverAsync(null, true, null, firstTime, null, null, selectedRating, null, null, 1, _token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year > selectedYear)).ToList();
            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesLastYear(int? selectedYear, decimal selectedRating)
        {
            var lastTime = new DateTime((int)selectedYear, 12, 31);
            var searchedMovies = await _first.Movies.DiscoverAsync(null, true, null, null, lastTime, null, selectedRating, null, null, 1, _token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year < selectedYear)).ToList();
            return list;
        }

        public async Task<List<Movie>> GetSearchedMovies(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            var date1 = new DateTime(2010, 8, 18);
            var firstTime = new DateTime((int)selectedFirstYear, 1, 1);
            var secondTime = new DateTime((int)selectedLastYear, 12, 31);
            var searchedMovies = await _first.Movies.DiscoverAsync(null, true, null, firstTime, secondTime, null, selectedRating, null, null, 1, _token);
            List<Movie> list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value > firstTime && item.ReleaseDate.Value < secondTime)).ToList();
            return list;
        }

        public async Task<List<Movie>> GetListOfMoviesByCompany(int companyId, int page)
        {
            var searched = await _first.Companies.GetMoviesAsync(companyId, "ru", page, _token);
            List<Movie> list = searched.Results.ToList();
            return list;
        }

        public async Task<List<Movie>> GetListOfMoviesByGenre(int genre, int page)
        {
            var searched = await _first.Genres.GetMoviesAsync(genre, "ru", true, page, _token);
            List<Movie> list = searched.Results.ToList();
            return list;
        }

        public async Task<Video> GetTrailler(int id)
        {
            IEnumerable<Video> videosRus = await _first.Movies.GetVideosAsync(id, "ru", _token);
            if (videosRus.Count() != 0)
            { return videosRus.FirstOrDefault(); }
            IEnumerable<Video> videos = await _first.Movies.GetVideosAsync(id, null, _token);
            return videos.FirstOrDefault();
        }

        public async Task<Video> GetTraillerShow(int id)
        {
            IEnumerable<Video> videosRus = await _first.Shows.GetVideosAsync(id, null, null, "ru", _token);
            if (videosRus.Count() != 0)
            { return videosRus.FirstOrDefault(); }
            IEnumerable<Video> videos = await _first.Shows.GetVideosAsync(id, null, null, null, _token);
            return videos.FirstOrDefault();
        }

        public async Task<Person> GetActor(int id)
        {
            var searched = await _first.People.GetAsync(id, true, _token);
            return searched;
        }
        
        public async Task<List<Show>> GetShowsByName(string name)
        {
            var shows = await _first.Shows.SearchAsync(name, "ru", null, true, 1, _token);
            List<Show> searchrShows = shows.Results.ToList();
            return searchrShows;
        }

        public async Task<List<Show>> GetSearchedShows(decimal selectedRating)
        {
            var searchrShows = await _first.Shows.DiscoverAsync("ru", null, null, null, null, selectedRating, null, null, 1, _token);
            List<Show> list = searchrShows.Results.ToList();
            return list;
        }

        public async Task<List<Show>> GetSearchedShows(int? selectedYear, decimal selectedRating)
        {
            var searchrShows = await _first.Shows.DiscoverAsync("ru", selectedYear, null, null, null, selectedRating, null, null, 1, _token);
            List<Show> list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year == selectedYear)).ToList();
            return list;
        }

        public async Task<List<Show>> GetSearchedShowsFirstYear(int? selectedYear, decimal selectedRating)
        {
            var firstTime = new DateTime((int)selectedYear, 1, 1);
            var searchrShows = await _first.Shows.DiscoverAsync("ru", null, firstTime, null, null, selectedRating, null, null, 1, _token);
            List<Show> list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year > selectedYear)).ToList();
            return list;
        }

        public async Task<List<Show>> GetSearchedShowsLastYear(int? selectedYear, decimal selectedRating)
        {
            var lastTime = new DateTime((int)selectedYear, 12, 31);
            var searchrShows = await _first.Shows.DiscoverAsync("ru", null, null, lastTime, null, selectedRating, null, null, 1, _token);
            List<Show> list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year < selectedYear)).ToList();
            return list;
        }

        public async Task<List<Show>> GetSearchedShows(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            var firstTime = new DateTime((int) selectedFirstYear, 1, 1);
            var secondTime = new DateTime((int) selectedLastYear, 12, 31);
            var searchrShows = await _first.Shows.DiscoverAsync("ru", null, firstTime, secondTime, null, selectedRating, null, null, 1, _token);
            List<Show> list = (searchrShows.Results.Where(item => item.FirstAirDate.Value > firstTime && item.FirstAirDate.Value < secondTime)).ToList();
            return list;
        }
    }
}