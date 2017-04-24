using ModuleMainModule.Model;
using NLog;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace TestModule
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand<string> NavigateCommandMain { get; private set; }
        public DelegateCommand<string> NavigateCommandListShow { get; private set; }
        public DelegateCommand<string> NavigateCommandListMovie { get; private set; }
        public DelegateCommand<string> NavigateCommandListActor { get; private set; }
        public InteractionRequest<INotification> NotificationRequestConnection { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandMain = new DelegateCommand<string>(NavigateMain);
            NavigateCommandListShow = new DelegateCommand<string>(NavigateListShow);
            NavigateCommandListMovie = new DelegateCommand<string>(NavigateListMovie);
            NavigateCommandListActor = new DelegateCommand<string>(NavigateListActor);
            NotificationRequestConnection = new InteractionRequest<INotification>();
            Timer();
        }

        public string InteractionResultMessage { get; private set; }

        #region Constants

        private const string _mainView = "Главная";
        public string MainView
        {  get { return _mainView; }   }

        private const string _movies = "Фильмы";
        public string Movies
        {  get { return _movies; }   }

        private const string _shows = "Сериалы";
        public string Shows
        {  get { return _shows; }   }

        private const string _search = "Поиск";
        public string Search
        {  get { return _search; }  }

        private const string _help = "Справка";
        public string Help
        {   get { return _help; }  }

        private const string _best = "Лучшие";
        public string Best
        {   get { return _best; }  }

        private const string _popular = "Популярные";
        public string Popular
        {   get { return _popular; }  }

        private const string _upComing = "Скоро в кино";
        public string UpComing
        {   get { return _upComing; }  }

        private const string _nowPlaying = "Сейчас в кино";
        public string NowPlaying
        {   get { return _nowPlaying; }  }

        private const string _nowOnTV = "Сейчас на ТВ";
        public string NowOnTV
        {   get { return _nowOnTV; }   }

        private const string _searchMovie = "Найти фильм";
        public string SearchMovie
        {   get { return _searchMovie; }  }

        private const string _searchShow = "Найти сериал";
        public string SearchShow
        {   get { return _searchShow; }   }

        private const string _searchActor = "Найти актера";
        public string SearchActor
        {   get { return _searchActor; }   }

        private const string _aboutDeveloper = "О разработчике";
        public string AboutDeveloper
        {   get { return _aboutDeveloper; }   }

        private const string _aboutProgram = "О программе";
        public string AboutProgram
        {   get { return _aboutProgram; }  }

        private const string _information = "Информация";
        public string Information
        {   get { return _information; }  }

        private const string _selected = "Избранное";
        public string Selected
        { get { return _selected; } }

        private const string _selectedMovies = "Избранные фильмы";
        public string SelectedMovies
        { get { return _selectedMovies; } }

        private const string _selectedShows = "Избранные сериалы";
        public string SelectedShows
        { get { return _selectedShows; } }

        private const string _selectedActors = "Избранные актеры";
        public string SelectedActors
        { get { return _selectedActors; } }      

        private const string _popularPath = "/Images/popular.png";
        public string PopularPath
        { get { return _popularPath; } }

        private const string _bestPath = "/Images/best.png";
        public string BestPath
        { get { return _bestPath; } }

        private const string _nowMoviePath = "/Images/oncinema.png";
        public string NowMoviePath
        { get { return _nowMoviePath; } }

        private const string _nowShowPath = "/Images/ontv.png";
        public string NowShowPath
        { get { return _nowShowPath; } }

        private const string _futureMoviePath = "/Images/future.png";
        public string FutureMoviePath
        { get { return _futureMoviePath; } }

        private const string _mainPagePath = "/Images/icon.jpg";
        public string MainPagePath
        { get { return _mainPagePath; } }

        private const string _searchMoviePath = "/Images/MovieSearch.jpg";
        public string SearchMoviePath
        { get { return _searchMoviePath; } }

        private const string _searchShowPath = "/Images/ShowSearch.jpg";
        public string SearchShowPath
        { get { return _searchShowPath; } }

        private const string _searchActorPath = "/Images/ActorSearch.jpg";
        public string SearchActorPath
        { get { return _searchActorPath; } }

        private const string _favoriteMoviePath = "/Images/MovieSearch.jpg";
        public string FavoriteMoviePath
        { get { return _favoriteMoviePath; } }

        private const string _favoriteShowPath = "/Images/ShowSearch.jpg";
        public string FavoriteShowPath
        { get { return _favoriteShowPath; } }

        private const string _favoritActorPath = "/Images/ActorSearch.jpg";
        public string FavoritActorPath
        { get { return _favoritActorPath; } }

        private const string _mainRegionBackgroundPath = "/Images/BackgroundList.jpg";
        public string MainRegionBackgroundPath
        { get { return _mainRegionBackgroundPath; } }

        private const string _listRegionBackgroundPath = "/Images/BackgroundMain.jpg";
        public string ListRegionBackgroundPath
        { get { return _listRegionBackgroundPath; } }

        private const string _forExceptions = "ShellViewModel";
        private const string _error = "Ошибка";
        private const string _connectionProblems = "Проблемы с доступом к сайту. Проверьте, возможно потеряно интернет соединение";
        private const string _userNotified = "Пользователь был оповещен";

        #endregion

        #region Methods

        private void NavigateMain(string navigatePath)
        {
            try
            {
                if (navigatePath != null)
                { _regionManager.RequestNavigate("MainRegion", navigatePath); }
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void NavigateListShow(string type)
        {
            try
            {
                var parameters = new NavigationParameters { { "type", type } };
                if (type != null)
                { _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters); }
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void NavigateListMovie(string type)
        {
            try
            {
                var parameters = new NavigationParameters { { "type", type } };
                if (type != null)
                { _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters); }
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void NavigateListActor(string type)
        {
            try
            {
                var parameters = new NavigationParameters { { "type", type } };
                if (type != null)
                { _regionManager.RequestNavigate("ListRegion", "ActorsList", parameters); }
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void checkConnection(object sender, EventArgs e)
        {
            bool internetConnection = NetworkClient.CheckForInternetConnection();
            if (internetConnection == false)
            {
                RaiseNotificationConnection();
            }
        }

        private void RaiseNotificationConnection()
        {
            this.NotificationRequestConnection.Raise(
               new Notification { Content = _connectionProblems, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private void Timer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(checkConnection);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
        }

        #endregion
    }
}