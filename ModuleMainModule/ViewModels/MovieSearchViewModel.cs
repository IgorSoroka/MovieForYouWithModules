using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class MovieSearchViewModel : BindableBase
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandNameSearch { get; private set; }
        public DelegateCommand NavigateCommandGenreSearch { get; private set; }
        public DelegateCommand NavigateCommandSearch { get; private set; }

        public MovieSearchViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandNameSearch = new DelegateCommand(NameSearch);
            NavigateCommandGenreSearch = new DelegateCommand(GenreSearch);
            NavigateCommandSearch = new DelegateCommand(Search);

            YearsList = GetYearsList();
            List<string> genresList = RepositoryGenres.GetNames();
            Genres = new ObservableCollection<string>(genresList);
        }

        #region Methods

        private void NameSearch()
        {
            var parameters = new NavigationParameters();
            parameters.Add("name", Name);

            _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
        }

        private void GenreSearch()
        {
            var parameters = new NavigationParameters();
            parameters.Add("genre", SelectGenre);

            _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
        }

        private void Search()
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedYear", SelectedYear ?? 0);
            parameters.Add("SelectedFirstYear", SelectedFirstYear ?? 0);
            parameters.Add("SelectedLastYear", SelectedLastYear ?? 0);
            parameters.Add("SelectedRating", SelectedRating);

            _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
        }

        private ObservableCollection<int> GetYearsList()
        {
            ObservableCollection<int> years = new ObservableCollection<int>();
            for (int i = 2017; i >= 1900; i--)
            {
                years.Add(i);
            }
            return years;
        }

        #endregion

        #region Properties

        private ObservableCollection<string> _genres;
        public ObservableCollection<string> Genres
        {
            get { return _genres; }
            set { SetProperty(ref _genres, value); }
        }

        private string _selectGenre;
        public string SelectGenre
        {
            get { return _selectGenre; }
            set { SetProperty(ref _selectGenre, value); }
        }

        private ObservableCollection<int> _yearsList;
        public ObservableCollection<int> YearsList
        {
            get { return _yearsList; }
            set { SetProperty(ref _yearsList, value); }
        }

        private int? _selectedYear;
        public int? SelectedYear
        {
            get { return _selectedYear; }
            set { SetProperty(ref _selectedYear, value); }
        }

        private int? _selectedFirstYear;
        public int? SelectedFirstYear
        {
            get { return _selectedFirstYear; }
            set { SetProperty(ref _selectedFirstYear, value); }
        }

        private int? _selectedLastYear;
        public int? SelectedLastYear
        {
            get { return _selectedLastYear; }
            set { SetProperty(ref _selectedLastYear, value); }
        }

        private decimal _selectedRating;
        public decimal SelectedRating
        {
            get { return _selectedRating; }
            set { SetProperty(ref _selectedRating, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion
    }
}