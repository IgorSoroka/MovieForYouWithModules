﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using System.Threading.Tasks;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class ActorsListViewModel : BindableBase, INavigationAware
    {
        readonly IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand NavigateCommandShowDirectActor { get; private set; }

        public ActorsListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandShowDirectActor = new DelegateCommand(ShowDirectActor);
        }

        private Person _selectedSearchedActor;
        public Person SelectedSearchedActor
        {
            get { return _selectedSearchedActor; }
            set { SetProperty(ref _selectedSearchedActor, value); }
        }

        private ObservableCollection<Person> _actorsList;
        public ObservableCollection<Person> ActorsList
        {
            get { return _actorsList; }
            set { SetProperty(ref _actorsList, value); }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var name = navigationContext.Parameters["name"] as string;
            if (name != null)
                await GetSearchedActors(name);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        private async Task GetSearchedActors(string name)
        {
            List<Person> actorsTest = await Data.GetActorsByName(name);
            ActorsList = new ObservableCollection<Person>(actorsTest);
        }

        private void ShowDirectActor()
        {
            var parameters = new NavigationParameters { { "id", SelectedSearchedActor.Id } };
            _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
        }
    }
}