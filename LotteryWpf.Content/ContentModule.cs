using LotteryWpf.Content.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace LotteryWpf.Content
{
    public class ContentModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionMan = containerProvider.Resolve<IRegionManager>();
            regionMan.RegisterViewWithRegion("ContentRegion", typeof(TopPage));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LotteryPage>();
        }
    }
}