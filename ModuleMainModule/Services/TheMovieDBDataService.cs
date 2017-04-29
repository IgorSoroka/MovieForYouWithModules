using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 618

namespace MainModule
{
    public class TheMovieDBDataService
    {
        private readonly ServiceClient _client = new ServiceClient(ApiKey);
        private static readonly CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _token = CancelTokenSource.Token;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constants

        private const string ApiKey = "fa314d1331397149188e07fbec92930d";
        private const string ForExceptions = "TheMovieDBDataService";

        private const int FirstMonth = 1;
        private const int LastMonth = 12;
        private const int FirstDayOfMonth = 1;
        private const int LastDayOfMonth = 31;
        private const int PageForSearching = 1;

        #endregion

        #region Methods

        public async Task<List<Movie>> GetPopularMoviesData(int page)
        {
            List<Movie> popularMovies = new List<Movie>();
            try
            {                
                var movies = await _client.Movies.GetPopularAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);               
                popularMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }            
            return popularMovies;
        }

        public async Task<List<Movie>> GetNewMoviesData(int page)
        {
            List<Movie> newMovies = new List<Movie>();
            try
            {
                var movies = await _client.Movies.GetNowPlayingAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                newMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return newMovies;
        }

        public async Task<List<Movie>> GetTopRatedMoviesData(int page)
        {
            List<Movie> topMovies = new List<Movie>();
            try
            {
                var movies = await _client.Movies.GetTopRatedAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                topMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return topMovies;
        }

        public async Task<List<Movie>> GetUpCommingMoviesData(int page)
        {
            List<Movie> upcomingMovies = new List<Movie>();
            try
            {
                var movies = await _client.Movies.GetUpcomingAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                upcomingMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return upcomingMovies;
        }

        public async Task<Movie> GetDirectMoveData(int id)
        {
            Movie movie = new Movie();
            try
            {
                movie = await _client.Movies.GetAsync(id, Thread.CurrentThread.CurrentCulture.Name, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return movie;
        }

        public async Task<List<Show>> GetPopularShowsData(int page)
        {
            List<Show> popularShows = new List<Show>();
            try
            {
                var shows = await _client.Shows.GetPopularAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                popularShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return popularShows;
        }

        public async Task<List<Show>> GetNowShowsData(int page)
        {
            List<Show> nowShows = new List<Show>();
            try
            {
                var shows = await _client.Shows.GetAiringAsync(Thread.CurrentThread.CurrentCulture.Name, page, null, _token);
               nowShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return nowShows;
        }

        public async Task<List<Show>> GetTopRatedShowsData(int page)
        {
            List<Show> topRatedShows = new List<Show>();
            try
            {
                var shows = await _client.Shows.GetTopRatedAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                topRatedShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return topRatedShows;
        }

        public async Task<Show> GetDirectShowData(int id)
        {
            Show show = new Show();
            try
            {
                show = await _client.Shows.GetAsync(id, Thread.CurrentThread.CurrentCulture.Name, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return show;
        }

        public async Task<Person> GetDirectActorData(int id)
        {
            Person actor = new Person();
            try
            {
                actor = await _client.People.GetAsync(id, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return actor;
        }

        public async Task<List<PersonCredit>> GetDirectActorMoviesList(int id)
        {
            List<PersonCredit> moviesTest = new List<PersonCredit>();
            try
            {
                IEnumerable<PersonCredit> movies = await _client.People.GetCreditsAsync(id, Thread.CurrentThread.CurrentCulture.Name, (DataInfoType)1, _token);
                List<PersonCredit> actorMovies = movies.ToList();
                moviesTest = (from item in actorMovies
                              let itemReleaseDate = item.ReleaseDate
                              where itemReleaseDate != null
                              orderby itemReleaseDate.Value descending
                              select item).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return moviesTest;
        }

        public async Task<List<string>> GetGenres()
        {
            List<string> stringGenres = new List<string>();
            try
            {
                IEnumerable<Genre> genres = await _client.Genres.GetAsync((DataInfoType)1, _token);
                foreach (var item in genres)
                {
                    stringGenres.Add(item.Name);
                }
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return stringGenres;
        }
        
        public async Task DownloaderAsync(Movies movies)
        {
            foreach (var item in movies.Results)
            {
                if (!string.IsNullOrEmpty(item.Poster))
                {
                    var filepath = Path.Combine(Environment.CurrentDirectory, "Pictures", item.Poster.TrimStart('/'));
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
            List<Movie> searchrMovies = new List<Movie>();
            try
            {
                var movies = await _client.Movies.SearchAsync(name, Thread.CurrentThread.CurrentCulture.Name, true, null, true, PageForSearching, _token);
                searchrMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return searchrMovies;
        }

        public async Task<List<Person>> GetPopActors()
        {
            List<Person> popularPersons = new List<Person>();
            try
            {
                var firstPerson = await _client.People.SearchAsync("Diesel", true, true, PageForSearching, _token);
                popularPersons.Add(firstPerson.Results.FirstOrDefault());
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return popularPersons;
        }

        public async Task<List<Person>> GetActorsByName(string actorName)
        {
            List<Person> list = new List<Person>();
            try
            {
                var searchPeople = await _client.People.SearchAsync(actorName, true, true, PageForSearching, _token);
                list = searchPeople.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Movie>> GetSearchedMovies(decimal selectedRating)
        {
            List<Movie> list = new List<Movie>();
            try
            {
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, null, null, null, selectedRating, null, null, PageForSearching, _token);
                list = searchedMovies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }


        public async Task<List<Movie>> GetSearchedMovies(int? selectedYear, decimal selectedRating)
        {
            List<Movie> list = new List<Movie>();
            try
            {
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, selectedYear, null, null, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year == selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesFirstYear(int? selectedYear, decimal selectedRating)
        {
            List<Movie> list = new List<Movie>();
            try
            {
                var firstTime = new DateTime((int)selectedYear, FirstMonth, FirstDayOfMonth);
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, firstTime, null, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year > selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesLastYear(int? selectedYear, decimal selectedRating)
        {
            List<Movie> list = new List<Movie>();
            try
            {
                var lastTime = new DateTime((int)selectedYear, LastMonth, LastDayOfMonth);
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, null, lastTime, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year < selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Movie>> GetSearchedMovies(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            List<Movie> list = new List<Movie>();
            try
            {                
                var firstTime = new DateTime((int)selectedFirstYear, FirstMonth, FirstDayOfMonth);
                var secondTime = new DateTime((int)selectedLastYear, LastMonth, LastDayOfMonth);
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, firstTime, secondTime, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value > firstTime && item.ReleaseDate.Value < secondTime)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Movie>> GetListOfMoviesByCompany(int companyId, int page)
        {
            List<Movie> list = new List<Movie>();
            try
            {
                var searched = await _client.Companies.GetMoviesAsync(companyId, Thread.CurrentThread.CurrentCulture.Name, page, _token);
                list = searched.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Movie>> GetListOfMoviesByGenre(int genre, int page)
        {
            List<Movie> list = new List<Movie>();
            try
            {
                var searched = await _client.Genres.GetMoviesAsync(genre, Thread.CurrentThread.CurrentCulture.Name, true, page, _token);
                list = searched.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<Video> GetTrailler(int id)
        {
            IEnumerable<Video> videosRus = await _client.Movies.GetVideosAsync(id, Thread.CurrentThread.CurrentCulture.Name, _token);
            if (videosRus.Count() != 0)
            { return videosRus.FirstOrDefault(); }
            IEnumerable<Video> videos = await _client.Movies.GetVideosAsync(id, null, _token);
            return videos.FirstOrDefault();
        }

        public async Task<Video> GetTraillerShow(int id)
        {
            IEnumerable<Video> videosRus = await _client.Shows.GetVideosAsync(id, null, null, Thread.CurrentThread.CurrentCulture.Name, _token);
            if (videosRus.Count() != 0)
            { return videosRus.FirstOrDefault(); }
            IEnumerable<Video> videos = await _client.Shows.GetVideosAsync(id, null, null, null, _token);
            return videos.FirstOrDefault();
        }

        public async Task<Person> GetActor(int id)
        {
            Person searched = new Person();
            try
            {
                searched = await _client.People.GetAsync(id, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return searched;
        }
        
        public async Task<List<Show>> GetShowsByName(string name)
        {
            List<Show> searchrShows = new List<Show>();
            try
            {
                var shows = await _client.Shows.SearchAsync(name, Thread.CurrentThread.CurrentCulture.Name, null, true, PageForSearching, _token);
                searchrShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return searchrShows;
        }

        public async Task<List<Show>> GetSearchedShows(decimal selectedRating)
        {
            List<Show> list = new List<Show>();
            try
            {
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, null, null, null, selectedRating, null, null, PageForSearching, _token);
                list = searchrShows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Show>> GetSearchedShows(int? selectedYear, decimal selectedRating)
        {
            List<Show> list = new List<Show>();
            try
            {
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, selectedYear, null, null, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year == selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Show>> GetSearchedShowsFirstYear(int? selectedYear, decimal selectedRating)
        {
            List<Show> list = new List<Show>();
            try
            {
                var firstTime = new DateTime((int)selectedYear, FirstMonth, FirstDayOfMonth);
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, firstTime, null, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year > selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Show>> GetSearchedShowsLastYear(int? selectedYear, decimal selectedRating)
        {
            List<Show> list = new List<Show>();
            try
            {
                var lastTime = new DateTime((int)selectedYear, LastMonth, LastDayOfMonth);
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, null, lastTime, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year < selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        public async Task<List<Show>> GetSearchedShows(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            List<Show> list = new List<Show>();
            try
            {
                var firstTime = new DateTime((int)selectedFirstYear, FirstMonth, FirstDayOfMonth);
                var secondTime = new DateTime((int)selectedLastYear, LastMonth, LastDayOfMonth);
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, firstTime, secondTime, null, selectedRating, null, null, PageForSearching, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value > firstTime && item.FirstAirDate.Value < secondTime)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
            return list;
        }

        #endregion
    }
}