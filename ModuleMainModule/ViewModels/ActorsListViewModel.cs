using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using System.Threading.Tasks;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NLog;
using Prism.Interactivity.InteractionRequest;
using System;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Services;
using ModuleMainModule.Model;

namespace ModuleMainModule.ViewModels
{
    class ActorsListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private static readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly IActorService ActorService = new ActorService();

        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public InteractionRequest<INotification> NotificationRequestNull { get; private set; }

        public ActorsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(ShowDirectActor);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
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

        private ObservableCollection<Person> _actorsList;
        public ObservableCollection<Person> ActorsList
        {
            get { return _actorsList; }
            set { SetProperty(ref _actorsList, value); }
        }

        public string InteractionResultMessage { get; private set; }

        #endregion

        #region StringConstants

        private const string _readMore = "Подробнее";
        public string ReadMore
        { get { return _readMore; } }

        private const string _forExceptions = "ActorListViewModel";
        private const string _exceededNumberRequests = "Превышено число запросов к серверу";
        private const string _error = "Ошибка";
        private const string _errorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";
        private const string _userNotified = "Пользователь был оповещен";
        private const string selectedActors = "Избранные актеры";
        private const string searchingResults = "Результаты поиска";

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
                    Title = selectedActors;                    
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    await GetSearchedActors(name);
                    Title = searchingResults;
                }                  
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
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async Task GetSearchedActors(string name)
        {
            try
            {
                List<Person> actorsTest = await Data.GetActorsByName(name);
                ActorsList = new ObservableCollection<Person>(actorsTest);                           
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

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

        private void ShowDirectActor()
        {
            try
            {
                var parameters = new NavigationParameters { { "id", SelectedSearchedActor.Id } };
                _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private async void GetFavoriteActors()
        {
            try
            {
                IEnumerable<ActorDTO> favoriteActorsFromDb = ActorService.GetActors();
                List<int> actorsId = new List<int>();
                foreach (var item in favoriteActorsFromDb)
                {
                    actorsId.Add(item.ExternalId);
                }
                List<Person> favoriteActorsFromSite = new List<Person>();
                foreach (var item in actorsId)
                {
                    Person actor = await Data.GetDirectActorData(item);
                    favoriteActorsFromSite.Add(actor);
                }
                ActorsList = new ObservableCollection<Person>(favoriteActorsFromSite);
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        #endregion
    }
}