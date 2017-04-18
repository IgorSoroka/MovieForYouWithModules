using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace TestModule
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        
        public DelegateCommand<string> NavigateCommandMain { get; private set; }
        public DelegateCommand<string> NavigateCommandListShow { get; private set; }
        public DelegateCommand<string> NavigateCommandListMovie { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandMain = new DelegateCommand<string>(NavigateMain);
            NavigateCommandListShow = new DelegateCommand<string>(NavigateListShow);
            NavigateCommandListMovie = new DelegateCommand<string>(NavigateListMovie);
        }

        private void NavigateMain(string navigatePath)
        {
            if (navigatePath != null)
            { _regionManager.RequestNavigate("MainRegion", navigatePath);}
        }

        private void NavigateListShow(string type)
        {
            var parameters = new NavigationParameters {{"type", type}};
            if (type != null)
            { _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);}
        }

        private void NavigateListMovie(string type)
        {
            var parameters = new NavigationParameters {{"type", type}};

            if (type != null)
            { _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);}
        }

        #region Constants

        private const string _mainView = "Главная";
        public string MainView
        {
            get { return _mainView; }
        }

        private const string _movies = "Фильмы";
        public string Movies
        {
            get { return _movies; }
        }

        private const string _shows = "Сериалы";
        public string Shows
        {
            get { return _shows; }
        }

        private const string _search = "Поиск";
        public string Search
        {
            get { return _search; }
        }

        private const string _help = "Справка";
        public string Help
        {
            get { return _help; }
        }

        private const string _best = "Лучшие";
        public string Best
        {
            get { return _best; }
        }

        private const string _popular = "Популярные";
        public string Popular
        {
            get { return _popular; }
        }

        private const string _upComing = "Скоро в кино";
        public string UpComing
        {
            get { return _upComing; }
        }

        private const string _nowPlaying = "Сейчас в кино";
        public string NowPlaying
        {
            get { return _nowPlaying; }
        }

        private const string _nowOnTV = "Сейчас на ТВ";
        public string NowOnTV
        {
            get { return _nowOnTV; }
        }

        private const string _searchMovie = "Найти фильм";
        public string SearchMovie
        {
            get { return _searchMovie; }
        }

        private const string _searchShow = "Найти сериал";
        public string SearchShow
        {
            get { return _searchShow; }
        }

        private const string _searchActor = "Найти актера";
        public string SearchActor
        {
            get { return _searchActor; }
        }

        private const string _aboutDeveloper = "О разработчике";
        public string AboutDeveloper
        {
            get { return _aboutDeveloper; }
        }

        private const string _aboutProgram = "О программе";
        public string AboutProgram
        {
            get { return _aboutProgram; }
        }

        private const string _information = "Информация";
        public string Information
        {
            get { return _information; }
        }

        #endregion
    }
}