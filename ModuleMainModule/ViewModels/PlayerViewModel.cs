﻿using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NLog;

namespace ModuleMainModule.ViewModels
{
    public class PlayerViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationJournal _journal;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand GoBackCommand { get; set; }

        public PlayerViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            GoBackCommand = new DelegateCommand(GoBack);
        }

        private const string _backDescription = "Назад к описанию";
        public string BackDescription
        {   get { return _backDescription; }   }

        private const string _forExceptions = "PlayerViewModel";

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
                logger.ErrorException(_forExceptions, e);
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