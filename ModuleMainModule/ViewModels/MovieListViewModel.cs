using System;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using ModuleMainModule.Model;
using Prism.Commands;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using NLog;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Services;

namespace ModuleMainModule.ViewModels
{
    class MoviesListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private  static readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly IMovieService MovieService = new MovieService();

        public DelegateCommand NavigateCommandShowDirectMovie { get; private set; }
        public DelegateCommand NavigateCommandShowNextPage { get; private set; }
        public DelegateCommand NavigateCommandShowPriviousPage { get; private set; }
        public InteractionRequest<INotification> NotificationRequestServer { get; private set; }
        public InteractionRequest<INotification> NotificationRequestNull { get; private set; }

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
            Title = _bestMovies;
            GetBestMovies(Page);
        }

        #region Constants

        private const string _next = "Следуюшая";
        public string Next
        {   get { return _next; }   }

        private const string _privious = "Предыдущая";
        public string Privious
        {   get { return _privious; }   }

        private const string _readMore = "Подробнее";
        public string ReadMore
        { get { return _readMore; } }

        private const string selectedMovies = "Избранные фильмы";
        private const string searchingResults = "Результаты поиска";
        private const string _forExceptions = "MovieListViewModel";
        private const string _exceededNumberRequests = "Превышено число запросов к серверу";
        private const string _error = "Ошибка";
        private const string _userNotified = "Пользователь был оповещен";
        private const string _errorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";
        private const string _bestMovies = "Лучшие фильмы";
        private const string _popularMovies = "Популярные фильмы";
        private const string _nowPlayingMovies = "Сейчас в кино";
        private const string _futureMovies = "Скоро в кино";

        private const int _minPage = 1;
        private const int _maxPage = 5;

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
                        Title = _bestMovies;
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
                        Title = _popularMovies;
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
                        Title = _futureMovies;
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
                        Title = _nowPlayingMovies;
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
                        Title = selectedMovies;
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
                    Title = searchingResults;
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
                    Title = searchingResults;
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    GetMoviesByName(name);
                    Title = searchingResults;
                }
                            
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                    if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                    {
                        GetMoviesByOnlyRating(selectedRating);
                        Title = searchingResults;
                    }
                    else if (selectedYear != 0)
                    {
                        GetMoviesByYearAndRating(selectedYear, selectedRating);
                        Title = searchingResults;
                    }
                    else if (selectedFirstYear != 0 && selectedLastYear != 0)
                    {
                        GetMoviesByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
                        Title = searchingResults;
                    }
                    else if (selectedFirstYear == 0 || selectedLastYear == 0)
                    {
                        if (selectedFirstYear != 0 && selectedLastYear == 0)
                        {
                            GetMoviesByFirstYearAndRating(selectedFirstYear, selectedRating);
                            Title = searchingResults;
                        }
                        else if (selectedFirstYear == 0 && selectedLastYear != 0)
                        {
                            GetMoviesByFLastYearAndRating(selectedLastYear, selectedRating);
                            Title = searchingResults;
                        }
                    }
            }
            catch (NullReferenceException ex)
            {
                //RaiseNotificationNull();
                logger.ErrorException(_forExceptions, ex);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotificationServer()
        {
            this.NotificationRequestServer.Raise(
               new Notification { Content = _exceededNumberRequests, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = _errorLoadingData, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
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
                logger.ErrorException(_forExceptions, e);
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
            if (_popular && Page > _minPage)
            {
                Page--;
               GetPopularMovies(Page);
            }
            if (_best && Page > _minPage)
            {
                Page--;
                GetBestMovies(Page);
            }
            if (_future && Page > _minPage)
            {
                Page--;
                GetUpComingMovies(Page);
            }
            if (_now && Page > _minPage)
            {
                Page--;
                GetNowPlayingMovies(Page);
            }
            if (_company && Page > _minPage)
            {
                Page--;
                GetMoviesByCompany(_selectedCompany, Page);
            }
            if (_genre && Page > _minPage)
            {
                Page--;
                GetMoviesByGenre(_selectedGenre, Page);
            }
        }

        private void ShowNextPage()
        {
            if (_popular && Page < _maxPage)
            {
                Page++;
                GetPopularMovies(Page);
            }
            if (_best && Page < _maxPage)
            {
                Page++;
                GetBestMovies(Page);
            }
            if (_future && Page < _maxPage)
            {
                Page++;
                GetUpComingMovies(Page);
            }
            if (_now && Page < _maxPage)
            {
                Page++;
                GetNowPlayingMovies(Page);
            }
            if (_company && Page < _maxPage)
            {
                Page++;
                GetMoviesByCompany(_selectedCompany, Page);
            }
            if (_genre && Page < _maxPage)
            {
                Page++;
                GetMoviesByGenre(_selectedGenre, Page);
            }
        }

        private async void GetMoviesByFLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMoviesLastYear(selectedLastYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }     
        }

        private async void GetMoviesByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMoviesFirstYear(selectedFirstYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetMoviesByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMovies(selectedFirstYear, selectedLastYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetMoviesByYearAndRating(int selectedYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMovies(selectedYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetMoviesByOnlyRating(decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMovies(selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetMoviesByName(string name)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetMoviesByName(name);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetMoviesByGenre(string genre, int page)
        {
            try
            {
                var genreNumber = RepositoryGenres.GetGenreId(genre);
                List<Movie> moviesTest = await Data.GetListOfMoviesByGenre(genreNumber, page);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetMoviesByCompany(string company, int page)
        {
            try
            {
                var companyNumber = RepositoryCompanies.GetCompanyId(company);
                List<Movie> moviesTest = await Data.GetListOfMoviesByCompany(companyNumber, page);
                Movies = new ObservableCollection<Movie>(moviesTest);                
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetPopularMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetPopularMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetBestMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetTopRatedMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetUpComingMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetUpCommingMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetNowPlayingMovies(int page)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetNewMoviesData(page);
                Movies = new ObservableCollection<Movie>(moviesTest);
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
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
                    Movie movie = await Data.GetDirectMoveData(item);
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
                logger.ErrorException(_forExceptions, e);
            }
        }
    }

    #endregion
}