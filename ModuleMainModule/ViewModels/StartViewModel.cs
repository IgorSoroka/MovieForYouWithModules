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
    class StartViewModel : BindableBase
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandMovie { get; private set; }
        //public DelegateCommand NavigateCommandMovie { get; private set; }
        public DelegateCommand<int?> NavigateCommandShow { get; private set; }

        public StartViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            GetAllData();
            NavigateCommandMovie = new DelegateCommand<int?>(ShowDirectMovie);
            NavigateCommandShow = new DelegateCommand<int?>(ShowDirectShow);
        }

        private void ShowDirectMovie(int? obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", obj);

            _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
        }

        private void ShowDirectShow(int? obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", obj);

            _regionManager.RequestNavigate("MainRegion", "ShowView", parameters);
        }

        private async void GetAllData()
        {
            List<Movie> moviesTest = await Data.GetPopularMoviesData();
            BestMovie = moviesTest.First();
            SecondMovie = moviesTest[1];
            ThirdMovie = moviesTest[2];

            List<Show> showsTest = await Data.GetPopularShowsData();
            BestShow = showsTest.First();
            SecondShow = showsTest[1];
            ThirdShow = showsTest[2];
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
    }
}