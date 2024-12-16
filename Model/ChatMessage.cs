using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChatMessage//自己发送的
{
    // 消息类型，使用 int 来表示
    public int MessageType { get; set; }

    // 发送人
    public string Sender { get; set; }

    // 接收人
    public string Receiver { get; set; }

    // 额外消息，可能是文本、文件路径、图片等
    public int ExtraMessage { get; set; }

    // 构造函数
    public ChatMessage(int messageType, string sender, string receiver, int extraMessage)
    {
        MessageType = messageType;
        Sender = sender;
        Receiver = receiver;
        ExtraMessage = extraMessage;
    }

    // 转换为 JSON 字符串
    public string ToJson()
    {
        return $"{{\"messageType\":{MessageType},\"sender\":\"{Sender}\",\"receiver\":{Receiver},\"extraMessage\":\"{ExtraMessage}\"}}";
    }

    // 从 JSON 字符串反序列化为 ChatMessage 对象
    public static ChatMessage FromJson(string json)
    {
        // 这里简单解析，可以使用 JSON 库（如 Json.NET）来进行更复杂的处理
        var parts = json.Trim('{', '}').Split(',');
        var messageType = int.Parse(parts[0].Split(':')[1]);
        var sender = parts[1].Split(':')[1].Trim('"');
        var receiver = parts[2].Split(':')[1].Trim('"');
        var extraMessage = int.Parse(parts[3].Split(':')[1].Trim('"'));

        return new ChatMessage(messageType, sender, receiver, extraMessage);
    }
}

public class ResponseMessage//接收到的
{
    public int MessageType { get; set; }  // 消息类型，使用 MessageType 枚举
    public string Sender { get; set; }    // 发送者
    public string Receiver { get; set; }  // 接收者
    public string Content { get; set; }   // 消息内容
    public DateTime Timestamp { get; set; }  // 时间戳

    public ResponseMessage()
    {
        Timestamp = DateTime.Now;
    }

    // 构造函数
    public ResponseMessage(int messageType, string sender, string receiver, string content)
    {
        MessageType = messageType;
        Sender = sender;
        Receiver = receiver;
        Content = content;
        Timestamp = DateTime.UtcNow;
    }

    // 转换为 JSON 字符串
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    // 从 JSON 字符串反序列化为 ResponseMessage 对象
    public static ResponseMessage FromJson(string json)
    {
        return JsonConvert.DeserializeObject<ResponseMessage>(json);
    }
}

public enum MessageType
{
    StartGame = 0,
    Discard = 1,
    Chi = 2,
    Peng = 3,

    Gang_Zimo = 4,
    Gang_JiaGang = 5,
    Gang_FromOthers = 6,

    Win_Zimo = 7,
    Win_DianPao = 8,

    Liuju = 9
}
