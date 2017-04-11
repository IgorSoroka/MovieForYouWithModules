using System;
using MainModule;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    public class PlayerViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();

        public PlayerViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private Uri _video;
        public Uri Video
        {
            get { return _video; }
            set { SetProperty(ref _video, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            string videoUrl = navigationContext.Parameters["VideoUrl"] as string;
            Video = new Uri(string.Concat("http://www.youtube.com/embed/", videoUrl));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        { return true; }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }
    }
}
