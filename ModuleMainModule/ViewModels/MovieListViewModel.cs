using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using NLog;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
#pragma warning disable 618

namespace ModuleMainModule.ViewModels
{
    class MoviesListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private  static readonly TheMovieDBDataService DataService = new TheMovieDBDataService();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly IMovieService MovieService = new MovieService();

        public DelegateCommand NavigateCommandShowDirectMovie { get; private set; }
        public DelegateCommand NavigateCommandShowNextPage { get; private set; }
        public DelegateCommand NavigateCommandShowPriviousPage { get; private set; }
        public InteractionRequest<INotification> NotificationRequestServer { get; }
        public InteractionRequest<INotification> NotificationRequestNull { get; }

        private bool _best;
        private bool _popular;
        private bool _future;
        private bool _now;
        private bool _genre;
        private bool _company;
        private string _selectedGenre;
        private string _selectedCompany;

        public MoviesListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand(NavigateShowDirectMovie);
            NavigateCommandShowNextPage = new DelegateCommand(ShowNextPage);
            NavigateCommandShowPriviousPage = new DelegateCommand(ShowPriviousPage);
            NotificationRequestServer = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();

            Page = 1;
            _best = true;
            Title = BestMovies;
            GetBestMovies(Page);
        }

        #region Constants

        private const string _next = "Следуюшая";
        public string Next => _next;

        private const string _privious = "Предыдущая";
        public string Privious => _privious;

        private const string _readMore = "Подробнее";
        public string ReadMore => _readMore;

        private const string SelectedMovies = "Избранные фильмы";
        private const string SearchingResults = "Результаты поиска";
        private const string ForExceptions = "MovieListViewModel";
        private const string ExceededNumberRequests = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string UserNotified = "Пользователь был оповещен";
        private const string ErrorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";
        private const string BestMovies = "Лучшие фильмы";
        private const string PopularMovies = "Популярные фильмы";
        private const string NowPlayingMovies = "Сейчас в кино";
        private const string FutureMovies = "Скоро в кино";

        private const int MinPage = 1;
        private const int MaxPage = 5;

        #endregion

        #region Properties

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set { SetProperty(ref _selectedMovie, value); }
        }

        private int _page;
        public int Page
        {
            get { return _page; }
            set { SetProperty(ref _page, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _notFound;
        public string NotFound
        {
            get { return _notFound; }
            set { SetProperty(ref _notFound, value); }
        }

        private ObservableCollection<Movie> _movies;
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set { SetProperty(ref _movies, value); }
        }

        public string InteractionResultMessage { get; private set; }

        #endregion

        #region Methods

        public void OnNavigatedTo(NavigationContext navigationContext)
        {            
            Page = 1;
            try
            {
                var type = navigationContext.Parameters["type"] as string;
                if (type != null)
                {
                    if (type == "Best")
                    {
                        _best = true;
                        _popular = false;
                        _future = false;
                        _now = false;
                        _genre = false;
                        _company = false;
                        GetBestMovies(Page);
                        Title = BestMovies;
                    }
                    if (type == "Popular")
                    {
                        _best = false;
                        _popular = true;
                        _future = false;
                        _now = false;
                        _genre = false;
                        _company = false;
                        GetPopularMovies(Page);
                        Title = PopularMovies;
                    }
                    if (type == "Future")
                    {
                        _best = false;
                        _popular = false;
                        _future = true;
                        _now = false;
                        _genre = false;
                        _company = false;
                        GetUpComingMovies(Page);
                        Title = FutureMovies;
                    }
                    if (type == "Now")
                    {
                        _best = false;
                        _popular = false;
                        _future = false;
                        _now = true;
                        _genre = false;
                        _company = false;
                        GetNowPlayingMovies(Page);
                        Title = NowPlayingMovies;
                    }
                    if (type == "Favorite")
                    {
                        _best = false;
                        _popular = false;
                        _future = false;
                        _now = false;
                        _genre = false;
                        _company = false;
                        GetFavoriteMovies();
                        Title = SelectedMovies;
                    }
                }

                _selectedGenre = null;
                _selectedGenre = navigationContext.Parameters["genre"] as string;
                if (_selectedGenre != null)
                {
                    _best = false;
                    _popular = false;
                    _future = false;
                    _now = false;
                    _genre = true;
                    _company = false;
                    GetMoviesByGenre(_selectedGenre, Page);
                    Title = SearchingResults;
                }

                _selectedCompany = null;
                _selectedCompany = navigationContext.Parameters["company"] as string;
                if (_selectedCompany != null)
                {
                    _best = false;
                    _popular = false;
                    _future = false;
                    _now = false;
                    _genre = false;
                    _company = true;
                    GetMoviesByCompany(_selectedCompany, Page);
                    Title = SearchingResults;
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    GetMoviesByName(name);
                    Title = SearchingResults;
                }
                            
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                    if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                    {
                        GetMoviesByOnlyRating(selectedRating);
                        Title = SearchingResults;
                    }
                    else if (selectedYear != 0)
                    {
                        GetMoviesByYearAndRating(selectedYear, selectedRating);
                        Title = SearchingResults;
                    }
                    else if (selectedFirstYear != 0 && selectedLastYear != 0)
                    {
                        GetMoviesByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
                        Title = SearchingResults;
                    }
                    else if (selectedFirstYear == 0 || selectedLastYear == 0)
                    {
                        if (selectedFirstYear != 0 && selectedLastYear == 0)
                        {
                            GetMoviesByFirstYearAndRating(selectedFirstYear, selectedRating);
                            Title = SearchingResults;
                        }
                        else if (selectedFirstYear == 0 && selectedLastYear != 0)
                        {
                            GetMoviesByFLastYearAndRating(selectedLastYear, selectedRating);
                            Title = SearchingResults;
                        }
                    }
            }
            catch (NullReferenceException ex)
            {
                //RaiseNotificationNull();
                _logger.ErrorException(ForExceptions, ex);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotificationServer()
        {
            NotificationRequestServer.Raise(
               new Notification { Content = ExceededNumberRequests, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void RaiseNotificationNull()
        {
            NotificationRequestNull.Raise(
               new Notification { Content = ErrorLoadingData, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void NavigateShowDirectMovie()
        {
            try
            { 
                var parameters = new NavigationParameters { { "id", SelectedMovie.Id } };
                _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        //private bool CanExecutePriviousPage()
        //{
        //    if (Page == 1)
        //        return false;
        //    else
        //        return true;
        //}

        //private bool CanExecuteNextPage()
        //{
        //    if (Page == 5)
        //        return false;
        //    else
        //        return true;
        //}

        private void ShowPriviousPage()
        {
            if (_popular && Page > MinPage)
            {
                Page--;
               GetPopularMovies(Page);
            }
            if (_best && Page > MinPage)
            {
                Page--;
                GetBestMovies(Page);
            }
            if (_future && Page > MinPage)
            {
                Page--;
                GetUpComingMovies(Page);
            }
            if (_now && Page > MinPage)
            {
                Page--;
                GetNowPlayingMovies(Page);
            }
            if (_company && Page > MinPage)
            {
                Page--;
                GetMoviesByCompany(_selectedCompany, Page);
            }
            if (_genre && Page > MinPage)
            {
                Page--;
                GetMoviesByGenre(_selectedGenre, Page);
            }
        }

        private void ShowNextPage()
        {
            if (_popular && Page < MaxPage)
            {
                Page++;
                GetPopularMovies(Page);
            }
            if (_best && Page < MaxPage)
            {
                Page++;
                GetBestMovies(Page);
            }
            if (_future && Page < MaxPage)
            {
                Page++;
                GetUpComingMovies(Page);
            }
            if (_now && Page < MaxPage)
            {
                Page++;
                GetNowPlayingMovies(Page);
            }
            if (_company && Page < MaxPage)
            {
                Page++;
                GetMoviesByCompany(_selectedCompany, Page);
            }
            if (_genre && Page < MaxPage)
            {
                Page++;
                GetMoviesByGenre(_selectedGenre, Page);
            }
        }

        private async void GetMoviesByFLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetSearchedMoviesLastYear(selectedLastYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }     
        }

        private async void GetMoviesByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetSearchedMoviesFirstYear(selectedFirstYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetMoviesByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetSearchedMovies(selectedFirstYear, selectedLastYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetMoviesByYearAndRating(int selectedYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetSearchedMovies(selectedYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetMoviesByOnlyRating(decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetSearchedMovies(selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetMoviesByName(string name)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetMoviesByName(name);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetMoviesByGenre(string genre, int page)
        {
            try
            {
                var genreNumber = RepositoryGenres.GetGenreId(genre);
                List<Movie> moviesTest = await DataService.GetListOfMoviesByGenre(genreNumber, page);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetMoviesByCompany(string company, int page)
        {
            try
            {
                var companyNumber = RepositoryCompanies.GetCompanyId(company);
                List<Movie> moviesTest = await DataService.GetListOfMoviesByCompany(companyNumber, page);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetPopularMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetPopularMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetBestMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetTopRatedMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetUpComingMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetUpCommingMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetNowPlayingMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await DataService.GetNewMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetFavoriteMovies()
        {
            try
            {
                IEnumerable<MovieDTO> favoriteMoviesFromDb = MovieService.GetMovies();
                List<int> moviesId = new List<int>();
                foreach (var item in favoriteMoviesFromDb)
                {
                    moviesId.Add(item.ExternalId);
                }
                List<Movie> favoriteMoviesFromSite = new List<Movie>();
                foreach (var item in moviesId)
                {
                    Movie movie = await DataService.GetDirectMoveData(item);
                    favoriteMoviesFromSite.Add(movie);
                }
                Movies = new ObservableCollection<Movie>(favoriteMoviesFromSite);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }
    }

    #endregion
}