using System;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    public class PlayerViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        IRegionNavigationJournal _journal;
        public DelegateCommand GoBackCommand { get; set; }

        public PlayerViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            GoBackCommand = new DelegateCommand(GoBack);
        }

        private Uri _video;
        public Uri Video
        {
            get { return _video; }
            set { SetProperty(ref _video, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;

            string videoUrl = navigationContext.Parameters["VideoUrl"] as string;
            Video = new Uri(string.Concat("http://www.youtube.com/embed/", videoUrl));
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
