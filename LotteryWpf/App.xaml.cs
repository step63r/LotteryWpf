using LotteryWpf.Content;
using LotteryWpf.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace LotteryWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // モジュールの登録
            moduleCatalog.AddModule<ContentModule>();
        }
    }
}
