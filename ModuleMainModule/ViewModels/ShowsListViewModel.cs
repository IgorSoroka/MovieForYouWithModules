using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
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
        private readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly IShowService ShowService = new ShowService();

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
            NavigateCommandShowNextPage = new DelegateCommand(ShowNextPage);
            NavigateCommandShowPriviousPage = new DelegateCommand(ShowPriviousPage);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
        }

        #region Constants

        private const string _next = "Следуюшая";
        public string Next
        {   get { return _next; }   }

        private const string _privious = "Предыдущая";
        public string Privious
        {   get { return _privious; }   }

        private const string _readMore = "Подробнее";
        public string ReadMore
        { get { return _readMore; } }

        private const string selectedShows = "Избранные сериалы";
        private const string searchingResults = "Результаты поиска";
        private const string _forExceptions = "ShowListViewModel";
        private const string _exceededNumberRequests = "Превышено число запросов к серверу";
        private const string _error = "Ошибка";
        private const string _userNotified = "Пользователь был оповещен";
        private const string _errorLoadingData = "Произошла ошибка загрузки данных. Повторите Ваш запрос еще раз";
        private const string _bestShows = "Лучшие сериалы";
        private const string _popularShows = "Популярные сериалы";
        private const string _onTvShows = "Сейчас на ТВ";

        private const int _minPage = 1;
        private const int _maxPage = 5;

        #endregion

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
                        Title = _bestShows;
                    }
                    if (type == "Popular")
                    {
                        _best = false;
                        _popular = true;
                        _now = false;
                        GetPopularShows(Page);
                        Title = _popularShows;
                    }
                    if (type == "Now")
                    {
                        _best = false;
                        _popular = false;
                        _now = true;
                        GetNowPlayingShows(Page);
                        Title = _onTvShows;
                    }
                    if (type == "Favorite")
                    {
                        _best = false;
                        _popular = false;
                        _now = false;
                        GetFavoriteShows();
                        Title = selectedShows;
                    }
                }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                {
                    GetShowsByName(name);
                    Title = searchingResults;
                }
                           
                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                {
                    GetShowsByOnlyRating(selectedRating);
                    Title = searchingResults;
                }
                else if (selectedYear != 0)
                {
                    GetShowsByYearAndRating(selectedYear, selectedRating);
                    Title = searchingResults;
                }
                else if (selectedFirstYear != 0 && selectedLastYear != 0)
                {
                    GetShowsByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
                    Title = searchingResults;
                }
                else if (selectedFirstYear == 0 || selectedLastYear == 0)
                {
                    if (selectedFirstYear != 0 && selectedLastYear == 0)
                    {
                        GetShowsByFirstYearAndRating(selectedFirstYear, selectedRating);
                        Title = searchingResults;
                    }
                    else if (selectedFirstYear == 0 && selectedLastYear != 0)
                    {
                        GetShowsByLastYearAndRating(selectedLastYear, selectedRating);
                        Title = searchingResults;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                //RaiseNotificationNull();
                logger.ErrorException(_forExceptions, ex);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = _exceededNumberRequests, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = _errorLoadingData, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        //private bool CanExecutePriviousPage()
        //{
        //    if (Page == 1)
        //        return false;
        //    else
        //        return true;
        //}

        //private bool CanExecuteNextPage()
        //{
        //    if (Page == 5)
        //        return false;
        //    else
        //        return true;
        //}

        private void ShowPriviousPage()
        {
            if (_popular && Page > _minPage)
            {
                Page--;
                GetPopularShows(Page);
            }
            if (_best && Page > _minPage)
            {
                Page--;
                GetBestShows(Page);
            }
            if (_now && Page > _minPage)
            {
                Page--;
                GetNowPlayingShows(Page);
            }
        }

        private void ShowNextPage()
        {

            if (_popular && Page < _maxPage)
            {
                Page++;
                GetPopularShows(Page);
            }
            if (_best && Page < _maxPage)
            {
                Page++;
                GetBestShows(Page);
            }            
            if (_now && Page < _maxPage)
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
            }
        }

        #endregion
    }
}