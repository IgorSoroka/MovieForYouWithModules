using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class MoviesListViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandShowDirectMovie { get; private set; }

        public MoviesListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectMovie = new DelegateCommand<int?>(NavigateShowDirectMovie);
        }

        private void NavigateShowDirectMovie(int? id)
        {
            //var parameters = new NavigationParameters();
            //parameters.Add("id", id);

            var parameters = new NavigationParameters();
            parameters.Add("id", SelectedMovie.Id);

            _regionManager.RequestNavigate("MainRegion", "MovieView", parameters);
        }

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set { SetProperty(ref _selectedMovie, value); }
        }

        private ObservableCollection<Movie> _movies;
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set { SetProperty(ref _movies, value); }
        }

        private async void GetPopularMovies()
        {
            List<Movie> moviesTest = await Data.GetPopularMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
            //SelectedMovie = null;
        }

        private async void GetBestMovies()
        {
            List<Movie> moviesTest = await Data.GetTopRatedMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
            //SelectedMovie = null;
        }

        private async void GetUpComingMovies()
        {
            List<Movie> moviesTest = await Data.GetUpCommingMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
            //SelectedMovie = null;
        }

        private async void GetNowPlayingMovies()
        {
            List<Movie> moviesTest = await Data.GetNewMoviesData();
            Movies = new ObservableCollection<Movie>(moviesTest);
            //SelectedMovie = null;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var type = navigationContext.Parameters["type"] as string;
            if (type != null)
            {
                if(type == "Best")
                    GetBestMovies();
                if(type == "Popular")
                    GetPopularMovies();
                if (type == "Future")
                    GetUpComingMovies();
                if (type == "Now")
                    GetNowPlayingMovies();
            }

            var genre = navigationContext.Parameters["genre"] as string;
            if (genre != null)
            {
                GetMoviesByGenre(genre);
            }

            var name = navigationContext.Parameters["name"] as string;
            if (name != null)
            {
                GetMoviesByName(name);
            }

            int selectedYear = (int)navigationContext.Parameters["SelectedYear"];
            int selectedFirstYear = (int)navigationContext.Parameters["SelectedFirstYear"];
            int selectedLastYear = (int)navigationContext.Parameters["SelectedLastYear"];
            decimal selectedRating = (decimal)navigationContext.Parameters["SelectedRating"];

            if (selectedFirstYear == 0 && selectedLastYear == 0 && selectedYear == 0)
            {
                GetMoviesByOnlyRating(selectedRating);
            }
            else if (selectedYear != 0)
            {
                GetMoviesByYearAndRating(selectedYear, selectedRating);
            }
            else if (selectedFirstYear != 0 && selectedLastYear != 0)
            {
                GetMoviesByFirstLastYearAndRating(selectedFirstYear, selectedLastYear, selectedRating);
            }
            else if (selectedFirstYear == 0 || selectedLastYear == 0)
            {
                if (selectedFirstYear != 0 && selectedLastYear == 0)
                {
                    GetMoviesByFirstYearAndRating(selectedFirstYear, selectedRating);
                }
                else if (selectedFirstYear == 0 && selectedLastYear != 0)
                {
                    GetMoviesByFLastYearAndRating(selectedLastYear, selectedRating);
                }
            }
        }

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
            int genreNumber = RepositoryGenres.GetGenreId(genre);
            List<Movie> moviesTest = await Data.GetListOfMoviesByGenre(genreNumber);
            Movies = new ObservableCollection<Movie>(moviesTest);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {}
    }
}