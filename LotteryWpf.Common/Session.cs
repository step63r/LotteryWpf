namespace LotteryWpf.Common
{
    /// <summary>
    /// セッション情報クラス
    /// </summary>
    public class Session
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string Guid { get; set; } = "";
        /// <summary>
        /// 名前
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 賞品名
        /// </summary>
        public Prize Prize { get; set; }
    }
}
