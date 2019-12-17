using System;

namespace LotteryWpf.Common
{
    /// <summary>
    /// 賞品クラス
    /// </summary>
    public class Prize
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string Guid { get; set; } = "";
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 確定演出を使うか
        /// </summary>
        public bool EnableFinalProduction { get; set; } = false;
        /// <summary>
        /// 確定演出時の背景色
        /// </summary>
        public string FinalProductionBackgroud { get; set; } = "";
    }
}
