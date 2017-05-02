using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using System.Threading.Tasks;
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
    class ActorsListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly TheMovieDBDataService _dataService;
        private readonly Logger _logger;
        private readonly IActorService _actorService;

        #region StringConstants

        private const string _readMore = "Подробнее";
        public string ReadMore => _readMore;

        private const string _loadingData = "Загрузка данных...";
        public string LoadingData => _loadingData;

        private const string ForExceptions = "ActorListViewModel";
        private const string ExceededNumberRequests = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string ErrorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";
        private const string UserNotified = "Пользователь был оповещен";
        private const string SelectedActors = "Избранные актеры";
        private const string SearchingResults = "Результаты поиска";

        #endregion

        public InteractionRequest<INotification> NotificationRequest { get; }
        public InteractionRequest<INotification> NotificationRequestNull { get; }
        public DelegateCommand<int?> NavigateCommandShowDirectActor { get; private set; }

        public ActorsListViewModel(RegionManager regionManager, TheMovieDBDataService dataService, ActorService actorService)
        {
            _regionManager = regionManager;
            _dataService = dataService;
            _actorService = actorService;
            _logger = LogManager.GetCurrentClassLogger();

            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
            NavigateCommandShowDirectActor = new DelegateCommand<int?>(ShowDirectActor);
        }

        #region Properties

        private Person _selectedSearchedActor;
        public Person SelectedSearchedActor
        {
            get { return _selectedSearchedActor; }
            set { SetProperty(ref _selectedSearchedActor, value); }
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

        private bool _busyIndicator;
        public bool BusyIndicatorValue
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }

        private ObservableCollection<Person> _actorsList;
        public ObservableCollection<Person> ActorsList
        {
            get { return _actorsList; }
            set { SetProperty(ref _actorsList, value); }
        }

        public string InteractionResultMessage { get; private set; }

        #endregion

        #region Methods

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var type = navigationContext.Parameters["type"] as string;
                if (type != null)
                {     
                    GetFavoriteActors();
                    Title = SelectedActors;                    
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    await GetSearchedActors(name);
                    Title = SearchingResults;
                }                  
            }
            catch (NullReferenceException)
            {
                RaiseNotificationNull();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async Task GetSearchedActors(string name)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Person> actorsTest = await _dataService.GetActorsByName(name);
                ActorsList = new ObservableCollection<Person>(actorsTest);
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

        private void ShowDirectActor(int? id)
        {
            try
            {
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }
        /// <summary>
        ///  Метод получения списка избранных актеров из базы даннных
        /// </summary>
        private async void GetFavoriteActors()
        {
            try
            {
                BusyIndicatorValue = true;
                IEnumerable<ActorDTO> favoriteActorsFromDb = _actorService.GetActors();
                List<int> actorsId = new List<int>();
                foreach (var item in favoriteActorsFromDb)
                {
                    actorsId.Add(item.ExternalId);
                }
                List<Person> favoriteActorsFromSite = new List<Person>();
                foreach (var item in actorsId)
                {
                    Person actor = await _dataService.GetDirectActorData(item);
                    favoriteActorsFromSite.Add(actor);
                }
                ActorsList = new ObservableCollection<Person>(favoriteActorsFromSite);
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