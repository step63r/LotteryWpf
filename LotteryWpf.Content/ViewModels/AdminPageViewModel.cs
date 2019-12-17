using LotteryWpf.Common;
using LotteryWpf.Content.Services;
using LotteryWpf.Content.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace LotteryWpf.Content.ViewModels
{
    public class AdminPageViewModel : BindableBase
    {
        #region インタフェース
        /// <summary>
        /// RegionManager
        /// </summary>
        private readonly IRegionManager _regionManager;
        /// <summary>
        /// EventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region コマンド・プロパティ
        /// <summary>
        /// 登録コマンド
        /// </summary>
        public DelegateCommand RegisterCommand { get; private set; }

        /// <summary>
        /// 戻るコマンド
        /// </summary>
        public DelegateCommand GoBackCommand { get; private set; }

        private Prize _inputPrize = new Prize();
        /// <summary>
        /// 賞品名
        /// </summary>
        public Prize InputPrize
        {
            get { return _inputPrize; }
            set { SetProperty(ref _inputPrize, value); }
        }
        #endregion

        /// <summary>
        /// 賞品リスト
        /// </summary>
        public ObservableCollection<Prize> Prizes { get; set; } = new ObservableCollection<Prize>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        public AdminPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            // インタフェースを受け取る
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            // コマンドを定義
            RegisterCommand = new DelegateCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
            RegisterCommand.ObservesProperty(() => InputPrize);
            GoBackCommand = new DelegateCommand(ExecuteGoBackCommand);

            foreach (var prize in PrizesDataStore.GetPrizes())
            {
                Prizes.Add(prize);
            }
        }

        /// <summary>
        /// 登録を実行する
        /// </summary>
        private void ExecuteRegisterCommand()
        {
            var prize = new Prize()
            {
                Guid = Guid.NewGuid().ToString(),
                Name = InputPrize.Name,
                EnableFinalProduction = InputPrize.EnableFinalProduction,
                FinalProductionBackgroud = ""
            };
            PrizesDataStore.AddPrize(prize);
            Prizes.Add(prize);
            InputPrize = new Prize();
        }

        /// <summary>
        /// 登録が実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteRegisterCommand()
        {
            return !string.IsNullOrEmpty(InputPrize.Name);
        }

        /// <summary>
        /// 戻る
        /// </summary>
        private void ExecuteGoBackCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(TopPage));
        }
    }
}
