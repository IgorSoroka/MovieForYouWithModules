using System.Windows;
using MainModule;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;

namespace TestModule
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(MovieForYouMainModule));
        }
    }
}