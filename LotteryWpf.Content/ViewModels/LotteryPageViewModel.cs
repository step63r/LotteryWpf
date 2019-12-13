using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace LotteryWpf.Content.ViewModels
{
    public class LotteryPageViewModel : BindableBase
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
        /// ストップコマンド
        /// </summary>
        public DelegateCommand StopCommand { get; private set; }

        private string _currentPrize;
        /// <summary>
        /// 現在の賞品
        /// </summary>
        public string CurrentPrize
        {
            get { return _currentPrize; }
            set { SetProperty(ref _currentPrize, value); }
        }

        private bool _isStopped = false;
        /// <summary>
        /// ストップフラグ
        /// </summary>
        public bool IsStopped
        {
            get { return _isStopped; }
            set { SetProperty(ref _isStopped, value); }
        }
        #endregion

        /// <summary>
        /// WPFタイマ
        /// </summary>
        private DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// 残っている賞品一覧
        /// </summary>
        private List<string> _remainedPrizes = new List<string>() { "aaa", "bbb", "ccc", "ddd", "eee" };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        public LotteryPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            // インタフェースを受け取る
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            // コマンドを定義
            StopCommand = new DelegateCommand(ExecuteStopCommand, CanExecuteStopCommand);
            StopCommand.ObservesProperty(() => IsStopped);

            // xmlから残った賞品を取得
            // TODO

            // 抽選開始
            Loaded();
        }

        /// <summary>
        /// 読込完了時イベント
        /// </summary>
        private void Loaded()
        {
            // タイマ設定
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _dispatcherTimer.Tick += new EventHandler(_dispatcherTimer_Tick);
            _dispatcherTimer.Start();
        }

        /// <summary>
        /// タイマイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var random = new Random();
            int index = random.Next(_remainedPrizes.Count);
            CurrentPrize = _remainedPrizes[index];
        }

        /// <summary>
        /// 抽選をストップする
        /// </summary>
        private void ExecuteStopCommand()
        {
            _dispatcherTimer.Stop();
            IsStopped = true;

            // 抽選結果をxmlに保存
            // TODO
        }

        private bool CanExecuteStopCommand()
        {
            return !_isStopped;
        }
    }
}
