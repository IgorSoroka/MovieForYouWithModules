using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using NLog;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
#pragma warning disable 618

namespace ModuleMainModule.ViewModels
{
    class ShowsListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly TheMovieDBDataService _dataService;
        private readonly Logger _logger;
        private readonly IShowService _showService;

        private bool _best;
        private bool _popular;
        private bool _now;

        #region Constants

        private const string _next = "Следуюшая";
        public string Next => _next;

        private const string _privious = "Предыдущая";
        public string Privious => _privious;

        private const string _readMore = "Подробнее";
        public string ReadMore => _readMore;

        private const string _loadingData = "Загрузка данных...";
        public string LoadingData => _loadingData;

        private const string SelectedShows = "Избранные сериалы";
        private const string SearchingResults = "Результаты поиска";
        private const string ForExceptions = "ShowListViewModel";
        private const string ExceededNumberRequests = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string UserNotified = "Пользователь был оповещен";
        private const string ErrorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";
        private const string BestShows = "Лучшие сериалы";
        private const string PopularShows = "Популярные сериалы";
        private const string OnTvShows = "Сейчас на ТВ";

        private const int MinPage = 1;
        private const int MaxPage = 5;

        #endregion

        public DelegateCommand NavigateCommandShowDirectShow { get; private set; }
        public DelegateCommand NavigateCommandShowNextPage { get; private set; }
        public DelegateCommand NavigateCommandShowPriviousPage { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; }
        public InteractionRequest<INotification> NotificationRequestNull { get; }

        public ShowsListViewModel(RegionManager regionManager, TheMovieDBDataService dataService, ShowService showService)
        {
            _regionManager = regionManager;
            _dataService = dataService;
            _showService = showService;
            _logger = LogManager.GetCurrentClassLogger();

            NavigateCommandShowDirectShow = new DelegateCommand(NavigateShowDirectShow);
            NavigateCommandShowNextPage = new DelegateCommand(ShowNextPage);
            NavigateCommandShowPriviousPage = new DelegateCommand(ShowPriviousPage);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
        }

        #region Properties

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

        private bool _busyIndicator;
        public bool BusyIndicatorValue
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }

        public string InteractionResultMessage { get; private set; }

        #endregion

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
                        Title = BestShows;
                    }
                    if (type == "Popular")
                    {
                        _best = false;
                        _popular = true;
                        _now = false;
                        GetPopularShows(Page);
                        Title = PopularShows;
                    }
                    if (type == "Now")
                    {
                        _best = false;
                        _popular = false;
                        _now = true;
                        GetNowPlayingShows(Page);
                        Title = OnTvShows;
                    }
                    if (type == "Favorite")
                    {
                        _best = false;
                        _popular = false;
                        _now = false;
                        GetFavoriteShows();
                        Title = SelectedShows;
                    }
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    GetShowsByName(name);
                    Title = SearchingResults;
                }
                           
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                {
                    GetShowsByOnlyRating(selectedRating);
                    Title = SearchingResults;
                }
                else if (selectedYear != 0)
                {
                    GetShowsByYearAndRating(selectedYear, selectedRating);
                    Title = SearchingResults;
                }
                else if (selectedFirstYear != 0 && selectedLastYear != 0)
                {
                    GetShowsByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
                    Title = SearchingResults;
                }
                else if (selectedFirstYear == 0 || selectedLastYear == 0)
                {
                    if (selectedFirstYear != 0 && selectedLastYear == 0)
                    {
                        GetShowsByFirstYearAndRating(selectedFirstYear, selectedRating);
                        Title = SearchingResults;
                    }
                    else if (selectedFirstYear == 0 && selectedLastYear != 0)
                    {
                        GetShowsByLastYearAndRating(selectedLastYear, selectedRating);
                        Title = SearchingResults;
                    }
                }
            }
            catch (NullReferenceException)
            {
                //RaiseNotificationNull();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotification()
        {
            NotificationRequest.Raise(
               new Notification { Content = ExceededNumberRequests, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void RaiseNotificationNull()
        {
            NotificationRequestNull.Raise(
               new Notification { Content = ErrorLoadingData, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }
        
        private void ShowPriviousPage()
        {
            if (_popular && Page > MinPage)
            {
                Page--;
                GetPopularShows(Page);
            }
            if (_best && Page > MinPage)
            {
                Page--;
                GetBestShows(Page);
            }
            if (_now && Page > MinPage)
            {
                Page--;
                GetNowPlayingShows(Page);
            }
        }

        private void ShowNextPage()
        {

            if (_popular && Page < MaxPage)
            {
                Page++;
                GetPopularShows(Page);
            }
            if (_best && Page < MaxPage)
            {
                Page++;
                GetBestShows(Page);
            }            
            if (_now && Page < MaxPage)
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
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetPopularShows(int page)
        {            
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetPopularShowsData(page);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetBestShows(int page)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetTopRatedShowsData(page);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetNowPlayingShows(int page)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetNowShowsData(page);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetShowsByLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetSearchedShowsLastYear(selectedLastYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {            
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetShowsByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetSearchedShowsFirstYear(selectedFirstYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetShowsByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetSearchedShows(selectedFirstYear, selectedLastYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {              
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetShowsByYearAndRating(int selectedYear, decimal selectedRating)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetSearchedShows(selectedYear, selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {               
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetShowsByOnlyRating(decimal selectedRating)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetSearchedShows(selectedRating);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {              
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private async void GetShowsByName(string name)
        {
            try
            {
                BusyIndicatorValue = true;
                List<Show> showsTest = await _dataService.GetShowsByName(name);
                Shows = new ObservableCollection<Show>(showsTest);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {             
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }
        /// <summary>
        ///  Метод получения списка избранных сериалов из базы даннных
        /// </summary>
        private async void GetFavoriteShows()
        {
            try
            {
                BusyIndicatorValue = true;
                IEnumerable<ShowDTO> favoriteShowsFromDb = _showService.GetShows();
                List<int> showsId = new List<int>();
                foreach (var item in favoriteShowsFromDb)
                {
                    showsId.Add(item.ExternalId);
                }
                List<Show> favoriteShowsFromSite = new List<Show>();
                foreach (var item in showsId)
                {
                    Show show = await _dataService.GetDirectShowData(item);
                    favoriteShowsFromSite.Add(show);
                }               
                Shows = new ObservableCollection<Show>(favoriteShowsFromSite);
                BusyIndicatorValue = false;
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        #endregion
    }
}