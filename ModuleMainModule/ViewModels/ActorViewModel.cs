using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using System.Threading.Tasks;
using MainModule;
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

        public ActorViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand(NavigateShowDirectMovie);
        }

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

        #endregion
    }
}