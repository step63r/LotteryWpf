using Prism.Mvvm;

namespace LotteryWpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "LotteryWpf";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
