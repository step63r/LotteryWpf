using LotteryWpf.Common;
using LotteryWpf.Content.Services;
using LotteryWpf.Content.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;

namespace LotteryWpf.Content.ViewModels
{
    public class TopPageViewModel : BindableBase, IRegionMemberLifetime
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
        /// 開始コマンド
        /// </summary>
        public DelegateCommand StartCommand { get; private set; }

        /// <summary>
        /// 当選履歴確認コマンド
        /// </summary>
        public DelegateCommand CheckHistoryCommand { get; private set; }

        private string _currentUserName = "";
        /// <summary>
        /// 現在の抽選者名
        /// </summary>
        public string CurrentUserName
        {
            get { return _currentUserName; }
            set { SetProperty(ref _currentUserName, value); }
        }

        private int _remainCount;
        /// <summary>
        /// 残り賞品数
        /// </summary>
        public int RemainCount
        {
            get { return _remainCount; }
            set { SetProperty(ref _remainCount, value); }
        }

        private bool _isRemainValid;
        /// <summary>
        /// 残り賞品数が妥当か
        /// </summary>
        public bool IsRemainValid
        {
            get { return _isRemainValid; }
            set { SetProperty(ref _isRemainValid, value); }
        }

        /// <summary>
        /// インスタンスを使い回すか
        /// </summary>
        public bool KeepAlive { get; } = false;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        public TopPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            // インタフェースを受け取る
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            // コマンドを定義
            StartCommand = new DelegateCommand(ExecuteStartCommand, CanExecuteStartCommand);
            StartCommand.ObservesProperty(() => CurrentUserName);
            StartCommand.ObservesProperty(() => RemainCount);
            CheckHistoryCommand = new DelegateCommand(ExecuteCheckHistoryCommand, CanExecuteCheckHistoryCommand);

            // 初期処理
            ConfigDataStore.Initialize();
            PrizesDataStore.Initialize();
            SessionsDataStore.Initialize();

            // 残り賞品数
            UpdateRemainCount();
        }

        /// <summary>
        /// 残り賞品数を取得する
        /// </summary>
        private void UpdateRemainCount()
        {
            RemainCount = PrizesDataStore.GetPrizes().Count - SessionsDataStore.GetSessions().Count;
            IsRemainValid = RemainCount <= 0;
        }

        /// <summary>
        /// 抽選を開始する
        /// </summary>
        private void ExecuteStartCommand()
        {
            // コマンド判定
            if (CurrentUserName.Equals(ConfigDataStore.GetUserName()))
            {
                _regionManager.RequestNavigate("ContentRegion", nameof(AdminPage));
            }
            else if (CurrentUserName.Equals(ConfigDataStore.GetClearSessionsCommand()))
            {
                SessionsDataStore.Clear();
                UpdateRemainCount();
            }
            else if (CurrentUserName.Equals(ConfigDataStore.GetClearPrizesCommand()))
            {
                PrizesDataStore.Clear();
                UpdateRemainCount();
            }
            else
            {
                _regionManager.RequestNavigate("ContentRegion", nameof(LotteryPage));
                _eventAggregator.GetEvent<MessageSentEvent>().Publish(CurrentUserName);
            }
            CurrentUserName = "";
        }

        /// <summary>
        /// 抽選が開始可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteStartCommand()
        {
            return (!string.IsNullOrEmpty(CurrentUserName) && RemainCount > 0) || 
                CurrentUserName.Equals(ConfigDataStore.GetUserName()) ||
                CurrentUserName.Equals(ConfigDataStore.GetClearPrizesCommand()) ||
                CurrentUserName.Equals(ConfigDataStore.GetClearSessionsCommand());
        }

        /// <summary>
        /// 当選履歴確認を実行する
        /// </summary>
        private void ExecuteCheckHistoryCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(HistoryPage));
        }

        /// <summary>
        /// 当選履歴確認が実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteCheckHistoryCommand()
        {
            return true;
        }
    }
}
