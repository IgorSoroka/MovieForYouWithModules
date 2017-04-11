using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using MainModule;
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

        public MovieViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(NavigateShowDirectActor);
            NavigateCommandShowTrailler = new DelegateCommand(ShowTrailler);
        }

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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VideoUrl = null;
            var type = (int)navigationContext.Parameters["id"];
            GetDirectMovieInfo(type);
            GetVideoUrl(type);
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

        private async void GetVideoUrl(int id)
        {
            var video = await Data.GetTrailler(id);
            if (video != null)
            {
                VideoUrl = video.Key;
            }
        }

        private async void GetDirectMovieInfo(int id)
        {
            var movie = await Data.GetDirectMoveData(id);
            List<MediaCrew> crews = (movie.Credits.Crew).Take(20).ToList();
            List<MediaCast> casts = (movie.Credits.Cast).Take(10).ToList();
            DirectMovie = movie;
            Crew = new ObservableCollection<MediaCrew>(crews);
            Cast = new ObservableCollection<MediaCast>(casts);
        }

        #endregion
    }
}