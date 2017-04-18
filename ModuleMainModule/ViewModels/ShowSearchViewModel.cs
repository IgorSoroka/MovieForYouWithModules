using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class ShowSearchViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;
        public DelegateCommand NavigateCommandNameSearch { get; private set; }
        public DelegateCommand NavigateCommandSearch { get; private set; }
        public DelegateCommand NavigateCommandReset { get; private set; }

        public ShowSearchViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandNameSearch = new DelegateCommand(NameSearch);
            NavigateCommandSearch = new DelegateCommand(Search);
            NavigateCommandReset = new DelegateCommand(Reset);
            YearsList = GetYearsList();
        }

        #region Constants

        private const string _find = "Найти";
        public string Find
        {
            get { return _find; }
        }

        private const string _resetAll = "Сбросить";
        public string ResetAll
        {
            get { return _resetAll; }
        }

        private const string _nameSearching = "Поиск по названию";
        public string NameSearching
        {
            get { return _nameSearching; }
        }

        private const string _year = "Год создания";
        public string Year
        {
            get { return _year; }
        }

        private const string _interval = "Интервал годов";
        public string Interval
        {
            get { return _interval; }
        }

        private const string _from = "С";
        public string From
        {
            get { return _from; }
        }

        private const string _to = "по";
        public string To
        {
            get { return _to; }
        }

        private const string _raiting = "Рейтинг";
        public string Raiting
        {
            get { return _raiting; }
        }

        #endregion

        #region Properties

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

        #region Methods

        private void Reset()
        {
            SelectedFirstYear = null;
            SelectedLastYear = null;
            SelectedRating = 0;
            SelectedYear = null;
        }

        private void NameSearch()
        {
            var parameters = new NavigationParameters { { "name", Name } };
            _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);
        }

        private void Search()
        {
            var parameters = new NavigationParameters
            {
                {"SelectedYear", SelectedYear ?? 0},
                {"SelectedFirstYear", SelectedFirstYear ?? 0},
                {"SelectedLastYear", SelectedLastYear ?? 0},
                {"SelectedRating", SelectedRating}
            };
            _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);
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
    }
}