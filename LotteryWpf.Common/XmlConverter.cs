using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LotteryWpf.Common
{
    /// <summary>
    /// XMLシリアライザ／デシリアライザ
    /// </summary>
    public static class XmlConverter
    {
        /// <summary>
        /// 排他制御のためのセマフォオブジェクト
        /// </summary>
        static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 任意のオブジェクトをxmlにシリアル化する（非同期）
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns></returns>
        public static async Task SerialzeAsync<T>(T obj, string filePath)
        {
            // ロックを取得
            await _semaphore.WaitAsync();

            try
            {
                using (var fs = new StreamWriter(filePath, false, Encoding.GetEncoding("utf-8")))
                {
                    await Task.Run(() =>
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(fs, obj);
                    });
                    // .NET Framework 4.5以降
                    await fs.FlushAsync();
                }
            }
            finally
            {
                // ロックを解放
                _semaphore.Release();
            }
        }

        /// <summary>
        /// xmlファイルを任意のオブジェクトに逆シリアル化する（非同期）
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <returns></returns>
        public static async Task<T> DeserializeAsync<T>(string filePath)
        {
            // ロックを取得
            await _semaphore.WaitAsync();
            try
            {
                using (var fs = new StreamReader(filePath, Encoding.GetEncoding("utf-8")))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return await Task.Run(() => (T)serializer.Deserialize(fs));
                }
            }
            finally
            {
                // ロックを解放
                _semaphore.Release();
            }
        }

        #region 同期型
        /// <summary>
        /// 任意のオブジェクトをxmlにシリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="Path">ファイルパス</param>
        /// <returns>成功したらtrue、失敗したらfalse</returns>
        public static bool Serialize<T>(T obj, string Path) where T : class
        {
            bool ret = false;
            try
            {
                using (var fs = new StreamWriter(Path, false, Encoding.GetEncoding("utf-8")))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(fs, obj);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 任意のオブジェクトをxmlにシリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="Path">ファイルパス</param>
        /// <returns>成功したらtrue、失敗したらfalse</returns>
        public static bool SerializeFromCol<T>(ObservableCollection<T> obj, string Path) where T : class
        {
            bool ret = false;
            try
            {
                using (var fs = new StreamWriter(Path, false, Encoding.GetEncoding("utf-8")))
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<T>));
                    serializer.Serialize(fs, obj);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// xmlファイルを任意のオブジェクトに逆シリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="Path">ファイルパス</param>
        /// <returns>オブジェクト</returns>
        public static T DeSerialize<T>(string Path) where T : class
        {
            var ret = default(T);
            try
            {
                using (var fs = new StreamReader(Path, Encoding.GetEncoding("utf-8")))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    ret = (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// xmlファイルを任意のオブジェクトに逆シリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="Path">ファイルパス</param>
        /// <returns>オブジェクト</returns>
        public static ObservableCollection<T> DeSerializeToCol<T>(string Path) where T : class
        {
            var ret = default(ObservableCollection<T>);
            try
            {
                using (var fs = new StreamReader(Path, Encoding.GetEncoding("utf-8")))
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<T>));
                    ret = (ObservableCollection<T>)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ret;
        }
        #endregion
    }
}
