using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using NLog;
using System;

namespace ModuleMainModule.ViewModels
{
    class MovieViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private static readonly GetData Data = new GetData();
        private static readonly IMovieService MovieService = new MovieService();
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }
        public DelegateCommand NavigateCommandShowTrailler { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        public DelegateCommand NavigateCommandDellFromDb { get; private set; }    
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public InteractionRequest<INotification> NotificationRequestNull { get; private set; }

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

        private const string _plot = "Сюжет";
        public string Plot
        {  get { return _plot; }  }

        private const string _readMore = "Подробнее";
        public string ReadMore
        { get { return _readMore; } }

        private const string _delFavorites = "Удалить из избранного";
        public string DelFavorites
        {  get { return _delFavorites; }  }

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites
        {  get { return _addFavorites; }  }

        private const string _trailer = "Смотреть трейлер";
        public string Trailer
        {  get { return _trailer; }  }

        private const string _showCast = "Состав";
        public string ShowCast
        {  get { return _showCast; }  }

        private const string _mainRoles = "В главных ролях";
        public string MainRoles
        {  get { return _mainRoles; }  }

        private const string _originalName = "Оригинальное название";
        public string OriginalName
        {  get { return _originalName; }  }

        private const string _raiting = "Рейтинг";
        public string Raiting
        {  get { return _raiting; }  }

        private const string _voteCount = "Количество голосов";
        public string VoteCount
        {   get { return _voteCount; }  }

        private const string _genres = "Жанры";
        public string Genres
        {   get { return _genres; }  }

        private const string _countries = "Страны производители";
        public string Countries
        {   get { return _countries; }  }

        private const string _keywords = "Ключевые слова";
        public string Keywords
        {   get { return _keywords; }   }

        private const string _homePage = "Домашняя страница";
        public string HomePage
        {   get { return _homePage; }   }

        private const string _premiere = "Премьера";
        public string Premiere
        {   get { return _premiere; }   }

        private const string _aboutMovie = "О фильме";
        public string AboutMovie
        {   get { return _aboutMovie; }  }

        private const string _duration = "Продолжительность";
        public string Duration
        {   get { return _duration; }   }

        private const string _budget = "Бюджет";
        public string Budget
        {   get { return _budget; }   }

        private const string _revenue = "Кассовые сборы (США)";
        public string Revenue
        {   get { return _revenue; }   }

        private const string _companies = "Компании производители";
        public string Companies
        {   get { return _companies; }  }

        private const string _forExceptions = "MovieViewModel";
        private const string _exceededNumberRequests = "Превышено число запросов к серверу";
        private const string _error = "Ошибка";
        private const string _userNotified = "Пользователь был оповещен";
        private const string _errorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";

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

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = _exceededNumberRequests, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = _errorLoadingData, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetVideoUrl(int id)
        {
            try
            {
                var video = await Data.GetTrailler(id);
                if (video != null)
                {
                    VideoUrl = video.Key;
                }
            }
            catch (ServiceRequestException ex)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetDirectMovieInfo(int id)
        {
            try
            {
                var movie = await Data.GetDirectMoveData(id);
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
            }
            catch (ServiceRequestException ex)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void AddToDb()
        {
            MovieDTO movie = new MovieDTO() { Name = DirectMovie.OriginalTitle, ExternalId = DirectMovie.Id };           
            MovieService.TakeMovie(movie);
            CanDelFromDb = true;
            CanAddToDb = false;
        }

        private void DelFromDb()
        {            
            MovieService.DelMovie(DirectMovie.Id);
            CanDelFromDb = false;
            CanAddToDb = true;
        }

        #endregion
    }
}