using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NLog;
using Prism.Interactivity.InteractionRequest;
using System;

namespace ModuleMainModule.ViewModels
{
    class ShowViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        static readonly IShowService ShowService = new ShowService();
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }
        public DelegateCommand NavigateCommandShowTrailler { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        public DelegateCommand NavigateCommandDellFromDb { get; private set; }    
        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public ShowViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(NavigateShowDirectActor);
            NavigateCommandShowTrailler = new DelegateCommand(ShowTrailler);
            NavigateCommandAddToDb = new DelegateCommand(AddToDb);
            NavigateCommandDellFromDb = new DelegateCommand(DelFromDb);
            NotificationRequest = new InteractionRequest<INotification>();
        }

        #region Constants

        private const string _plot = "Сюжет";
        public string Plot
        {
            get { return _plot; }
        }

        private const string _delFavorites = "Удалить из избранного";
        public string DelFavorites
        {
            get { return _delFavorites; }
        }

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites
        {
            get { return _addFavorites; }
        }

        private const string _trailer = "Смотреть трейлер";
        public string Trailer
        {
            get { return _trailer; }
        }

        private const string _showCast = "Состав";
        public string ShowCast
        {
            get { return _showCast; }
        }

        private const string _mainRoles = "В главных ролях";
        public string MainRoles
        {
            get { return _mainRoles; }
        }

        private const string _originalName = "Оригинальное название";
        public string OriginalName
        {
            get { return _originalName; }
        }

        private const string _seasonsNumber = "Количество сезонов";
        public string SeasonsNumber
        {
            get { return _seasonsNumber; }
        }

        private const string _seriesNumber = "Количество вышедших серий";
        public string SeriesNumber
        {
            get { return _seriesNumber; }
        }

        private const string _raiting = "Рейтинг";
        public string Raiting
        {
            get { return _raiting; }
        }

        private const string _voteCount = "Количество голосов";
        public string VoteCount
        {
            get { return _voteCount; }
        }

        private const string _genres = "Жанры";
        public string Genres
        {
            get { return _genres; }
        }

        private const string _networks = "Сети производители";
        public string Networks
        {
            get { return _networks; }
        }

        private const string _countries = "Страны производители";
        public string Countries
        {
            get { return _countries; }
        }

        private const string _keywords = "Ключевые слова";
        public string Keywords
        {
            get { return _keywords; }
        }

        private const string _status = "Статус";
        public string Status
        {
            get { return _status; }
        }

        private const string _homePage = "Домашняя страница";
        public string HomePage
        {
            get { return _homePage; }
        }

        private const string _premiere = "Премьера первой серии";
        public string Premiere
        {
            get { return _premiere; }
        }

        private const string _lastSeries = "Последняя вышедшая серия";
        public string LastSeries
        {
            get { return _lastSeries; }
        }

        private const string _aboutShow = "О сериале";
        public string AboutShow
        {
            get { return _aboutShow; }
        }

        #endregion

        #region Properties

        private Show _direcctShow;
        public Show DirectShow
        {
            get { return _direcctShow; }
            set { SetProperty(ref _direcctShow, value); }
        }

        private MediaCast _selectedActor;
        public MediaCast SelectedActor
        {
            get { return _selectedActor; }
            set { SetProperty(ref _selectedActor, value); }
        }

        private string _videoUrl;
        public string VideoUrl
        {
            get { return _videoUrl; }
            set { SetProperty(ref _videoUrl, value); }
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
                GetDirectShowInfo(type);
                GetVideoUrl(type);
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowViewModel", e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async void GetVideoUrl(int id)
        {
            try
            {
                var video = await Data.GetTraillerShow(id);
                if (video != null)
                { VideoUrl = video.Key; }
            }
            catch (ServiceRequestException ex)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowViewModel", e);
            }            
        }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
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
                logger.ErrorException("ShowViewModel", e);
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
                logger.ErrorException("ShowViewModel", e);
            }
        }

        private async void GetDirectShowInfo(int id)
        {
            try
            {
                var show = await Data.GetDirectShowData(id);
                List<MediaCrew> crews = (show.Credits.Crew).Take(10).ToList();
                List<MediaCast> casts = (show.Credits.Cast).Take(10).ToList();
                DirectShow = show;
                Crew = new ObservableCollection<MediaCrew>(crews);
                Cast = new ObservableCollection<MediaCast>(casts);

                ShowDTO showFromDb = ShowService.GetShow(DirectShow.Id);
                if (showFromDb == null)
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
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowViewModel", e);
            }
        }

        private void AddToDb()
        {
            ShowDTO show = new ShowDTO() { Name = DirectShow.Name, ExternalId = DirectShow.Id };          
            ShowService.TakeShow(show);

            CanDelFromDb = true;
            CanAddToDb = false;
        }

        private void DelFromDb()
        {            
            ShowService.DelShow(DirectShow.Id);

            CanDelFromDb = false;
            CanAddToDb = true;
        }

        #endregion
    }
}