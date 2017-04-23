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
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        Logger logger = LogManager.GetCurrentClassLogger();
        static readonly IActorService ActorService = new ActorService();

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

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var type = navigationContext.Parameters["type"] as string;
                if (type != null)
                {     
                    GetFavoriteActors();
                    Title = "Избранные актеры";                    
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    await GetSearchedActors(name);
                    Title = "Результаты поиска";
                }                  
            }
            catch (NullReferenceException ex)
            {
                RaiseNotificationNull();
                logger.ErrorException("ActorListViewModel", ex);
            }
            catch (Exception e)
            {
                logger.ErrorException("ActorListViewModel", e);
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
                //if(actorsTest == null)
                //{
                //    NotFound = "not null";
                //} 
                //else
                //{
                //    NotFound = null;
                //}               
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ActorListViewModel", e);
            }
        }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз.", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
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
                logger.ErrorException("ActorListViewModel", e);
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
                logger.ErrorException("ActorListViewModel", e);
            }
        }
    }
}