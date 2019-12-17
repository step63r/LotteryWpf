using LotteryWpf.Common;

namespace LotteryWpf.Content.Services
{
    /// <summary>
    /// 設定情報管理クラス
    /// </summary>
    public static class ConfigDataStore
    {
        /// <summary>
        /// 設定情報
        /// </summary>
        private static Config _config;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private static string _filePath = string.Format(@"{0}\{1}", Path.BaseDir, Path.ConfigFileName);

        /// <summary>
        /// 初期化処理
        /// </summary>
        public static void Initialize()
        {
            CreateFileIfNotExists();
            _config = XmlConverter.DeSerialize<Config>(_filePath);
        }

        /// <summary>
        /// 管理者名を取得する
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            return _config.UserName;
        }

        /// <summary>
        /// 管理者名を設定する
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static void SetUserName(string userName)
        {
            _config.UserName = userName;
            XmlConverter.Serialize(_config, _filePath);
        }

        /// <summary>
        /// セッションクリアコマンドを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetClearSessionsCommand()
        {
            return _config.ClearSessionsCommand;
        }

        /// <summary>
        /// セッションクリアコマンドを設定する
        /// </summary>
        /// <param name="clearSessionCommand"></param>
        /// <returns></returns>
        public static void SetClearSessionsCommand(string clearSessionCommand)
        {
            _config.ClearSessionsCommand = clearSessionCommand;
            XmlConverter.Serialize(_config, _filePath);
        }

        /// <summary>
        /// 当選結果クリアコマンドを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetClearPrizesCommand()
        {
            return _config.ClearPrizesCommand;
        }

        /// <summary>
        /// 当選結果クリアコマンドを設定する
        /// </summary>
        /// <param name="clearResultsCommand"></param>
        public static void SetClearPrizesCommand(string clearResultsCommand)
        {
            _config.ClearPrizesCommand = clearResultsCommand;
            XmlConverter.Serialize(_config, _filePath);
        }

        /// <summary>
        /// ファイルがない場合作成する
        /// </summary>
        private static void CreateFileIfNotExists()
        {
            // ディレクトリ取得
            var dirInfo = System.IO.Path.GetDirectoryName(_filePath);
            System.IO.Directory.CreateDirectory(dirInfo);

            // ファイルが存在しなければ作る
            if (!System.IO.File.Exists(_filePath))
            {
                _config = new Config();
                XmlConverter.Serialize(_config, _filePath);
            }
        }
    }
}
