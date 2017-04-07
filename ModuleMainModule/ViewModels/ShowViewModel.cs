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
    class ShowViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandShowDirectActor { get; private set; }

        public ShowViewModel(RegionManager regionManager)
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
            GetDirectShowInfo(type);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async void GetDirectShowInfo(int id)
        {
            Show show = await Data.GetDirectShowData(id);
            List<MediaCrew> crews = (show.Credits.Crew).Take(20).ToList<MediaCrew>();
            List<MediaCast> casts = (show.Credits.Cast).Take(10).ToList<MediaCast>();

            DirectShow = show;
            Crew = new ObservableCollection<MediaCrew>(crews);
            Cast = new ObservableCollection<MediaCast>(casts);
        }
    }
}