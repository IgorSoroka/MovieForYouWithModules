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
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandShowDirectShow { get; private set; }

        public ShowsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectShow = new DelegateCommand(NavigateShowDirectShow);
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

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #region Methods

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var type = navigationContext.Parameters["type"] as string;
            if (type != null)
            {
                if (type == "Best")
                {
                    GetBestShows();
                    Title = "Лучшие сериалы";
                }
                if (type == "Popular")
                {
                    GetPopularShows();
                    Title = "Популярные сериалы";
                }
                if (type == "Now")
                {
                    GetNowPlayingShows();
                    Title = "Сейчас на ТВ";
                }
            }

            var name = navigationContext.Parameters["name"] as string;
            if (name != null)
            { GetShowsByName(name); }

            var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
            var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
            var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
            var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

            if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
            { GetShowsByOnlyRating(selectedRating); }
            else if (selectedYear != 0)
            { GetShowsByYearAndRating(selectedYear, selectedRating); }
            else if (selectedFirstYear != 0 && selectedLastYear != 0)
            { GetShowsByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating); }
            else if (selectedFirstYear == 0 || selectedLastYear == 0)
            { 
                if (selectedFirstYear != 0 && selectedLastYear == 0)
                { GetShowsByFirstYearAndRating(selectedFirstYear, selectedRating); }
                else if (selectedFirstYear == 0 && selectedLastYear != 0)
                { GetShowsByLastYearAndRating(selectedLastYear, selectedRating); }
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void NavigateShowDirectShow()
        {
            var parameters = new NavigationParameters { { "id", SelectedShow.Id } };
            _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
        }

        private async void GetPopularShows()
        {
            List<Show> showsTest = await Data.GetPopularShowsData();
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetBestShows()
        {
            List<Show> showsTest = await Data.GetTopRatedShowsData();
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async void GetNowPlayingShows()
        {
            List<Show> showsTest = await Data.GetNowShowsData();
            Shows = new ObservableCollection<Show>(showsTest);
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