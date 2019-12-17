using LotteryWpf.Common;
using LotteryWpf.Content.Services;
using LotteryWpf.Content.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Media;
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

        private Prize _currentPrize;
        /// <summary>
        /// 現在の賞品
        /// </summary>
        public Prize CurrentPrize
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

        private bool _isVisibleStoryboard = false;
        /// <summary>
        /// ストーリーボードのGridを表示するか
        /// </summary>
        public bool IsVisibleStoryboard
        {
            get { return _isVisibleStoryboard; }
            set { SetProperty(ref _isVisibleStoryboard, value); }
        }

        private bool _isVisibleBigPanel = false;
        /// <summary>
        /// 確定演出用の大きいパネルを表示するか
        /// </summary>
        public bool IsVisibleBigPanel
        {
            get { return _isVisibleBigPanel; }
            set { SetProperty(ref _isVisibleBigPanel, value); }
        }

        private Brush _bigPanelBackground = new SolidColorBrush(Colors.Transparent);
        /// <summary>
        /// 確定演出用の大きいパネルの背景色
        /// </summary>
        public Brush BigPanelBackground
        {
            get { return _bigPanelBackground; }
            set { SetProperty(ref _bigPanelBackground, value); }
        }

        /// <summary>
        /// インスタンスを使い回すか
        /// </summary>
        public bool KeepAlive { get; } = false;
        #endregion

        /// <summary>
        /// WPFタイマ
        /// </summary>
        private DispatcherTimer _dispatcherTimer;
        /// <summary>
        /// 残っている賞品一覧
        /// </summary>
        private List<Prize> _remainedPrizes = new List<Prize>();
        /// <summary>
        /// サウンドプレイヤー（DirectXが入っていないので多重再生は不可）
        /// </summary>
        private SoundPlayer _player;

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

            // 残った賞品を取得
            var exceptList = new List<Prize>();
            foreach (var session in SessionsDataStore.GetSessions())
            {
                exceptList.Add(session.Prize);
            }

            foreach (var prize in PrizesDataStore.GetPrizes())
            {
                if (exceptList.Where(item => item.Guid == prize.Guid).FirstOrDefault() == null)
                {
                    _remainedPrizes.Add(prize);
                }
            }

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

            // ドラムロール開始
            var stream = Properties.Resources.nc90552;
            _player = new SoundPlayer(stream);
            _player.PlayLooping();
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

        private async Task RunSpecialEffectAsync(Brush backgroundBrush)
        {
            await Task.Run(() =>
            {
                // ストーリーボード表示
                IsVisibleStoryboard = true;
                var stream = Properties.Resources.nc179911;
                _player = new SoundPlayer(stream);
                _player.PlaySync();
                IsVisibleStoryboard = false;

                // 大きいパネル表示
                BigPanelBackground = backgroundBrush;
                IsVisibleBigPanel = true;
                stream = Properties.Resources.fanfare1;
                _player = new SoundPlayer(stream);

                _player.PlaySync();
                IsVisibleBigPanel = false;
            });
        }

        /// <summary>
        /// 抽選をストップする
        /// </summary>
        private void ExecuteStopCommand()
        {
            _dispatcherTimer.Stop();
            IsStopped = true;
            _player.Stop();

            if (CurrentPrize.EnableFinalProduction)
            {
                // 確定演出表示
                _ = RunSpecialEffectAsync(new SolidColorBrush(Colors.LightGoldenrodYellow));
                //_ = RunSpecialEffectAsync(new SolidColorBrush(Colors.WhiteSmoke));
                //_ = RunSpecialEffectAsync(new SolidColorBrush(Colors.AntiqueWhite));
            }

            else
            {
                var stream = Properties.Resources.roll_finish1;
                _player = new SoundPlayer(stream);
                _player.Play();
            }

            // 抽選結果をxmlに保存
            SessionsDataStore.AddSession(new Session()
            {
                Guid = Guid.NewGuid().ToString(),
                UserName = CurrentUserName,
                Prize = CurrentPrize
            });
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
            _dispatcherTimer.Stop();
            IsStopped = true;
            _player.Stop();
            _regionManager.RequestNavigate("ContentRegion", nameof(TopPage));
        }
    }
}
