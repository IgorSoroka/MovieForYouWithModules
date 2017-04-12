using System;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using ModuleMainModule.Model;
using Prism.Commands;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class MoviesListViewModel : BindableBase, INavigationAware
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandShowDirectMovie { get; private set; }

        public MoviesListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand(NavigateShowDirectMovie);
        }

        private void NavigateShowDirectMovie()
        {
            var parameters = new NavigationParameters {{"id", SelectedMovie.Id}};
            _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
        }

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set { SetProperty(ref _selectedMovie, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<Movie> _movies;
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set { SetProperty(ref _movies, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var type = navigationContext.Parameters["type"] as string;
                if (type != null)
                {
                    if (type == "Best")
                    {
                        GetBestMovies();
                        Title = "Лучшие фильмы";
                    }
                    if (type == "Popular")
                    {
                        GetPopularMovies();
                        Title = "Популярные фильмы";
                    }
                    if (type == "Future")
                    {
                        GetUpComingMovies();
                        Title = "Скоро в кино";
                    }
                    if (type == "Now")
                    {
                        GetNowPlayingMovies();
                        Title = "Сейчас в кино";
                    }
                }

                var genre = navigationContext.Parameters["genre"] as string;
                if (genre != null)
                { GetMoviesByGenre(genre); }

                var company = navigationContext.Parameters["company"] as string;
                if (company != null)
                { GetMoviesByCompany(company); }

                var name = navigationContext.Parameters["name"] as string;
                if (name != null)
                { GetMoviesByName(name); }

                var selectedYear = (int)navigationContext.Parameters["SelectedYear"];
                var selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
                var selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
                var selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

                if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
                { GetMoviesByOnlyRating(selectedRating); }
                else if (selectedYear != 0)
                { GetMoviesByYearAndRating(selectedYear, selectedRating); }
                else if (selectedFirstYear != 0 && selectedLastYear != 0)
                { GetMoviesByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating); }
                else if (selectedFirstYear == 0 || selectedLastYear == 0)
                {
                    if (selectedFirstYear != 0 && selectedLastYear == 0)
                    { GetMoviesByFirstYearAndRating(selectedFirstYear, selectedRating); }
                    else if (selectedFirstYear == 0 && selectedLastYear != 0)
                    { GetMoviesByFLastYearAndRating(selectedLastYear, selectedRating); }
                }
            }
            catch (NullReferenceException e)
            {
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {}

        #region Methods

        private async void GetMoviesByFLastYearAndRating(int selectedLastYear, decimal selectedRating)
        {
            List<Movie> moviesTest = await Data.GetSearchedMoviesLastYear(selectedLastYear, selectedRating);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByFirstYearAndRating(int selectedFirstYear, decimal selectedRating)
        {
            List<Movie> moviesTest = await Data.GetSearchedMoviesFirstYear(selectedFirstYear, selectedRating);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByFirstLastYearAndRating(int selectedFirstYear, int selectedLastYear, decimal selectedRating)
        {
            List<Movie> moviesTest = await Data.GetSearchedMovies(selectedFirstYear, selectedLastYear, selectedRating);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByYearAndRating(int selectedYear, decimal selectedRating)
        {
            List<Movie> moviesTest = await Data.GetSearchedMovies(selectedYear, selectedRating);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByOnlyRating(decimal selectedRating)
        {
            List<Movie> moviesTest = await Data.GetSearchedMovies(selectedRating);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByName(string name)
        {
            List<Movie> moviesTest = await Data.GetMoviesByName(name);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByGenre(string genre)
        {
            var genreNumber = RepositoryGenres.GetGenreId(genre);
            List<Movie> moviesTest = await Data.GetListOfMoviesByGenre(genreNumber);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetMoviesByCompany(string company)
        {
            var companyNumber = RepositoryCompanies.GetCompanyId(company);
            List<Movie> moviesTest = await Data.GetListOfMoviesByCompany(companyNumber);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetPopularMovies()
        {
            List<Movie> moviesTest = await Data.GetPopularMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetBestMovies()
        {
            List<Movie> moviesTest = await Data.GetTopRatedMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetUpComingMovies()
        {
            List<Movie> moviesTest = await Data.GetUpCommingMoviesData();
            //moviesTest.OrderBy(item => item.ReleaseDate.Value.Date);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        private async void GetNowPlayingMovies()
        {
            List<Movie> moviesTest = await Data.GetNewMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        #endregion
    }
}