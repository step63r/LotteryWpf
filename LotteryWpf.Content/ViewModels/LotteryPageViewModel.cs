﻿using LotteryWpf.Common;
using LotteryWpf.Content.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace LotteryWpf.Content.ViewModels
{
    public class LotteryPageViewModel : BindableBase, IRegionMemberLifetime
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

        /// <summary>
        /// 戻るコマンド
        /// </summary>
        public DelegateCommand GoBackCommand { get; private set; }

        private string _currentUserName;
        /// <summary>
        /// 現在の抽選者名
        /// </summary>
        public string CurrentUserName
        {
            get { return _currentUserName; }
            set { SetProperty(ref _currentUserName, value); }
        }

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
        
        /// <summary>
        /// インスタンスを使い回すか
        /// </summary>
        public bool KeepAlive { get; } = false;
        #endregion

        /// <summary>
        /// 設定情報ファイルパス
        /// </summary>
        private static string _configPath = string.Format(@"{0}\{1}", Path.BaseDir, Path.ConfigFileName);

        /// <summary>
        /// セッション情報
        /// </summary>
        private SessionInfo _sessionInfo;

        /// <summary>
        /// WPFタイマ
        /// </summary>
        private DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// 残っている賞品一覧
        /// </summary>
        private List<string> _remainedPrizes = new List<string>();

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
            _eventAggregator.GetEvent<MessageSentEvent>().Subscribe(CurrentUserNameReceived);

            // コマンドを定義
            StopCommand = new DelegateCommand(ExecuteStopCommand, CanExecuteStopCommand);
            StopCommand.ObservesProperty(() => IsStopped);
            GoBackCommand = new DelegateCommand(ExecuteGoBackCommand);

            // xmlから残った賞品を取得
            _sessionInfo = XmlConverter.DeSerialize<SessionInfo>(_configPath);
            var exceptList = new List<string>();
            foreach (var result in _sessionInfo.LotteryResults)
            {
                exceptList.Add(result.PrizeName);
            }
            // TODO: 同じ文字列があった場合の対策
            _remainedPrizes = _sessionInfo.Prizes.Except(exceptList).ToList();

            // 抽選開始
            Loaded();
        }

        /// <summary>
        /// 抽選者名取得イベント
        /// </summary>
        /// <param name="message"></param>
        private void CurrentUserNameReceived(string message)
        {
            CurrentUserName = message;
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
            _sessionInfo.LotteryResults.Add(new LotteryResult()
            {
                UserName = CurrentUserName,
                PrizeName = CurrentPrize
            });
            XmlConverter.Serialize(_sessionInfo, _configPath);
        }

        private bool CanExecuteStopCommand()
        {
            return !_isStopped;
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
