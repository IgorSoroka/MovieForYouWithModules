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
        static readonly GetData Data = new GetData();
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
        {
            get { return _find; }
        }

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

         #endregion

        #region Methods

        private async void GetActorsData()
        {
            try
            {
                Watson = await Data.GetActor(10990);
                Johansson = await Data.GetActor(1245);
                Lawrence = await Data.GetActor(72129);
                Hathaway = await Data.GetActor(1813);
                Jackman = await Data.GetActor(6968);
                Depp = await Data.GetActor(85);
                Diesel = await Data.GetActor(12835);
                DiCaprio = await Data.GetActor(6193);
                Hardy = await Data.GetActor(2524);
                Pitt = await Data.GetActor(287);
                Downey = await Data.GetActor(3223);
                Robbie = await Data.GetActor(234352);
            }
            catch (System.NullReferenceException ex)
            {
                RaiseNotificationNull();
                logger.ErrorException("ActorSearchViewModel", ex);
            }
            catch (ServiceRequestException)
            {
                RaiseNotification();
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        private void RaiseNotification()
        {
            this.NotificationRequest.Raise(
               new Notification { Content = "Превышено число запросов к серверу", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
        }

        private void RaiseNotificationNull()
        {
            this.NotificationRequestNull.Raise(
               new Notification { Content = "Для перехода дождитесь полной загрузки данных по выбранному Вами актеру", Title = "Ошибка" },
               n => { InteractionResultMessage = "The user was notified."; });
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
                logger.ErrorException("MovieListViewModel", e);
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
                logger.ErrorException("ActorSearchViewModel", ex);
            }
            catch (Exception e)
            {
                logger.ErrorException("MovieListViewModel", e);
            }
        }

        #endregion

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return OnValidate(propertyName); }
        }

        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Invalid property name", propertyName);

            string error = string.Empty;
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
            return error;
        }


         
    }
}