using ModuleMainModule.Model;
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
        private const string ApiKey = "fa314d1331397149188e07fbec92930d";
        private readonly ServiceClient _client = new ServiceClient(ApiKey);
        private readonly CancellationToken _token = new CancellationToken();

        public async Task<List<Movie>> GetPopularMoviesData(int page)
        {
            List<Movie> popularMovies;
            try
            {                
                var movies = await _client.Movies.GetPopularAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);               
                popularMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return popularMovies;
        }

        public async Task<List<Movie>> GetNewMoviesData(int page)
        {
            List<Movie> newMovies;
            try
            {
                var movies = await _client.Movies.GetNowPlayingAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                newMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return newMovies;
        }

        public async Task<List<Movie>> GetTopRatedMoviesData(int page)
        {
            List<Movie> topMovies;
            try
            {
                var movies = await _client.Movies.GetTopRatedAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                topMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {

                throw;
            }            
            return topMovies;
        }

        public async Task<List<Movie>> GetUpCommingMoviesData(int page)
        {
            List<Movie> upcomingMovies;
            try
            {
                var movies = await _client.Movies.GetUpcomingAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                upcomingMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }                     
            return upcomingMovies;
        }

        public async Task<Movie> GetDirectMoveData(int id)
        {
            Movie movie;
            try
            {
                movie = await _client.Movies.GetAsync(id, Thread.CurrentThread.CurrentCulture.Name, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return movie;
        }

        public async Task<List<Show>> GetPopularShowsData(int page)
        {
            List<Show> popularShows;
            try
            {
                var shows = await _client.Shows.GetPopularAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                if (shows == null)
                {
                    bool internetConnection = NetworkClient.CheckForInternetConnection();
                }
                popularShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            return popularShows;
        }

        public async Task<List<Show>> GetNowShowsData(int page)
        {
            List<Show> nowShows;
            try
            {
                var shows = await _client.Shows.GetAiringAsync(Thread.CurrentThread.CurrentCulture.Name, page, null, _token);
               nowShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            
            return nowShows;
        }

        public async Task<List<Show>> GetTopRatedShowsData(int page)
        {
            List<Show> topRatedShows;
            try
            {
                var shows = await _client.Shows.GetTopRatedAsync(Thread.CurrentThread.CurrentCulture.Name, page, _token);
                topRatedShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return topRatedShows;
        }

        public async Task<Show> GetDirectShowData(int id)
        {
            Show show;
            try
            {
                show = await _client.Shows.GetAsync(id, Thread.CurrentThread.CurrentCulture.Name, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return show;
        }

        public async Task<Person> GetDirectActorData(int id)
        {
            Person actor;
            try
            {
                actor = await _client.People.GetAsync(id, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return actor;
        }

        public async Task<List<PersonCredit>> GetDirectActorMoviesList(int id)
        {
            List<PersonCredit> moviesTest;
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
            List<Movie> searchrMovies;
            try
            {
                var movies = await _client.Movies.SearchAsync(name, Thread.CurrentThread.CurrentCulture.Name, true, null, true, 1, _token);
                searchrMovies = movies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return searchrMovies;
        }

        public async Task<List<Person>> GetPopActors()
        {
            List<Person> popularPersons = new List<Person>();
            try
            {
                var firstPerson = await _client.People.SearchAsync("Diesel", true, true, 1, _token);
                popularPersons.Add(firstPerson.Results.FirstOrDefault());
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return popularPersons;
        }

        public async Task<List<Person>> GetActorsByName(string actorName)
        {
            List<Person> list;
            try
            {
                var searchPeople = await _client.People.SearchAsync(actorName, true, true, 1, _token);
                list = searchPeople.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }
            return list;
        }

        public async Task<List<Movie>> GetSearchedMovies(decimal selectedRating)
        {
            List<Movie> list;
            try
            {
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, null, null, null, selectedRating, null, null, 1, _token);
                list = searchedMovies.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return list;
        }


        public async Task<List<Movie>> GetSearchedMovies(int? selectedYear, decimal selectedRating)
        {
            List<Movie> list;
            try
            {
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, selectedYear, null, null, null, selectedRating, null, null, 1, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year == selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesFirstYear(int? selectedYear, decimal selectedRating)
        {
            List<Movie> list;
            try
            {
                var firstTime = new DateTime((int)selectedYear, 1, 1);
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, firstTime, null, null, selectedRating, null, null, 1, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year > selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Movie>> GetSearchedMoviesLastYear(int? selectedYear, decimal selectedRating)
        {
            List<Movie> list;
            try
            {
                var lastTime = new DateTime((int)selectedYear, 12, 31);
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, null, lastTime, null, selectedRating, null, null, 1, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value.Year < selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Movie>> GetSearchedMovies(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            List<Movie> list;
            try
            {
                var date1 = new DateTime(2010, 8, 18);
                var firstTime = new DateTime((int)selectedFirstYear, 1, 1);
                var secondTime = new DateTime((int)selectedLastYear, 12, 31);
                var searchedMovies = await _client.Movies.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, true, null, firstTime, secondTime, null, selectedRating, null, null, 1, _token);
                list = (searchedMovies.Results.Where(item => item.ReleaseDate.Value > firstTime && item.ReleaseDate.Value < secondTime)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return list;
        }

        public async Task<List<Movie>> GetListOfMoviesByCompany(int companyId, int page)
        {
            List<Movie> list;
            try
            {
                var searched = await _client.Companies.GetMoviesAsync(companyId, Thread.CurrentThread.CurrentCulture.Name, page, _token);
                list = searched.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return list;
        }

        public async Task<List<Movie>> GetListOfMoviesByGenre(int genre, int page)
        {
            List<Movie> list;
            try
            {
                var searched = await _client.Genres.GetMoviesAsync(genre, Thread.CurrentThread.CurrentCulture.Name, true, page, _token);
                list = searched.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
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
            Person searched;
            try
            {
                searched = await _client.People.GetAsync(id, true, _token);
            }
            catch (ServiceRequestException)
            {
                throw;
            }           
            return searched;
        }
        
        public async Task<List<Show>> GetShowsByName(string name)
        {
            List<Show> searchrShows;
            try
            {
                var shows = await _client.Shows.SearchAsync(name, Thread.CurrentThread.CurrentCulture.Name, null, true, 1, _token);
                searchrShows = shows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return searchrShows;
        }

        public async Task<List<Show>> GetSearchedShows(decimal selectedRating)
        {
            List<Show> list;
            try
            {
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, null, null, null, selectedRating, null, null, 1, _token);
                list = searchrShows.Results.ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Show>> GetSearchedShows(int? selectedYear, decimal selectedRating)
        {
            List<Show> list;
            try
            {
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, selectedYear, null, null, null, selectedRating, null, null, 1, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year == selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Show>> GetSearchedShowsFirstYear(int? selectedYear, decimal selectedRating)
        {
            List<Show> list;
            try
            {
                var firstTime = new DateTime((int)selectedYear, 1, 1);
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, firstTime, null, null, selectedRating, null, null, 1, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year > selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Show>> GetSearchedShowsLastYear(int? selectedYear, decimal selectedRating)
        {
            List<Show> list;
            try
            {
                var lastTime = new DateTime((int)selectedYear, 12, 31);
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, null, lastTime, null, selectedRating, null, null, 1, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value.Year < selectedYear)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }

        public async Task<List<Show>> GetSearchedShows(int? selectedFirstYear, int? selectedLastYear, decimal selectedRating)
        {
            List<Show> list;
            try
            {
                var firstTime = new DateTime((int)selectedFirstYear, 1, 1);
                var secondTime = new DateTime((int)selectedLastYear, 12, 31);
                var searchrShows = await _client.Shows.DiscoverAsync(Thread.CurrentThread.CurrentCulture.Name, null, firstTime, secondTime, null, selectedRating, null, null, 1, _token);
                list = (searchrShows.Results.Where(item => item.FirstAirDate.Value > firstTime && item.FirstAirDate.Value < secondTime)).ToList();
            }
            catch (ServiceRequestException)
            {
                throw;
            }            
            return list;
        }
    }
}