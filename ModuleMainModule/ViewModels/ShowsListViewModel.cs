using System;
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
using ModuleMainModule.Interfaces;
using ModuleMainModule.Services;
using ModuleMainModule.Model;

namespace ModuleMainModule.ViewModels
{
    class ShowsListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();
        static readonly IShowService ShowService = new ShowService();

        public DelegateCommand NavigateCommandShowDirectShow { get; private set; }
        public DelegateCommand NavigateCommandShowNextPage { get; private set; }
        public DelegateCommand NavigateCommandShowPriviousPage { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public InteractionRequest<INotification> NotificationRequestNull { get; private set; }

        private bool _best;
        private bool _popular;
        private bool _now;

        public ShowsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectShow = new DelegateCommand(NavigateShowDirectShow);
            NavigateCommandShowNextPage = new DelegateCommand(ShowNextPage, CanExecuteNextPage);
            NavigateCommandShowPriviousPage = new DelegateCommand(ShowPriviousPage, CanExecutePriviousPage);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
        }

        #region Constants

        private const string _next = "Следуюшая";
        public string Next
        {
            get { return _next; }
        }

        private const string _privious = "Предыдущая";
        public string Privious
        {
            get { return _privious; }
        }

        #endregion

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

        public string InteractionResultMessage { get; private set; }

        #region Methods

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Page = 1;

            try
            {
                var type = navigationContext.Parameters["type"] as string;
                if (type != null)
                {
                    if (type == "Best")
                    {
                        _best = true;
                        _popular = false;
                        _now = false;
                        GetBestShows(Page);
                        Title = "Лучшие сериалы";
                    }
                    if (type == "Popular")
                    {
                        _best = false;
                        _popular = true;
                        _now = false;
                        GetPopularShows(Page);
                        Title = "Популярные сериалы";
                    }
                    if (type == "Now")
                    {
                        _best = false;
                        _popular = false;
                        _now = true;
                        GetNowPlayingShows(Page);
                        Title = "Сейчас на ТВ";
                    }
                    if (type == "Favorite")
                    {
                        _best = false;
                        _popular = false;
                        _now = false;
                        GetFavoriteShows();
                        Title = "Избранные сериалы";
                    }
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    GetShowsByName(name);
                    Title = "Результаты поиска";
                }

           
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                {
                    GetShowsByOnlyRating(selectedRating);
                    Title = "Результаты поиска";
                }
                else if (selectedYear != 0)
                {
                    GetShowsByYearAndRating(selectedYear, selectedRating);
                    Title = "Результаты поиска";
                }
                else if (selectedFirstYear != 0 && selectedLastYear != 0)
                {
                    GetShowsByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
                    Title = "Результаты поиска";
                }
                else if (selectedFirstYear == 0 || selectedLastYear == 0)
                {
                    if (selectedFirstYear != 0 && selectedLastYear == 0)
                    {
                        GetShowsByFirstYearAndRating(selectedFirstYear, selectedRating);
                        Title = "Результаты поиска";
                    }
                    else if (selectedFirstYear == 0 && selectedLastYear != 0)
                    {
                        GetShowsByLastYearAndRating(selectedLastYear, selectedRating);
                        Title = "Результаты поиска";
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                //RaiseNotificationNull();
                logger.ErrorException("ShowListViewModel", ex);
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз.", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

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

        private void ShowPriviousPage()
        {
            if (_popular && Page > 1)
            {
                Page--;
                GetPopularShows(Page);
            }

            if (_best && Page > 1)
            {
                Page--;
                GetBestShows(Page);
            }

            if (_now && Page > 1)
            {
                Page--;
                GetNowPlayingShows(Page);
            }
        }

        private void ShowNextPage()
        {

            if (_popular && Page < 5)
            {
                Page++;
                GetPopularShows(Page);
            }

            if (_best && Page < 5)
            {
                Page++;
                GetBestShows(Page);
            }
            
            if (_now && Page < 5)
            {
                Page++;
                GetNowPlayingShows(Page);
            }
        }


        private void NavigateShowDirectShow()
        {
            try
            {
                var parameters = new NavigationParameters { { "id", SelectedShow.Id } };
                _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetPopularShows(int page)
        {            
            try
            {
                List<Show> showsTest = await Data.GetPopularShowsData(page);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetBestShows(int page)
        {
            try
            {
                List<Show> showsTest = await Data.GetTopRatedShowsData(page);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetNowPlayingShows(int page)
        {
            try
            {
                List<Show> showsTest = await Data.GetNowShowsData(page);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetShowsByLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Show> showsTest = await Data.GetSearchedShowsLastYear(selectedLastYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {            
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetShowsByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            try
            {
                List<Show> showsTest = await Data.GetSearchedShowsFirstYear(selectedFirstYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetShowsByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            try
            {
                List<Show> showsTest = await Data.GetSearchedShows(selectedFirstYear, selectedLastYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {              
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetShowsByYearAndRating(int selectedYear, decimal selectedRating)
        {
            try
            {
                List<Show> showsTest = await Data.GetSearchedShows(selectedYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {               
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetShowsByOnlyRating(decimal selectedRating)
        {
            try
            {
                List<Show> showsTest = await Data.GetSearchedShows(selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {              
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetShowsByName(string name)
        {
            try
            {
                List<Show> showsTest = await Data.GetShowsByName(name);
                Shows = new ObservableCollection<Show>(showsTest);
            }
            catch (ServiceRequestException)
            {             
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        private async void GetFavoriteShows()
        {
            try
            {
                IEnumerable<ShowDTO> favoriteShowsFromDb = ShowService.GetShows();
                List<int> showsId = new List<int>();
                foreach (var item in favoriteShowsFromDb)
                {
                    showsId.Add(item.ExternalId);
                }
                List<Show> favoriteShowsFromSite = new List<Show>();
                foreach (var item in showsId)
                {
                    Show show = await Data.GetDirectShowData(item);
                    favoriteShowsFromSite.Add(show);
                }               
                Shows = new ObservableCollection<Show>(favoriteShowsFromSite);
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("ShowListViewModel", e);
            }
        }

        #endregion
    }
}