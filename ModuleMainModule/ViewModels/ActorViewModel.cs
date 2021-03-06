﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using System.Windows.Controls;
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
    public class ActorViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly TheMovieDBDataService _dataService;
        private readonly IActorService _actorService;
        private readonly Logger _logger;

        #region StringConstants

        private const string _delFavorites = "Удалить из избранного";
        public string DelFavorites => _delFavorites;

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites => _addFavorites;

        private const string _homePage = "Домашняя страница";
        public string ActorHomePage => _homePage;

        private const string _aboutActor = "Об актере";
        public string AboutActor => _aboutActor;

        private const string _biography = "Биография";
        public string Biography => _biography;

        private const string _readMore = "Подробнее";
        public string ReadMore => _readMore;

        private const string _birthday = "Дата рождения";
        public string ActorBirthday => _birthday;

        private const string _filmography = "Фильмография";
        public string Filmography => _filmography;

        private const string _birthPlace = "Место рождения";
        public string ActorBirthPlace => _birthPlace;

        private const string _loadingData = "Загрузка данных...";
        public string LoadingData => _loadingData;

        private const string ForExceptions = "ActorViewModel";
        private const string ExceededNumberRequests = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string UserNotified = "Пользователь был оповещен";

        #endregion

        public DelegateCommand NavigateCommandShowDirectMovie { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        public DelegateCommand NavigateCommandDellFromDb { get; private set; }    
        public InteractionRequest<INotification> NotificationRequest { get; }

        public ActorViewModel(RegionManager regionManager, TheMovieDBDataService dataService, ActorService actorService)
        {
            _regionManager = regionManager;
            _dataService = dataService;
            _actorService = actorService;
            _logger = LogManager.GetCurrentClassLogger();

            NavigateCommandShowDirectMovie = new DelegateCommand(NavigateShowDirectMovie);
            NavigateCommandAddToDb = new DelegateCommand(AddToDb);
            NavigateCommandDellFromDb = new DelegateCommand(DelFromDb);
            NotificationRequest = new InteractionRequest<INotification>();
        }

        #region Propertises

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

        private bool _canAddToDb;
        public bool CanAddToDb
        {
            get { return _canAddToDb; }
            set { SetProperty(ref _canAddToDb, value); }
        }

        private bool _canDelFromDb;
        public bool CanDelFromDb
        {
            get { return _canDelFromDb; }
            set { SetProperty(ref _canDelFromDb, value); }
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
            try
            {
                var type = (int)navigationContext.Parameters["id"];
                GetDirectActorInfo(type);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void RaiseNotification()
        {
            NotificationRequest.Raise(
               new Notification { Content = ExceededNumberRequests, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private async void GetDirectActorInfo(int id)
        {
            try
            {
                BusyIndicatorValue = true;
                var actor = await _dataService.GetDirectActorData(id);               
                List<PersonCredit> movies = await _dataService.GetDirectActorMoviesList(id);
                DirectActor = actor;
                ActorMovies = new ObservableCollection<PersonCredit>(movies);
                ActorDTO personFromDb = _actorService.GetActor(DirectActor.Id);
                if (personFromDb == null)
                {
                    CanDelFromDb = false;
                    CanAddToDb = true;
                }
                else
                {
                    CanDelFromDb = true;
                    CanAddToDb = false;
                }
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

        private void NavigateShowDirectMovie()
        {
            try
            {
                var parameters = new NavigationParameters { { "id", SelectedActorMovie.Id } };
                _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private void AddToDb()
        {
            ActorDTO actor = new ActorDTO { Name = DirectActor.Name, ExternalId = DirectActor.Id};           
            _actorService.TakeActor(actor);
            CanDelFromDb = true;
            CanAddToDb = false;
            RefreshFavoriteView();
        }

        private void DelFromDb()
        {            
            _actorService.DelActor(DirectActor.Id);
            CanDelFromDb = false;
            CanAddToDb = true;
            RefreshFavoriteView();
        }
        /// <summary>
        /// Метод для обновления списка избранных актеров при удалении/добавлении в избранное
        /// </summary>
        private void RefreshFavoriteView()
        {
            try
            {
                UserControl singleView = (UserControl)_regionManager.Regions["ListRegion"].ActiveViews.FirstOrDefault();
                ActorsListViewModel actorViewModel = (ActorsListViewModel)singleView.DataContext;

                if (actorViewModel.Title == "Избранные актеры")
                {
                    var parameters = new NavigationParameters { { "type", "Favorite" } };
                    _regionManager.RequestNavigate("ListRegion", "ActorsList", parameters);
                }
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