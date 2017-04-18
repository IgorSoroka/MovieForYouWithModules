using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;
using MainModule;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using ModuleMainModule.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class MovieViewModel : BindableBase, INavigationAware
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }
        public DelegateCommand NavigateCommandShowTrailler { get; private set; }
        public DelegateCommand NavigateCommandAddToDb { get; private set; }
        static readonly IMovieService MovieService = new MovieService();

        public MovieViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(NavigateShowDirectActor);
            NavigateCommandShowTrailler = new DelegateCommand(ShowTrailler);
            NavigateCommandAddToDb = new DelegateCommand(AddToDb);
        }

        #region Constants

        private const string _plot = "Сюжет";
        public string Plot
        {
            get { return _plot; }
        }

        private const string _addFavorites = "Добавить в избранное";
        public string AddFavorites
        {
            get { return _addFavorites; }
        }

        private const string _trailer = "Смотреть трейлер";
        public string Trailer
        {
            get { return _trailer; }
        }

        private const string _showCast = "Состав";
        public string ShowCast
        {
            get { return _showCast; }
        }

        private const string _mainRoles = "В главных ролях";
        public string MainRoles
        {
            get { return _mainRoles; }
        }

        private const string _originalName = "Оригинальное название";
        public string OriginalName
        {
            get { return _originalName; }
        }

        private const string _raiting = "Рейтинг";
        public string Raiting
        {
            get { return _raiting; }
        }

        private const string _voteCount = "Количество голосов";
        public string VoteCount
        {
            get { return _voteCount; }
        }

        private const string _genres = "Жанры";
        public string Genres
        {
            get { return _genres; }
        }

        private const string _countries = "Страны производители";
        public string Countries
        {
            get { return _countries; }
        }

        private const string _keywords = "Ключевые слова";
        public string Keywords
        {
            get { return _keywords; }
        }

        private const string _homePage = "Домашняя страница";
        public string HomePage
        {
            get { return _homePage; }
        }

        private const string _premiere = "Премьера";
        public string Premiere
        {
            get { return _premiere; }
        }

        private const string _aboutMovie = "О фильме";
        public string AboutMovie
        {
            get { return _aboutMovie; }
        }

        private const string _duration = "Продолжительность";
        public string Duration
        {
            get { return _duration; }
        }

        private const string _budget = "Бюджет";
        public string Budget
        {
            get { return _budget; }
        }

        private const string _revenue = "Кассовые сборы (США)";
        public string Revenue
        {
            get { return _revenue; }
        }

        private const string _companies = "Компании производители";
        public string Companies
        {
            get { return _companies; }
        }

        #endregion

        #region Properties

        private string _videoUrl;
        public string VideoUrl
        {
            get { return _videoUrl; }
            set { SetProperty(ref _videoUrl, value); }
        }

        private Movie _direcctMovie;
        public Movie DirectMovie
        {
            get { return _direcctMovie; }
            set { SetProperty(ref _direcctMovie, value); }
        }

        private MediaCast _selectedActor;
        public MediaCast SelectedActor
        {
            get { return _selectedActor; }
            set { SetProperty(ref _selectedActor, value); }
        }

        private ObservableCollection<MediaCast> _cast;
        public ObservableCollection<MediaCast> Cast
        {
            get { return _cast; }
            set { SetProperty(ref _cast, value); }
        }

        private ObservableCollection<MediaCrew> _crew;
        public ObservableCollection<MediaCrew> Crew
        {
            get { return _crew; }
            set { SetProperty(ref _crew, value); }
        }

        #endregion

        #region Methods

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            VideoUrl = null;
            var type = (int)navigationContext.Parameters["id"];
            await GetDirectMovieInfo(type);
            await GetVideoUrl(type);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void ShowTrailler()
        {
            var parameters = new NavigationParameters { { "VideoUrl", VideoUrl } };
            _regionManager.RequestNavigate("MainRegion", "Player", parameters);
        }

        private void NavigateShowDirectActor()
        {
            var parameters = new NavigationParameters { { "id", SelectedActor.Id } };
            _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
        }

        private async Task GetVideoUrl(int id)
        {
            var video = await Data.GetTrailler(id);
            if (video != null)
            {
                VideoUrl = video.Key;
            }
        }

        private async Task GetDirectMovieInfo(int id)
        {
            var movie = await Data.GetDirectMoveData(id);
            List<MediaCrew> crews = (movie.Credits.Crew).Take(10).ToList();
            List<MediaCast> casts = (movie.Credits.Cast).Take(10).ToList();
            DirectMovie = movie;
            Crew = new ObservableCollection<MediaCrew>(crews);
            Cast = new ObservableCollection<MediaCast>(casts);
        }

        private void AddToDb()
        {
            MovieDTO movie = new MovieDTO() { Name = DirectMovie.OriginalTitle, Id = DirectMovie.Id, Rating = 7 };
            IEnumerable<MovieDTO> movies = MovieService.GetMovies();
            MovieService.TakeMovie(movie);
            IEnumerable<MovieDTO> moviesPast = MovieService.GetMovies();
        }

        #endregion
    }
}