using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using NLog;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
#pragma warning disable 618

namespace ModuleMainModule.ViewModels
{
    class StartViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private static readonly TheMovieDBDataService DataService = new TheMovieDBDataService();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand<Movie> NavigateCommandMovie { get; private set; }
        public DelegateCommand<Show> NavigateCommandShow { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; }
        public InteractionRequest<INotification> NotificationRequestNull { get; }

        public StartViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            GetAllData();
            NavigateCommandMovie = new DelegateCommand<Movie>(ShowDirectMovie);
            NavigateCommandShow = new DelegateCommand<Show>(ShowDirectShow);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
        }

        #region StringConstants

        private const string ForExceptions = "StartViewModel";
        private const string ExceededNumberRequests = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string WaitFullDownload = "Для перехода дождитесь полной загрузки данных по выбранному Вами сериалу или актеру";
        private const string UserNotified = "Пользователь был оповещен";

        private const string _loadingData = "Загрузка данных...";
        public string LoadingData => _loadingData;

        #endregion

        #region Properties

        private Movie _bestMovie;
        public Movie BestMovie
        {
            get { return _bestMovie; }
            set { SetProperty(ref _bestMovie, value); }
        }

        private Movie _secondMovie;
        public Movie SecondMovie
        {
            get { return _secondMovie; }
            set { SetProperty(ref _secondMovie, value); }
        }

        private Movie _thirdMovie;
        public Movie ThirdMovie
        {
            get { return _thirdMovie; }
            set { SetProperty(ref _thirdMovie, value); }
        }

        private Show _bestShow;
        public Show BestShow
        {
            get { return _bestShow; }
            set { SetProperty(ref _bestShow, value); }
        }

        private Show _secondShow;
        public Show SecondShow
        {
            get { return _secondShow; }
            set { SetProperty(ref _secondShow, value); }
        }

        private Show _thirdShow;
        public Show ThirdShow
        {
            get { return _thirdShow; }
            set { SetProperty(ref _thirdShow, value); }
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

        private void RaiseNotification()
        {
            NotificationRequest.Raise(
               new Notification { Content = ExceededNumberRequests, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void RaiseNotificationNull()
        {
            NotificationRequestNull.Raise(
               new Notification { Content = WaitFullDownload, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void ShowDirectMovie(Movie movie)
        {            
            try
            {
                var id = movie.Id;
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
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

        private void ShowDirectShow(Show show)
        {
            try
            {
                var id = show.Id;
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
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

        private async void GetAllData()
        {
            try
            {
                BusyIndicatorValue = true;
                List<Movie> moviesTest = await DataService.GetPopularMoviesData(1);
                List<Show> showsTest = await DataService.GetPopularShowsData(1);
                BestMovie = moviesTest.First();
                SecondMovie = moviesTest[1];
                ThirdMovie = moviesTest[2];
                BestShow = showsTest.First();
                SecondShow = showsTest[1];
                ThirdShow = showsTest[2];


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

        #endregion
    }
}