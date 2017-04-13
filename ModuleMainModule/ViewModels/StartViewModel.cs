using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class StartViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<Movie> NavigateCommandMovie { get; private set; }
        public DelegateCommand<Show> NavigateCommandShow { get; private set; }

        public StartViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            GetAllData();
            NavigateCommandMovie = new DelegateCommand<Movie>(ShowDirectMovie);
            NavigateCommandShow = new DelegateCommand<Show>(ShowDirectShow);
        }

        #region Properties

        private Movie _bestMovie;
        public Movie BestMovie
        {
            get { return _bestMovie; }
            set { SetProperty(ref _bestMovie, value); }
        }

        private Movie _secondMovie;
        public Movie SecondMovie
        {
            get { return _secondMovie; }
            set { SetProperty(ref _secondMovie, value); }
        }

        private Movie _thirdMovie;
        public Movie ThirdMovie
        {
            get { return _thirdMovie; }
            set { SetProperty(ref _thirdMovie, value); }
        }

        private Show _bestShow;
        public Show BestShow
        {
            get { return _bestShow; }
            set { SetProperty(ref _bestShow, value); }
        }

        private Show _secondShow;
        public Show SecondShow
        {
            get { return _secondShow; }
            set { SetProperty(ref _secondShow, value); }
        }

        private Show _thirdShow;
        public Show ThirdShow
        {
            get { return _thirdShow; }
            set { SetProperty(ref _thirdShow, value); }
        }
        #endregion

        #region Methods

        private void ShowDirectMovie(Movie movie)
        {
            try
            {
                var id = movie.Id;
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
            }
            catch (System.NullReferenceException e)
            {
            }
        }

        private void ShowDirectShow(Show show)
        {
            try
            {
                var id = show.Id;
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
            }
            catch (System.NullReferenceException e)
            {
            }
           
        }

        private async void GetAllData()
        {
            try
            {
                List<Movie> moviesTest = await Data.GetPopularMoviesData(1);
                BestMovie = moviesTest.First();
                SecondMovie = moviesTest[1];
                ThirdMovie = moviesTest[2];
                List<Show> showsTest = await Data.GetPopularShowsData(1);
                BestShow = showsTest.First();
                SecondShow = showsTest[1];
                ThirdShow = showsTest[2];
            }
            catch (System.Net.TMDb.ServiceRequestException e)
            {
            }
        }

        #endregion
    }
}