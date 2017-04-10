using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class ShowSearchViewModel : BindableBase
    {
        IRegionManager _regionManager;
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
            var parameters = new NavigationParameters();
            parameters.Add("name", Name);
            _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);
        }

        private void Search()
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedYear", SelectedYear ?? 0);
            parameters.Add("SelectedFirstYear", SelectedFirstYear ?? 0);
            parameters.Add("SelectedLastYear", SelectedLastYear ?? 0);
            parameters.Add("SelectedRating", SelectedRating);
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
    }
}