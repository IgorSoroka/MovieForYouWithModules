using System.Net.TMDb;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NLog;
using Prism.Interactivity.InteractionRequest;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace ModuleMainModule.ViewModels
{
    class ActorSearchViewModel : BindableBase, IDataErrorInfo
    {
        private readonly IRegionManager _regionManager;
        private static readonly GetData Data = new GetData();
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand<Person> NavigateCommandDirectActor { get; private set; }
        public DelegateCommand<string> NavigateCommandSearch { get; private set; }      
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public InteractionRequest<INotification> NotificationRequestNull { get; private set; }

        public ActorSearchViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandDirectActor = new DelegateCommand<Person>(DirectActor);
            NavigateCommandSearch = new DelegateCommand<string>(Search);
            NotificationRequest = new InteractionRequest<INotification>();
            NotificationRequestNull = new InteractionRequest<INotification>();
            GetActorsData();
        }

        #region Constants

        private const string _find = "Найти";
        public string Find
        { get { return _find; }  }

        private const string _regulations = "Можно использовать только буквы, цифрры и символ '-'";
        public string Regulations
        { get { return _regulations; } }

        private const string _forExceptions = "ActorSearchViewModel";
        private const string _invalidPropertyName = "Некорретное имя свойства";
        private const string _exceededNumberRequests  = "Превышено число запросов к серверу";
        private const string _error = "Ошибка";
        private const string _waitFullDownload = "Для перехода дождитесь полной загрузки данных по выбранному Вами актеру";
        private const string _userNotified = "Пользователь был оповещен";

        private const int _watsonId = 10990;
        private const int _johanssonId = 1245;
        private const int _lawrenceId = 72129;
        private const int _hathawayId = 1813;
        private const int _jackmanId = 6968;
        private const int _deppId = 85;
        private const int _dieselId = 12835;
        private const int _diCaprioId = 6193;
        private const int _hardyId = 2524;
        private const int _pittId = 287;
        private const int _downeyId = 3223;
        private const int _robbieId = 234352;

        #endregion

        #region Properties

        private string _name;

        [Required]
        [RegularExpression(@"^[а-яА-Яa-zA-Z0-9\-''-'\s]{2,40}$")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    
        private Person _watson;
        public Person Watson
        {
            get { return _watson; }
            set { SetProperty(ref _watson, value); }
        }

        private Person _johansson;
        public Person Johansson
        {
            get { return _johansson; }
            set { SetProperty(ref _johansson, value); }
        }

        private Person _lawrence;
        public Person Lawrence
        {
            get { return _lawrence; }
            set { SetProperty(ref _lawrence, value); }
        }

        private Person _hathaway;
        public Person Hathaway
        {
            get { return _hathaway; }
            set { SetProperty(ref _hathaway, value); }
        }

        private Person _jackman;
        public Person Jackman
        {
            get { return _jackman; }
            set { SetProperty(ref _jackman, value); }
        }

        private Person _depp;
        public Person Depp
        {
            get { return _depp; }
            set { SetProperty(ref _depp, value); }
        }

        private Person _diesel;
        public Person Diesel
        {
            get { return _diesel; }
            set { SetProperty(ref _diesel, value); }
        }

        private Person _dicaprio;
        public Person DiCaprio
        {
            get { return _dicaprio; }
            set { SetProperty(ref _dicaprio, value); }
        }

        private Person _hardy;
        public Person Hardy
        {
            get { return _hardy; }
            set { SetProperty(ref _hardy, value); }
        }

        private Person _pitt;
        public Person Pitt
        {
            get { return _pitt; }
            set { SetProperty(ref _pitt, value); }
        }

        private Person _downey;
        public Person Downey
        {
            get { return _downey; }
            set { SetProperty(ref _downey, value); }
        }

        private Person _robbie;
        public Person Robbie
        {
            get { return _robbie; }
            set { SetProperty(ref _robbie, value); }
        }

        private bool _canSave;

        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        public string InteractionResultMessage { get; private set; }

        public string Error
        {
            get {  throw new NotImplementedException(); }
        }

        #endregion

        #region Methods

        private async void GetActorsData()
        {
            try
            {
                Watson = await Data.GetActor(_watsonId);
                Johansson = await Data.GetActor(_johanssonId);
                Lawrence = await Data.GetActor(_lawrenceId);
                Hathaway = await Data.GetActor(_hathawayId);
                Jackman = await Data.GetActor(_jackmanId);
                Depp = await Data.GetActor(_deppId);
                Diesel = await Data.GetActor(_dieselId);
                DiCaprio = await Data.GetActor(_diCaprioId);
                Hardy = await Data.GetActor(_hardyId);
                Pitt = await Data.GetActor(_pittId);
                Downey = await Data.GetActor(_downeyId);
                Robbie = await Data.GetActor(_robbieId);
            }
            catch (NullReferenceException ex)
            {
                RaiseNotificationNull();
                logger.ErrorException(_forExceptions, ex);
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = _exceededNumberRequests, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = _waitFullDownload, Title = _error },
               n => { InteractionResultMessage = _userNotified; });
        }

        private void Search(string obj)
        {
            try
            {
                var parameters = new NavigationParameters { { "name", Name } };
                _regionManager.RequestNavigate("ListRegion", "ActorsList", parameters);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
        }

        private void DirectActor(Person person)
        {
            try
            {
                var id = person.Id;
                var parameters = new NavigationParameters { { "id", id } };
                _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
            }
            catch (System.NullReferenceException ex)
            {
                RaiseNotificationNull();
                logger.ErrorException(_forExceptions, ex);
            }
            catch (Exception e)
            {
                logger.ErrorException(_forExceptions, e);
            }
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