using LotteryWpf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryWpf.Content.Services
{
    /// <summary>
    /// セッション情報管理クラス
    /// </summary>
    public static class SessionsDataStore
    {
        /// <summary>
        /// セッション情報
        /// </summary>
        private static List<Session> _sessions;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private static string _filePath = string.Format(@"{0}\{1}", Path.BaseDir, Path.SessionsFileName);

        /// <summary>
        /// 初期化処理
        /// </summary>
        public static void Initialize()
        {
            CreateFileIfNotExists();
            _sessions = XmlConverter.DeSerialize<List<Session>>(_filePath);
        }

        /// <summary>
        /// セッション情報一覧を取得する
        /// </summary>
        /// <returns></returns>
        public static List<Session> GetSessions()
        {
            return _sessions;
        }

        /// <summary>
        /// セッション情報をGUIDで取得する
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Session GetSession(string guid)
        {
            return _sessions.Where(item => item.Guid == guid).FirstOrDefault();
        }

        /// <summary>
        /// セッションを登録する
        /// </summary>
        /// <param name="session"></param>
        public static void AddSession(Session session)
        {
            _sessions.Add(session);
            XmlConverter.Serialize(_sessions, _filePath);
        }

        /// <summary>
        /// セッションを変更する
        /// </summary>
        /// <param name="session"></param>
        public static void UpdateSession(Session session)
        {
            var targetSession = _sessions.Where(item => item.Guid == session.Guid).FirstOrDefault();
            targetSession = session;
            XmlConverter.Serialize(_sessions, _filePath);
        }

        /// <summary>
        /// セッションを削除する
        /// </summary>
        /// <param name="session"></param>
        public static void RemoveSession(Session session)
        {
            var targetSession = _sessions.Where(item => item.Guid == session.Guid).FirstOrDefault();
            _sessions.Remove(targetSession);
            XmlConverter.Serialize(_sessions, _filePath);
        }

        /// <summary>
        /// セッション情報を全て削除する
        /// </summary>
        public static void Clear()
        {
            _sessions.Clear();
            XmlConverter.Serialize(_sessions, _filePath);
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
                _sessions = new List<Session>();
                XmlConverter.Serialize(_sessions, _filePath);
            }
        }
    }
}
