using LotteryWpf.Common;
using LotteryWpf.Content.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
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

        private string _inputPrize;
        /// <summary>
        /// 賞品名
        /// </summary>
        public string InputPrize
        {
            get { return _inputPrize; }
            set { SetProperty(ref _inputPrize, value); }
        }
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
        /// 賞品リスト
        /// </summary>
        public ObservableCollection<string> Prizes { get; set; } = new ObservableCollection<string>();

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

            // xmlを読み込み
            _sessionInfo = XmlConverter.DeSerialize<SessionInfo>(_configPath);
            if (_sessionInfo != null)
            {
                foreach (string prize in _sessionInfo.Prizes)
                {
                    Prizes.Add(prize);
                }
            }
            else
            {
                // なかったら作っておく
                _sessionInfo = new SessionInfo()
                {
                    LotteryResults = new List<LotteryResult>(),
                    Prizes = new List<string>()
                };
                XmlConverter.Serialize(_sessionInfo, _configPath);
            }
        }

        /// <summary>
        /// 登録を実行する
        /// </summary>
        private void ExecuteRegisterCommand()
        {
            _sessionInfo.Prizes.Add(InputPrize);
            bool ret = XmlConverter.Serialize(_sessionInfo, _configPath);

            if (ret)
            {
                Prizes.Add(InputPrize);
                InputPrize = "";
            }
        }

        /// <summary>
        /// 登録が実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteRegisterCommand()
        {
            return !string.IsNullOrEmpty(InputPrize);
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
