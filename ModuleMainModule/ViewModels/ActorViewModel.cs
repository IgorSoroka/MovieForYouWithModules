using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NLog;
using Prism.Interactivity.InteractionRequest;
using System;

namespace ModuleMainModule.ViewModels
{
    public class ActorViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private static readonly GetData Data = new GetData();
        private static readonly IActorService ActorService = new ActorService();
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand NavigateCommandShowDirectMovie { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        public DelegateCommand NavigateCommandDellFromDb { get; private set; }    
        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public ActorViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand(NavigateShowDirectMovie);
            NavigateCommandAddToDb = new DelegateCommand(AddToDb);
            NavigateCommandDellFromDb = new DelegateCommand(DelFromDb);
            NotificationRequest = new InteractionRequest<INotification>();
        }

        #region StringConstants

        private const string _delFavorites = "Удалить из избранного";
        public string DelFavorites
        {   get { return _delFavorites; }  }

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites
        {   get { return _addFavorites; }  }        

        private const string _homePage = "Домашняя страница";
        public string ActorHomePage
        {   get { return _homePage; }  }

        private const string _aboutActor = "Об актере";
        public string AboutActor
        {   get { return _aboutActor; }   }

        private const string _biography = "Биография";
        public string Biography
        {   get { return _biography; }   }

        private const string _readMore = "Подробнее";
        public string ReadMore
        { get { return _readMore; } }

        private const string _birthday = "Дата рождения";
        public string ActorBirthday
        {   get { return _birthday; }   }

        private const string _filmography = "Фильмография";
        public string Filmography
        {   get { return _filmography; }  }

        private const string _birthPlace = "Место рождения";
        public string ActorBirthPlace
        {   get { return _birthPlace; }   }

        private const string _forExceptions = "ActorViewModel";
        private const string _exceededNumberRequests = "Превышено число запросов к серверу";
        private const string _error = "Ошибка";        
        private const string _userNotified = "Пользователь был оповещен";

        #endregion

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
                logger.ErrorException(_forExceptions, e);
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
            this.NotificationRequest.Raise(
               new Notification { Content = _exceededNumberRequests, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private async void GetDirectActorInfo(int id)
        {
            try
            {
                var actor = await Data.GetDirectActorData(id);               
                List<PersonCredit> movies = await Data.GetDirectActorMoviesList(id);
                DirectActor = actor;
                ActorMovies = new ObservableCollection<PersonCredit>(movies);
                ActorDTO personFromDb = ActorService.GetActor(DirectActor.Id);
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
            }
            catch (ServiceRequestException ex)
            {                
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
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
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void AddToDb()
        {
            ActorDTO actor = new ActorDTO() { Name = DirectActor.Name, ExternalId = DirectActor.Id};           
            ActorService.TakeActor(actor);
            CanDelFromDb = true;
            CanAddToDb = false;
        }

        private void DelFromDb()
        {            
            ActorService.DelActor(DirectActor.Id);
            CanDelFromDb = false;
            CanAddToDb = true;           
        }

        #endregion
    }
}