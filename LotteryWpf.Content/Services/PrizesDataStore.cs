using LotteryWpf.Common;
using System.Collections.Generic;
using System.Linq;

namespace LotteryWpf.Content.Services
{
    /// <summary>
    /// 賞品一覧管理クラス
    /// </summary>
    public static class PrizesDataStore
    {
        /// <summary>
        /// 賞品一覧
        /// </summary>
        private static List<Prize> _prizes;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private static string _filePath = string.Format(@"{0}\{1}", Path.BaseDir, Path.PrizesFileName);

        /// <summary>
        /// 初期化処理
        /// </summary>
        public static void Initialize()
        {
            CreateFileIfNotExists();
            _prizes = XmlConverter.DeSerialize<List<Prize>>(_filePath);
        }

        /// <summary>
        /// 賞品一覧を取得する
        /// </summary>
        /// <returns></returns>
        public static List<Prize> GetPrizes()
        {
            return _prizes;
        }

        /// <summary>
        /// 賞品をGUIDで取得する
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Prize GetPrize(string guid)
        {
            return _prizes.Where(item => item.Guid == guid).FirstOrDefault();
        }

        /// <summary>
        /// 賞品を登録する
        /// </summary>
        /// <param name="prize"></param>
        public static void AddPrize(Prize prize)
        {
            _prizes.Add(prize);
            XmlConverter.Serialize(_prizes, _filePath);
        }

        /// <summary>
        /// 賞品を変更する
        /// </summary>
        /// <param name="prize"></param>
        public static void UpdatePrize(Prize prize)
        {
            var targetPrize = _prizes.Where(item => item.Guid == prize.Guid).FirstOrDefault();
            targetPrize = prize;
            XmlConverter.Serialize(_prizes, _filePath);
        }

        /// <summary>
        /// 賞品を削除する
        /// </summary>
        /// <param name="prize"></param>
        public static void RemovePrize(Prize prize)
        {
            _prizes.Remove(prize);
            XmlConverter.Serialize(_prizes, _filePath);
        }

        /// <summary>
        /// 賞品を削除する
        /// </summary>
        /// <param name="guid"></param>
        public static void RemovePrize(string guid)
        {
            var targetPrize = _prizes.Where(item => item.Guid == guid).FirstOrDefault();
            _prizes.Remove(targetPrize);
            XmlConverter.Serialize(_prizes, _filePath);
        }

        /// <summary>
        /// 賞品を全て削除する
        /// </summary>
        public static void Clear()
        {
            _prizes.Clear();
            XmlConverter.Serialize(_prizes, _filePath);
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
                _prizes = new List<Prize>();
                XmlConverter.Serialize(_prizes, _filePath);
            }
        }
    }
}
