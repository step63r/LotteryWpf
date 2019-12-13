using LotteryWpf.Common;
using LotteryWpf.Content.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LotteryWpf.Content.ViewModels
{
    public class HistoryPageViewModel : BindableBase, IRegionMemberLifetime
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
        /// 戻るコマンド
        /// </summary>
        public DelegateCommand GoBackCommand { get; private set; }

        /// <summary>
        /// 当選結果一覧
        /// </summary>
        public ObservableCollection<LotteryResult> LotteryResults { get; set; } = new ObservableCollection<LotteryResult>();

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
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        public HistoryPageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            // インタフェースを受け取る
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            // コマンドを定義
            GoBackCommand = new DelegateCommand(ExecuteGoBackCommand);

            // セッション情報取得
            _sessionInfo = XmlConverter.DeSerialize<SessionInfo>(_configPath);
            foreach (var result in _sessionInfo.LotteryResults)
            {
                LotteryResults.Add(result);
            }
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
