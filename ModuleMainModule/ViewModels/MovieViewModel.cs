using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using System.Windows.Controls;
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
    class MovieViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private static readonly TheMovieDBDataService DataService = new TheMovieDBDataService();
        private static readonly IMovieService MovieService = new MovieService();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }
        public DelegateCommand NavigateCommandShowTrailler { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        public DelegateCommand NavigateCommandDellFromDb { get; private set; }    
        public InteractionRequest<INotification> NotificationRequest { get; }
        public InteractionRequest<INotification> NotificationRequestNull { get; }

        public MovieViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(NavigateShowDirectActor);
            NavigateCommandShowTrailler = new DelegateCommand(ShowTrailler);
            NavigateCommandAddToDb = new DelegateCommand(AddToDb);
            NavigateCommandDellFromDb = new DelegateCommand(DelFromDb);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
        }

        #region Constants

        private const string _loadingData = "Загрузка данных...";
        public string LoadingData => _loadingData;

        private const string _plot = "Сюжет";
        public string Plot => _plot;

        private const string _readMore = "Подробнее";
        public string ReadMore => _readMore;

        private const string _delFavorites = "Удалить из избранного";
        public string DelFavorites => _delFavorites;

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites => _addFavorites;

        private const string _trailer = "Смотреть трейлер";
        public string Trailer => _trailer;

        private const string _showCast = "Состав";
        public string ShowCast => _showCast;

        private const string _mainRoles = "В главных ролях";
        public string MainRoles => _mainRoles;

        private const string _originalName = "Оригинальное название";
        public string OriginalName => _originalName;

        private const string _raiting = "Рейтинг";
        public string Raiting => _raiting;

        private const string _voteCount = "Количество голосов";
        public string VoteCount => _voteCount;

        private const string _genres = "Жанры";
        public string Genres => _genres;

        private const string _countries = "Страны производители";
        public string Countries => _countries;

        private const string _keywords = "Ключевые слова";
        public string Keywords => _keywords;

        private const string _homePage = "Домашняя страница";
        public string HomePage => _homePage;

        private const string _premiere = "Премьера";
        public string Premiere => _premiere;

        private const string _aboutMovie = "О фильме";
        public string AboutMovie => _aboutMovie;

        private const string _duration = "Продолжительность";
        public string Duration => _duration;

        private const string _budget = "Бюджет";
        public string Budget => _budget;

        private const string _revenue = "Кассовые сборы (США)";
        public string Revenue => _revenue;

        private const string _companies = "Компании производители";
        public string Companies => _companies;

        private const string ForExceptions = "MovieViewModel";
        private const string ExceededNumberRequests = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string UserNotified = "Пользователь был оповещен";
        private const string ErrorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";

        #endregion

        #region Properties

        private string _videoUrl;
        public string VideoUrl
        {
            get { return _videoUrl; }
            set { SetProperty(ref _videoUrl, value); }
        }

        private Movie _direcctMovie;
        public Movie DirectMovie
        {
            get { return _direcctMovie; }
            set { SetProperty(ref _direcctMovie, value); }
        }

        private MediaCast _selectedActor;
        public MediaCast SelectedActor
        {
            get { return _selectedActor; }
            set { SetProperty(ref _selectedActor, value); }
        }

        private ObservableCollection<MediaCast> _cast;
        public ObservableCollection<MediaCast> Cast
        {
            get { return _cast; }
            set { SetProperty(ref _cast, value); }
        }

        private ObservableCollection<MediaCrew> _crew;
        public ObservableCollection<MediaCrew> Crew
        {
            get { return _crew; }
            set { SetProperty(ref _crew, value); }
        }

        private bool _canAddToDb;
        public bool CanAddToDb
        {
            get { return _canAddToDb; }
            set { SetProperty(ref _canAddToDb, value); }
        }

        private bool _canDelFromDb;
        public bool CanDelFromDb
        {
            get { return _canDelFromDb; }
            set { SetProperty(ref _canDelFromDb, value); }
        }

        private bool _busyIndicator;
        public bool BusyIndicatorValue
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }

        public string InteractionResultMessage { get; private set; }

        #endregion

        #region Methods

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                VideoUrl = null;
                var type = (int)navigationContext.Parameters["id"];
                GetDirectMovieInfo(type);
                GetVideoUrl(type);
            }
            catch (NullReferenceException ex)
            {
                RaiseNotificationNull();
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

        private void RaiseNotification()
        {
            NotificationRequest.Raise(
               new Notification { Content = ExceededNumberRequests, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void RaiseNotificationNull()
        {
            NotificationRequestNull.Raise(
               new Notification { Content = ErrorLoadingData, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void ShowTrailler()
        {
            try
            {
                var parameters = new NavigationParameters { { "VideoUrl", VideoUrl } };
                _regionManager.RequestNavigate("MainRegion", "Player", parameters);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private void NavigateShowDirectActor()
        {
            try
            {
                var parameters = new NavigationParameters { { "id", SelectedActor.Id } };
                _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetVideoUrl(int id)
        {
            try
            {
                var video = await DataService.GetTrailler(id);
                if (video != null)
                {
                    VideoUrl = video.Key;
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetDirectMovieInfo(int id)
        {
            try
            {
                BusyIndicatorValue = true;
                var movie = await DataService.GetDirectMoveData(id);
                List<MediaCrew> crews = (movie.Credits.Crew).Take(10).ToList();
                List<MediaCast> casts = (movie.Credits.Cast).Take(10).ToList();
                DirectMovie = movie;
                Crew = new ObservableCollection<MediaCrew>(crews);
                Cast = new ObservableCollection<MediaCast>(casts);
                MovieDTO movieFromDb = MovieService.GetMovie(DirectMovie.Id);
                if (movieFromDb == null)
                {
                    CanDelFromDb = false;
                    CanAddToDb = true;
                }
                else
                {
                    CanDelFromDb = true;
                    CanAddToDb = false;
                }
                BusyIndicatorValue = false;

             
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private void AddToDb()
        {
            MovieDTO movie = new MovieDTO { Name = DirectMovie.OriginalTitle, ExternalId = DirectMovie.Id };           
            MovieService.TakeMovie(movie);
            CanDelFromDb = true;
            CanAddToDb = false;

            RefreshFavoriteView();
        }

        private void DelFromDb()
        {            
            MovieService.DelMovie(DirectMovie.Id);
            CanDelFromDb = false;
            CanAddToDb = true;

            RefreshFavoriteView();
        }

        private void RefreshFavoriteView()
        {
            try
            {
                UserControl singleView = (UserControl)_regionManager.Regions["ListRegion"].ActiveViews.FirstOrDefault();
                MoviesListViewModel movieViewModel = (MoviesListViewModel)singleView.DataContext;

                if (movieViewModel.Title == "Избранные фильмы")
                {
                    var parameters = new NavigationParameters { { "type", "Favorite" } };
                    _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
                }
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        #endregion
    }
}