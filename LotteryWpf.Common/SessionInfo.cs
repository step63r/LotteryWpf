using System.Collections.Generic;

namespace LotteryWpf.Common
{
    /// <summary>
    /// セッション情報クラス
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// これまでの抽選結果
        /// </summary>
        public List<LotteryResult> LotteryResults { get; set; }
        /// <summary>
        /// 賞品一覧
        /// </summary>
        public List<string> Prizes { get; set; }
    }
}
