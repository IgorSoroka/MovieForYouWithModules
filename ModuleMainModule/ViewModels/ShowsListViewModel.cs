using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class ShowsListViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandShowDirectShow { get; private set; }

        public ShowsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectShow = new DelegateCommand<int?>(NavigateShowDirectShow);
        }

        private void NavigateShowDirectShow(int? id)
        {
            //var parameters = new NavigationParameters();
            //parameters.Add("id", id);

            var parameters = new NavigationParameters();
            parameters.Add("id", SelectedShow.Id);

            _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
        }

        private Show _selectedShow;
        public Show SelectedShow
        {
            get { return _selectedShow; }
            set { SetProperty(ref _selectedShow, value); }
        }

        private MediaCast _selectedActor;
        public MediaCast SelectedActor
        {
            get { return _selectedActor; }
            set { SetProperty(ref _selectedActor, value); }
        }

        private ObservableCollection<Show> _shows;
        public ObservableCollection<Show> Shows
        {
            get { return _shows; }
            set { SetProperty(ref _shows, value); }
        }

        private async void GetPopularShows()
        {
            List<Show> showsTest = await Data.GetPopularShowsData();
            Shows = new ObservableCollection<Show>(showsTest);
            //SelectedShow = null;
        }

        private async void GetBestShows()
        {
            List<Show> showsTest = await Data.GetTopRatedShowsData();
            Shows = new ObservableCollection<Show>(showsTest);
            //SelectedShow = null;
        }

        private async void GetNowPlayingShows()
        {
            List<Show> showsTest = await Data.GetNowShowsData();
            Shows = new ObservableCollection<Show>(showsTest);
            //SelectedShow = null;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var type = navigationContext.Parameters["type"] as string;
            if (type != null)
            {
                if (type == "Best")
                    GetBestShows();
                if (type == "Popular")
                    GetPopularShows();
                if (type == "Now")
                    GetNowPlayingShows();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {}
    }
}