using System;
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
    class ShowsListViewModel : BindableBase, INavigationAware
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandShowDirectShow { get; private set; }
        public DelegateCommand NavigateCommandShowNextPage { get; private set; }
        public DelegateCommand NavigateCommandShowPriviousPage { get; private set; }

        private bool _best;
        private bool _popular;
        private bool _now;

        public ShowsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectShow = new DelegateCommand(NavigateShowDirectShow);
            NavigateCommandShowNextPage = new DelegateCommand(ShowNextPage, CanExecuteNextPage);
            NavigateCommandShowPriviousPage = new DelegateCommand(ShowPriviousPage, CanExecutePriviousPage);
        }

        private Show _selectedShow;
        public Show SelectedShow
        {
            get { return _selectedShow; }
            set { SetProperty(ref _selectedShow, value); }
        }

        private int _page;
        public int Page
        {
            get { return _page; }
            set { SetProperty(ref _page, value); }
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

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Page = 1;
            var type = navigationContext.Parameters["type"] as string;
            if (type != null)
            {
                if (type == "Best")
                {
                    _best = true;
                    _popular = false;
                    _now = false;
                    await GetBestShows(Page);
                    Title = "Лучшие сериалы";
                }
                if (type == "Popular")
                {
                    _best = false;
                    _popular = true;
                    _now = false;
                    await GetPopularShows(Page);
                    Title = "Популярные сериалы";
                }
                if (type == "Now")
                {
                    _best = false;
                    _popular = false;
                    _now = true;
                    await GetNowPlayingShows(Page);
                    Title = "Сейчас на ТВ";
                }
            }

            var name = navigationContext.Parameters["name"] as string;
            if (name != null)
            { await GetShowsByName(name); }

            try
            {
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                { await GetShowsByOnlyRating(selectedRating); }
                else if (selectedYear != 0)
                { await GetShowsByYearAndRating(selectedYear, selectedRating); }
                else if (selectedFirstYear != 0 && selectedLastYear != 0)
                { await GetShowsByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating); }
                else if (selectedFirstYear == 0 || selectedLastYear == 0)
                {
                    if (selectedFirstYear != 0 && selectedLastYear == 0)
                    { await GetShowsByFirstYearAndRating(selectedFirstYear, selectedRating); }
                    else if (selectedFirstYear == 0 && selectedLastYear != 0)
                    { await GetShowsByLastYearAndRating(selectedLastYear, selectedRating); }
                }
            }
            catch (NullReferenceException e)
            {
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private bool CanExecutePriviousPage()
        {
            if (Page == 1)
                return false;
            else
                return true;
        }

        private bool CanExecuteNextPage()
        {
            if (Page == 5)
                return false;
            else
                return true;
        }

        private async void ShowPriviousPage()
        {
            if (_popular && Page > 1)
            {
                Page--;
                await GetPopularShows(Page);
            }

            if (_best && Page > 1)
            {
                Page--;
                await GetBestShows(Page);
            }

            if (_now && Page > 1)
            {
                Page--;
                await GetNowPlayingShows(Page);
            }
        }

        private async void ShowNextPage()
        {

            if (_popular && Page < 5)
            {
                Page++;
                await GetPopularShows(Page);
            }

            if (_best && Page < 5)
            {
                Page++;
                await GetBestShows(Page);
            }
            
            if (_now && Page < 5)
            {
                Page++;
                await GetNowPlayingShows(Page);
            }
        }


        private void NavigateShowDirectShow()
        {
            var parameters = new NavigationParameters { { "id", SelectedShow.Id } };
            _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
        }

        private async Task GetPopularShows(int page)
        {
            List<Show> showsTest = await Data.GetPopularShowsData(page);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetBestShows(int page)
        {
            List<Show> showsTest = await Data.GetTopRatedShowsData(page);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetNowPlayingShows(int page)
        {
            List<Show> showsTest = await Data.GetNowShowsData(page);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetShowsByLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShowsLastYear(selectedLastYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetShowsByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShowsFirstYear(selectedFirstYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetShowsByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShows(selectedFirstYear, selectedLastYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetShowsByYearAndRating(int selectedYear, decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShows(selectedYear, selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetShowsByOnlyRating(decimal selectedRating)
        {
            List<Show> showsTest = await Data.GetSearchedShows(selectedRating);
            Shows = new ObservableCollection<Show>(showsTest);
        }

        private async Task GetShowsByName(string name)
        {
            List<Show> showsTest = await Data.GetShowsByName(name);
            Shows = new ObservableCollection<Show>(showsTest);
        }
        
        #endregion
    }
}