using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.TMDb;
using System.Threading;
using MainModule;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleMainModule.ViewModels
{
    class ActorSearchViewModel : BindableBase
    {
        IRegionManager _regionManager;
        static readonly GetData Data = new GetData();
        public DelegateCommand<int?> NavigateCommandDirectActor { get; private set; }
        public DelegateCommand<string> NavigateCommandSearch { get; private set; }

        public ActorSearchViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommandDirectActor = new DelegateCommand<int?>(DirectActor);
            NavigateCommandSearch = new DelegateCommand<string>(Search);
            GetActorsData();
        }

        #region Properties

        private string _name;
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

        #endregion

        private async void GetActorsData()
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
        }

        private void Search(string obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("name", Name);
            _regionManager.RequestNavigate("ListRegion", "ActorsList", parameters);
        }

        private void DirectActor(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            _regionManager.RequestNavigate("MainRegion", "ActorView", parameters);
        }
    }
}
