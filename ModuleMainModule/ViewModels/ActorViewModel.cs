using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using System.Threading.Tasks;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    public class ActorViewModel : BindableBase, INavigationAware
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandShowDirectMovie { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        static readonly IActorService ActorService = new ActorService();

        public ActorViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand(NavigateShowDirectMovie);
            NavigateCommandAddToDb = new DelegateCommand(AddToDb);
        }

        #region Constants

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites
        {
            get { return _addFavorites; }
        }

        private const string _homePage = "Домашняя страница";
        public string ActorHomePage
        {
            get { return _homePage; }
        }

        private const string _aboutActor = "Об актере";
        public string AboutActor
        {
            get { return _aboutActor; }
        }

        private const string _biography = "Биография";
        public string Biography
        {
            get { return _biography; }
        }

        private const string _birthday = "Дата рождения";
        public string ActorBirthday
        {
            get { return _birthday; }
        }

        private const string _filmography = "Фильмография";
        public string Filmography
        {
            get { return _filmography; }
        }

        private const string _birthPlace = "Место рождения";
        public string ActorBirthPlace
        {
            get { return _birthPlace; }
        }

        #endregion

        #region Propertises

        private Person _direcctActor;
        public Person DirectActor
        {
            get { return _direcctActor; }
            set { SetProperty(ref _direcctActor, value); }
        }

        private PersonCredit _selectedActorMovie;
        public PersonCredit SelectedActorMovie
        {
            get { return _selectedActorMovie; }
            set { SetProperty(ref _selectedActorMovie, value); }
        }

        private ObservableCollection<PersonCredit> _actorMovies;

        public ObservableCollection<PersonCredit> ActorMovies
        {
            get { return _actorMovies; }
            set { SetProperty(ref _actorMovies, value); }
        }

        #endregion

        #region Methods

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var type = (int)navigationContext.Parameters["id"];
            await GetDirectActorInfo(type);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async Task GetDirectActorInfo(int id)
        {
            var actor = await Data.GetDirectActorData(id);
            List<PersonCredit> movies = await Data.GetDirectActorMoviesList(id);
            DirectActor = actor;
            ActorMovies = new ObservableCollection<PersonCredit>(movies);
        }

        private void NavigateShowDirectMovie()
        {
            var parameters = new NavigationParameters { { "id", SelectedActorMovie.Id } };
            _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
        }

        private void AddToDb()
        {
            ActorDTO actor = new ActorDTO() { Name = DirectActor.Name, Id = DirectActor.Id};
            IEnumerable<ActorDTO> actors = ActorService.GetActors();
            ActorService.TakeActor(actor);
            IEnumerable<ActorDTO> actorsPast = ActorService.GetActors();
        }

        #endregion
    }
}