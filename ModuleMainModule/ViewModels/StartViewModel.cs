using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using NLog;
using System;
using ModuleMainModule.Model;

namespace ModuleMainModule.ViewModels
{
    class StartViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand<Movie> NavigateCommandMovie { get; private set; }
        public DelegateCommand<Show> NavigateCommandShow { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public InteractionRequest<INotification> NotificationRequestNull { get; private set; }

        public StartViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            GetAllData();
            NavigateCommandMovie = new DelegateCommand<Movie>(ShowDirectMovie);
            NavigateCommandShow = new DelegateCommand<Show>(ShowDirectShow);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
        }

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

        public string InteractionResultMessage { get; private set; }
        #endregion

        #region Methods

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = "Для перехода дождитесь полной загрузки данных по выбранному вами фильму или сериалу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void ShowDirectMovie(Movie movie)
        {            
            try
            {
                var id = movie.Id;
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
            }
            catch (System.NullReferenceException ex)
            {
                RaiseNotificationNull();
                logger.ErrorException("StartViewModel", ex);
            }
            catch (Exception e)
            {
                logger.ErrorException("StartViewModel", e);
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
            catch (System.NullReferenceException ex)
            {
                RaiseNotificationNull();
                logger.ErrorException("StartViewModel", ex);
            }
            catch (Exception e)
            {
                logger.ErrorException("StartViewModel", e);
            }
        }

        private async void GetAllData()
        {
            try
            {
                List<Movie> moviesTest = await Data.GetPopularMoviesData(1);
                BestMovie = moviesTest.First();
                SecondMovie = moviesTest[1];
                ThirdMovie = moviesTest[2];
                List<Show> showsTest = await Data.GetPopularShowsData(1);
                BestShow = showsTest.First();
                SecondShow = showsTest[1];
                ThirdShow = showsTest[2];
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();                
            }
            catch (Exception e)
            {
                logger.ErrorException("StartViewModel", e);
            }
        }

        #endregion
    }
}