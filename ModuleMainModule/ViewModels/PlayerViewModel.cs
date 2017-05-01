using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NLog;
#pragma warning disable 618

namespace ModuleMainModule.ViewModels
{
    public class PlayerViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationJournal _journal;
        private readonly Logger _logger;

        public DelegateCommand GoBackCommand { get; set; }

        public PlayerViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            _logger = LogManager.GetCurrentClassLogger();
            GoBackCommand = new DelegateCommand(GoBack);
        }

        private const string _backDescription = "Назад к описанию";
        public string BackDescription => _backDescription;

        private const string ForExceptions = "PlayerViewModel";

        private Uri _video;
        public Uri Video
        {
            get { return _video; }
            set { SetProperty(ref _video, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                _journal = navigationContext.NavigationService.Journal;
                string videoUrl = navigationContext.Parameters["VideoUrl"] as string;
                Video = new Uri(string.Concat("http://www.youtube.com/embed/", videoUrl));
            }
            catch (Exception e)
            {
                _logger.ErrorException(ForExceptions, e);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private void GoBack()
        {
            Video = null;
            _journal.GoBack();
        }
    }
}