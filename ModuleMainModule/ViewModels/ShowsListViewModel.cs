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

       private ObservableCollection<Show> _shows;
        public ObservableCollection<Show> Shows
        {
            get { return _shows; }
            set { SetProperty(ref _shows, value); }
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

            var name = navigationContext.Parameters["name"] as string;
            if (name != null)
            {
                GetShowsByName(name);
            }

            int selectedYear = (int)navigationContext.Parameters["SelectedYear"];
            int selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
            int selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
            decimal selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

            if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
            {
                GetShowsByOnlyRating(selectedRating);
            }
            else if (selectedYear != 0)
            {
                GetShowsByYearAndRating(selectedYear, selectedRating);
            }
            else if (selectedFirstYear != 0 && selectedLastYear != 0)
            {
                GetShowsByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
            }
            else if (selectedFirstYear == 0 || selectedLastYear == 0)
            {
                if (selectedFirstYear != 0 && selectedLastYear == 0)
                {
                    GetShowsByFirstYearAndRating(selectedFirstYear, selectedRating);
                }
                else if (selectedFirstYear == 0 && selectedLastYear != 0)
                {
                    GetShowsByLastYearAndRating(selectedLastYear, selectedRating);
                }
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        #region Methods

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

        private async void GetShowsByLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShowsLastYear(selectedLastYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetShowsByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShowsFirstYear(selectedFirstYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetShowsByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShows(selectedFirstYear, selectedLastYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetShowsByYearAndRating(int selectedYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShows(selectedYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetShowsByOnlyRating(decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShows(selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetShowsByName(string name)
        {
            List<Show> showsTest = await Data.GetShowsByName(name);
            Shows = new ObservableCollection<Show>(showsTest);
        }
        
        #endregion
    }
}