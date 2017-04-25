using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.TMDb;
using MainModule;
using NLog;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
#pragma warning disable 618

namespace ModuleMainModule.ViewModels
{
    class ActorSearchViewModel : BindableBase, IDataErrorInfo
    {
        private readonly IRegionManager _regionManager;
        private static readonly TheMovieDBDataService DataService = new TheMovieDBDataService();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand<Person> NavigateCommandDirectActor { get; private set; }
        public DelegateCommand<string> NavigateCommandSearch { get; private set; }      
        public InteractionRequest<INotification> NotificationRequest { get; }
        public InteractionRequest<INotification> NotificationRequestNull { get; }

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
        public string Find => _find;

        private const string _regulations = "Можно использовать только буквы, цифрры и символ '-'";
        public string Regulations => _regulations;

        private const string ForExceptions = "ActorSearchViewModel";
        private const string InvalidPropertyName = "Некорретное имя свойства";
        private const string ExceededNumberRequests  = "Превышено число запросов к серверу";
        private const string WarningError = "Ошибка";
        private const string WaitFullDownload = "Для перехода дождитесь полной загрузки данных по выбранному Вами актеру";
        private const string UserNotified = "Пользователь был оповещен";

        private const int WatsonId = 10990;
        private const int JohanssonId = 1245;
        private const int LawrenceId = 72129;
        private const int HathawayId = 1813;
        private const int JackmanId = 6968;
        private const int DeppId = 85;
        private const int DieselId = 12835;
        private const int DiCaprioId = 6193;
        private const int HardyId = 2524;
        private const int PittId = 287;
        private const int DowneyId = 3223;
        private const int RobbieId = 234352;

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
                Watson = await DataService.GetActor(WatsonId);
                Johansson = await DataService.GetActor(JohanssonId);
                Lawrence = await DataService.GetActor(LawrenceId);
                Hathaway = await DataService.GetActor(HathawayId);
                Jackman = await DataService.GetActor(JackmanId);
                Depp = await DataService.GetActor(DeppId);
                Diesel = await DataService.GetActor(DieselId);
                DiCaprio = await DataService.GetActor(DiCaprioId);
                Hardy = await DataService.GetActor(HardyId);
                Pitt = await DataService.GetActor(PittId);
                Downey = await DataService.GetActor(DowneyId);
                Robbie = await DataService.GetActor(RobbieId);
            }
            catch (NullReferenceException ex)
            {
                RaiseNotificationNull();
                _logger.ErrorException(ForExceptions, ex);
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        private void RaiseNotification()
        {
            NotificationRequest.Raise(
               new Notification { Content = ExceededNumberRequests, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
        }

        private void RaiseNotificationNull()
        {
            NotificationRequestNull.Raise(
               new Notification { Content = WaitFullDownload, Title = WarningError },
               n => { InteractionResultMessage = UserNotified; });
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
                _logger.ErrorException(ForExceptions, e);
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
            catch (NullReferenceException ex)
            {
                RaiseNotificationNull();
                _logger.ErrorException(ForExceptions, ex);
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        string IDataErrorInfo.this[string propertyName] => OnValidate(propertyName);

        protected virtual string OnValidate(string propertyName)
        {
            string error = String.Empty;
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