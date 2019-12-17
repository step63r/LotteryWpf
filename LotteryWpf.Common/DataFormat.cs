using System;

namespace LotteryWpf.Common
{
    /// <summary>
    /// パス設定
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// 基底ディレクトリ
        /// </summary>
        public static string BaseDir = string.Format(@"{0}\LotteryWpf", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        /// <summary>
        /// セッションファイル名
        /// </summary>
        public static string SessionsFileName = @"sessions.xml";
        /// <summary>
        /// 景品一覧ファイル名
        /// </summary>
        public static string PrizesFileName = @"prizes.xml";
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        public static string ConfigFileName = @"config.xml";
    }
}
