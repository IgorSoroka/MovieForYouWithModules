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

namespace ModuleMainModule.ViewModels
{
    class MoviesListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();

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
            Title = "Лучшие фильмы";
            GetBestMovies(Page);
        }

        #region Constants

        private const string _next = "Следуюшая";
        public string Next
        {
            get { return _next; }
        }

        private const string _privious = "Предыдущая";
        public string Privious
        {
            get { return _privious; }
        }

        #endregion

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

        private string _title = "not null";
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
                        Title = "Лучшие фильмы";
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
                        Title = "Популярные фильмы";
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
                        Title = "Скоро в кино";
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
                        Title = "Сейчас в кино";
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
                    Title = "Результаты поиска";
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
                    Title = "Результаты поиска";
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    GetMoviesByName(name);
                    Title = "Результаты поиска";
                }

            
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                    if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                    {
                        GetMoviesByOnlyRating(selectedRating);
                        Title = "Результаты поиска";
                    }
                    else if (selectedYear != 0)
                    {
                        GetMoviesByYearAndRating(selectedYear, selectedRating);
                        Title = "Результаты поиска";
                    }
                    else if (selectedFirstYear != 0 && selectedLastYear != 0)
                    {
                        GetMoviesByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
                        Title = "Результаты поиска";
                    }
                    else if (selectedFirstYear == 0 || selectedLastYear == 0)
                    {
                        if (selectedFirstYear != 0 && selectedLastYear == 0)
                        {
                            GetMoviesByFirstYearAndRating(selectedFirstYear, selectedRating);
                            Title = "Результаты поиска";
                        }
                        else if (selectedFirstYear == 0 && selectedLastYear != 0)
                        {
                            GetMoviesByFLastYearAndRating(selectedLastYear, selectedRating);
                            Title = "Результаты поиска";
                        }
                    }
            }
            catch (NullReferenceException ex)
            {
                //RaiseNotificationNull();
                logger.ErrorException("MovieListViewModel", ex);
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotificationServer()
        {
            this.NotificationRequestServer.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз.", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
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
                logger.ErrorException("MovieListViewModel", e);
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
            if (_popular && Page > 1)
            {
                Page--;
               GetPopularMovies(Page);
            }

            if (_best && Page > 1)
            {
                Page--;
                GetBestMovies(Page);
            }

            if (_future && Page > 1)
            {
                Page--;
                GetUpComingMovies(Page);
            }

            if (_now && Page > 1)
            {
                Page--;
                GetNowPlayingMovies(Page);
            }

            if (_company && Page > 1)
            {
                Page--;
                GetMoviesByCompany(_selectedCompany, Page);
            }

            if (_genre && Page > 1)
            {
                Page--;
                GetMoviesByGenre(_selectedGenre, Page);
            }
        }

        private void ShowNextPage()
        {
            if (_popular && Page < 5)
            {
                Page++;
                GetPopularMovies(Page);
            }

            if (_best && Page < 5)
            {
                Page++;
                GetBestMovies(Page);
            }

            if (_future && Page < 5)
            {
                Page++;
                GetUpComingMovies(Page);
            }

            if (_now && Page < 5)
            {
                Page++;
                GetNowPlayingMovies(Page);
            }

            if (_company && Page < 5)
            {
                Page++;
                GetMoviesByCompany(_selectedCompany, Page);
            }

            if (_genre && Page < 5)
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
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }     
        }

        private async void GetMoviesByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMoviesFirstYear(selectedFirstYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private async void GetMoviesByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMovies(selectedFirstYear, selectedLastYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private async void GetMoviesByYearAndRating(int selectedYear, decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMovies(selectedYear, selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private async void GetMoviesByOnlyRating(decimal selectedRating)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetSearchedMovies(selectedRating);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private async void GetMoviesByName(string name)
        {
            try
            {
                List<Movie> moviesTest = await Data.GetMoviesByName(name);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private async void GetMoviesByGenre(string genre, int page)
        {
            try
            {
                var genreNumber = RepositoryGenres.GetGenreId(genre);
                List<Movie> moviesTest = await Data.GetListOfMoviesByGenre(genreNumber, page);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private async void GetMoviesByCompany(string company, int page)
        {
            try
            {
                var companyNumber = RepositoryCompanies.GetCompanyId(company);
                List<Movie> moviesTest = await Data.GetListOfMoviesByCompany(companyNumber, page);
                Movies = new ObservableCollection<Movie>(moviesTest);
                if (moviesTest == null)
                {
                    NotFound = "not null";
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotificationServer();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
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
                logger.ErrorException("MovieListViewModel", e);
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
                logger.ErrorException("MovieListViewModel", e);
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
                logger.ErrorException("MovieListViewModel", e);
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
                logger.ErrorException("MovieListViewModel", e);
            }
        }       
    }
        #endregion

}