namespace LotteryWpf.Common
{
    /// <summary>
    /// 設定情報クラス
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 管理者名
        /// </summary>
        public string UserName { get; set; } = "admin";
        /// <summary>
        /// セッションクリアコマンド
        /// </summary>
        public string ClearSessionsCommand { get; set; } = "sessions/clear";
        /// <summary>
        /// 賞品一覧クリアコマンド
        /// </summary>
        public string ClearPrizesCommand { get; set; } = "prizes/clear";
    }
}
