using Prism.Events;

namespace LotteryWpf.Common
{
    /// <summary>
    /// 文字列型のパラメータを送受信するイベントクラス
    /// </summary>
    public class MessageSentEvent : PubSubEvent<string>
    {

    }
}
