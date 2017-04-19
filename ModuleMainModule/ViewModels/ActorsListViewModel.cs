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

namespace ModuleMainModule.ViewModels
{
    class ActorsListViewModel : BindableBase, INavigationAware
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public ActorsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(ShowDirectActor);
            NotificationRequest = new InteractionRequest<INotification>();
        }

        private Person _selectedSearchedActor;
        public Person SelectedSearchedActor
        {
            get { return _selectedSearchedActor; }
            set { SetProperty(ref _selectedSearchedActor, value); }
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
            var name = navigationContext.Parameters["name"] as string;
            if (name != null)
                await GetSearchedActors(name);
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
            catch (ServiceRequestException ex)
            {
                logger.ErrorException("MovieListViewModel", ex);
                RaiseNotification();
            }           
        }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void ShowDirectActor()
        {
            var parameters = new NavigationParameters { { "id", SelectedSearchedActor.Id } };
            _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
        }
    }
}