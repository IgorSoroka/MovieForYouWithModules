using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    public class ActorViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandShowDirectMovie { get; private set; }

        public ActorViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand<int?>(NavigateShowDirectMovie);
        }

        private void NavigateShowDirectMovie(int? id)
        {
            //var parameters = new NavigationParameters();
            //parameters.Add("id", id);

            var parameters = new NavigationParameters();
            parameters.Add("id", SelectedActorMovie.Id);

            _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
        }

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


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var type = (int)navigationContext.Parameters["id"];
            GetDirectActorInfo(type);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async void GetDirectActorInfo(int id)
        {
            Person actor = await Data.GetDirectActorData(id);
            List<PersonCredit> movies = await Data.GetDirectActorMoviesList(id);

            DirectActor = actor;
            ActorMovies = new ObservableCollection<PersonCredit>(movies);
        }
    }
}