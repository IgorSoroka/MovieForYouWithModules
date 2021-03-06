﻿using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using NLog;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable 618

namespace ModuleMainModule.ViewModels
{
    class ShowSearchViewModel : BindableBase, IDataErrorInfo
    {
        private readonly IRegionManager _regionManager;
        private readonly Logger _logger;

        #region Constants

        private const string _find = "Найти";
        public string Find => _find;

        private const string _resetAll = "Сбросить";
        public string ResetAll => _resetAll;

        private const string _nameSearching = "Поиск по названию";
        public string NameSearching => _nameSearching;

        private const string _year = "Год создания";
        public string Year => _year;

        private const string _interval = "Интервал годов";
        public string Interval => _interval;

        private const string _from = "С";
        public string From => _from;

        private const string _to = "по";
        public string To => _to;

        private const string _raiting = "Рейтинг";
        public string Raiting => _raiting;

        private const string _regulations = "Можно использовать только буквы, цифрры и символы '!', '?', '-', '(', ')'";
        public string Regulations => _regulations;

        private const string ForExceptions = "ShowSearchtViewModel";
        private const string InvalidPropertyName = "Некорретное имя свойства";

        private const int MinYear = 1990;
        private const int MaxYear = 2017;

        #endregion

        public DelegateCommand NavigateCommandNameSearch { get; private set; }
        public DelegateCommand NavigateCommandSearch { get; private set; }
        public DelegateCommand NavigateCommandReset { get; private set; }

        public ShowSearchViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            _logger = LogManager.GetCurrentClassLogger();

            NavigateCommandNameSearch = new DelegateCommand(NameSearch);
            NavigateCommandSearch = new DelegateCommand(Search);
            NavigateCommandReset = new DelegateCommand(Reset);
            YearsList = GetYearsList();
        }
        
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
        [Required]
        [RegularExpression(@"^[а-яА-Яa-zA-Z0-9\-''-'!?,.()\s]{1,40}$")]
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
        { get { throw new NotImplementedException(); } }

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
                if (CanSave)
                {
                    var parameters = new NavigationParameters { { "name", Name } };
                    _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);
                }
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
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
                _regionManager.RequestNavigate("ListRegion", "ShowsList", parameters);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private ObservableCollection<int> GetYearsList()
        {
            ObservableCollection<int> years = new ObservableCollection<int>();
            for (int i = MaxYear; i >= MinYear; i--)
            {
                years.Add(i);
            }
            return years;
        }    

        string IDataErrorInfo.this[string propertyName] => OnValidate(propertyName);
        /// <summary>
        /// Метод для валидации ввода названия (свойство Name)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual string OnValidate(string propertyName)
        {
            string error = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                    throw new ArgumentException(InvalidPropertyName, propertyName);

                var value = GetType().GetProperty(propertyName).GetValue(this, null);
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
                _logger.ErrorException(ForExceptions, e);
            }
            return error;
        }

        #endregion
    }
}