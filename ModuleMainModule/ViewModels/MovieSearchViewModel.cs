﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using ModuleMainModule.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using NLog;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ModuleMainModule.ViewModels
{
    class MovieSearchViewModel : BindableBase, IDataErrorInfo
    {
        private readonly IRegionManager _regionManager;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand NavigateCommandNameSearch { get; private set; }
        public DelegateCommand NavigateCommandGenreSearch { get; private set; }
        public DelegateCommand NavigateCommandCompanySearch { get; private set; }
        public DelegateCommand NavigateCommandSearch { get; private set; }
        public DelegateCommand NavigateCommandReset { get; private set; }       

        public MovieSearchViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandNameSearch = new DelegateCommand(NameSearch);
            NavigateCommandGenreSearch = new DelegateCommand(GenreSearch);
            NavigateCommandSearch = new DelegateCommand(Search);
            NavigateCommandCompanySearch = new DelegateCommand(CompanySearch);
            NavigateCommandReset = new DelegateCommand(Reset);

            YearsList = GetYearsList();
            List<string> genresList = RepositoryGenres.GetNames();
            Genres = new ObservableCollection<string>(genresList);
            List<string> companiesList = RepositoryCompanies.GetNames();
            Companies = new ObservableCollection<string>(companiesList);
        }

        #region Constants

        private const string _find = "Найти";
        public string Find
        {  get { return _find; }   }

        private const string _resetAll = "Сбросить";
        public string ResetAll
        {  get { return _resetAll; }   }

        private const string _nameSearching = "Поиск по названию";
        public string NameSearching
        {  get { return _nameSearching; }   }

        private const string _year = "Год создания";
        public string Year
        {  get { return _year; }  }

        private const string _interval = "Интервал годов";
        public string Interval
        {  get { return _interval; }  }

        private const string _from = "С";
        public string From
        {  get { return _from; }  }

        private const string _to = "по";
        public string To
        {  get { return _to; }  }

        private const string _raiting = "Рейтинг";
        public string Raiting
        {  get { return _raiting; }  }

        private const string _genreSearching = "Поиск по жанру";
        public string GenreSearching
        {  get { return _genreSearching; }  }

        private const string _companySearching = "Поиск по компинии";
        public string CompanySearching
        {   get { return _companySearching; }  }

        private const string _regulations = "Можно использовать только буквы, цифрры и символы '!', '?', '-', '(', ')'";
        public string Regulations
        { get { return _regulations; } }

        private const string _forExceptions = "MovieSearchViewModel";
        private const string _invalidPropertyName = "Некорретное имя свойства";

        private const int _minYear = 1990;
        private const int _maxYear = 2017;

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

        private ObservableCollection<string> _companies;
        public ObservableCollection<string> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }

        private string _selectedCompany;
        public string SelectedCompany
        {
            get { return _selectedCompany; }
            set { SetProperty(ref _selectedCompany, value); }
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
            set
            { SetProperty(ref _selectedYear, value); }
        }

        private int? _selectedFirstYear;
        public int? SelectedFirstYear
        {
            get { return _selectedFirstYear; }
            set
            { SetProperty(ref _selectedFirstYear, value); }
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
        [Required]
        [RegularExpression(@"^[а-яА-Яa-zA-Z0-9\-''-'!?,.()\s]{2,40}$")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private bool _canSave;

        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        public string Error
        {
            get  {   throw new NotImplementedException();  }
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
            try
            {
                var parameters = new NavigationParameters { { "name", Name } };
                _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void GenreSearch()
        {
            try
            {
                var parameters = new NavigationParameters { { "genre", SelectGenre } };
                _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void Search()
        {
            try
            {
                var parameters = new NavigationParameters
                    {
                        {"SelectedYear", SelectedYear ?? 0},
                        {"SelectedFirstYear", SelectedFirstYear ?? 0},
                        {"SelectedLastYear", SelectedLastYear ?? 0},
                        {"SelectedRating", SelectedRating}
                    };
                _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void CompanySearch()
        {
            try
            {
                var parameters = new NavigationParameters { { "company", SelectedCompany } };
                _regionManager.RequestNavigate("ListRegion", "MoviesList", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private ObservableCollection<int> GetYearsList()
        {
            ObservableCollection<int> years = new ObservableCollection<int>();
            for (int i = _maxYear; i >= _minYear; i--)
            {
                years.Add(i);
            }
            return years;
        }      

        string IDataErrorInfo.this[string propertyName]
        {
            get { return OnValidate(propertyName); }
        }

        protected virtual string OnValidate(string propertyName)
        {
            string error = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                    throw new ArgumentException(_invalidPropertyName, propertyName);


                var value = this.GetType().GetProperty(propertyName).GetValue(this, null);
                var results = new List<ValidationResult>(1);

                var context = new ValidationContext(this, null, null) { MemberName = propertyName };

                var result = Validator.TryValidateProperty(value, context, results);

                if (!result)
                {
                    var validationResult = results.First();
                    error = validationResult.ErrorMessage;
                }
                CanSave = error == String.Empty;
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
            return error;
        }

        #endregion
    }
}