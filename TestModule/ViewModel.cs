using System;
using System.Windows;
using System.Windows.Threading;
using ModuleMainModule.Services;
using NLog;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
#pragma warning disable 618

namespace TestModule
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly Logger _logger;

        public DelegateCommand<string> NavigateCommandMain { get; private set; }
        public DelegateCommand<string> NavigateCommandListShow { get; private set; }
        public DelegateCommand<string> NavigateCommandListMovie { get; private set; }
        public DelegateCommand<string> NavigateCommandListActor { get; private set; }

        public DelegateCommand ApplicationCommandMinimize { get; private set; }
        public DelegateCommand ApplicationCommandMaximaze { get; private set; }
        public DelegateCommand ApplicationCommandClose { get; private set; }

        public InteractionRequest<INotification> NotificationRequestConnection { get; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _logger = LogManager.GetCurrentClassLogger();

            NavigateCommandMain = new DelegateCommand<string>(NavigateMain);
            NavigateCommandListShow = new DelegateCommand<string>(NavigateListShow);
            NavigateCommandListMovie = new DelegateCommand<string>(NavigateListMovie);
            NavigateCommandListActor = new DelegateCommand<string>(NavigateListActor);

            ApplicationCommandMinimize = new DelegateCommand(Minimize);
            ApplicationCommandMaximaze = new DelegateCommand(Maximize);
            ApplicationCommandClose = new DelegateCommand(Close);

            NotificationRequestConnection = new InteractionRequest<INotification>();
            Timer();
        }

        public string InteractionResultMessage { get; private set; }

        #region Constants

        private const string _mainView = "Главная";
        public string MainView => _mainView;

        private const string _movies = "Фильмы";
        public string Movies => _movies;

        private const string _title = "Movie for You";
        public string Title => _title;

        private const string _shows = "Сериалы";
        public string Shows => _shows;

        private const string _search = "Поиск";
        public string Search => _search;

        private const string _help = "Справка";
        public string Help => _help;

        private const string _best = "Лучшие";
        public string Best => _best;

        private const string _popular = "Популярные";
        public string Popular => _popular;

        private const string _upComing = "Скоро в кино";
        public string UpComing => _upComing;

        private const string _nowPlaying = "Сейчас в кино";
        public string NowPlaying => _nowPlaying;

        private const string _nowOnTV = "Сейчас на ТВ";
        public string NowOnTv => _nowOnTV;

        private const string _searchMovie = "Найти фильм";
        public string SearchMovie => _searchMovie;

        private const string _searchShow = "Найти сериал";
        public string SearchShow => _searchShow;

        private const string _searchActor = "Найти актера";
        public string SearchActor => _searchActor;

        private const string _aboutDeveloper = "О разработчике";
        public string AboutDeveloper => _aboutDeveloper;

        private const string _aboutProgram = "О программе";
        public string AboutProgram => _aboutProgram;

        private const string _information = "Информация";
        public string Information => _information;

        private const string _selected = "Избранное";
        public string Selected => _selected;

        private const string _selectedMovies = "Избранные фильмы";
        public string SelectedMovies => _selectedMovies;

        private const string _selectedShows = "Избранные сериалы";
        public string SelectedShows => _selectedShows;

        private const string _selectedActors = "Избранные актеры";
        public string SelectedActors => _selectedActors;

        private const string _popularPath = "/Images/popular.png";
        public string PopularPath => _popularPath;

        private const string _bestPath = "/Images/best.png";
        public string BestPath => _bestPath;

        private const string _nowMoviePath = "/Images/oncinema.png";
        public string NowMoviePath => _nowMoviePath;

        private const string _nowShowPath = "/Images/ontv.png";
        public string NowShowPath => _nowShowPath;

        private const string _futureMoviePath = "/Images/future.png";
        public string FutureMoviePath => _futureMoviePath;

        private const string _mainPagePath = "/Images/icon.jpg";
        public string MainPagePath => _mainPagePath;

        private const string _searchMoviePath = "/Images/MovieSearch.png";
        public string SearchMoviePath => _searchMoviePath;

        private const string _searchShowPath = "/Images/ShowSearch.png";
        public string SearchShowPath => _searchShowPath;

        private const string _searchActorPath = "/Images/ActorSearch.png";
        public string SearchActorPath => _searchActorPath;

        private const string _favoriteMoviePath = "/Images/MovieFavorite.png";
        public string FavoriteMoviePath => _favoriteMoviePath;

        private const string _favoriteShowPath = "/Images/ShowFavorite.png";
        public string FavoriteShowPath => _favoriteShowPath;

        private const string _favoritActorPath = "/Images/ActorFavorite.png";
        public string FavoritActorPath => _favoritActorPath;

        private const string _mainRegionBackgroundPath = "/Images/ListBackground.png";
        public string MainRegionBackgroundPath => _mainRegionBackgroundPath;

        private const string _listRegionBackgroundPath = "/Images/BackgroundMain.jpg";
        public string ListRegionBackgroundPath => _listRegionBackgroundPath;

        private const string ForExceptions = "ShellViewModel";
        private const string WarningError = "Ошибка";
        private const string ConnectionProblems = "Проблемы с доступом к сайту. Проверьте, возможно потеряно интернет соединение";
        private const string UserNotified = "Пользователь был оповещен";

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
                _logger.ErrorException(ForExceptions, e);
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
                _logger.ErrorException(ForExceptions, e);
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
                _logger.ErrorException(ForExceptions, e);
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
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private void CheckConnection(object sender, EventArgs e)
        {
            try
            {
                bool internetConnection = NetworkClient.CheckForInternetConnection();
                if (internetConnection == false)
                {
                    RaiseNotificationConnection();
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ForExceptions, ex);
            }
        }

        private void RaiseNotificationConnection()
        {
            NotificationRequestConnection.Raise(
               new Notification { Content = ConnectionProblems, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void Timer()
        {
            try
            {
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += CheckConnection;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
                dispatcherTimer.Start();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Maximize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void Close()
        {
            Application.Current.MainWindow.Close();
        }

        #endregion
    }
}