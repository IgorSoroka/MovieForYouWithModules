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
    class MovieViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandShowDirectActor { get; private set; }

        public MovieViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand<int?>(NavigateShowDirectActor);
        }

        private void NavigateShowDirectActor(int? id)
        {
            //var parameters = new NavigationParameters();
            //parameters.Add("id", id);

            var parameters = new NavigationParameters();
            parameters.Add("id", SelectedActor.Id);

            _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
        }

        private Movie _direcctMovie;
        public Movie DirectMovie
        {
            get { return _direcctMovie; }
            set { SetProperty(ref _direcctMovie, value); }
        }

        private MediaCast _selectedActor;
        public MediaCast SelectedActor
        {
            get { return _selectedActor; }
            set { SetProperty(ref _selectedActor, value); }
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var type = (int)navigationContext.Parameters["id"];
            GetDirectMovieInfo(type);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async void GetDirectMovieInfo(int id)
        {
            Movie movie = await Data.GetDirectMoveData(id);
            List<MediaCrew> crews = (movie.Credits.Crew).Take(20).ToList<MediaCrew>();
            List<MediaCast> casts = (movie.Credits.Cast).Take(10).ToList<MediaCast>();

            DirectMovie = movie;
            Crew = new ObservableCollection<MediaCrew>(crews);
            Cast = new ObservableCollection<MediaCast>(casts);
        }
    }
}
