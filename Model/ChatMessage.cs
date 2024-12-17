using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


public class ChatMessage
{
    // 消息类型，使用 int 来表示
    public int MessageType { get; set; }

    public int SenderId { get; set; }

    // 发送人
    public string Sender { get; set; }

    // 接收人
    public string Receiver { get; set; }

    // 额外消息，可能是文本、文件路径、图片等
    public int ExtraMessage { get; set; }

    public List<int> ChiCards { get; set; }

    public HashSet<int> HandCards { get; set; }

    public ChatMessage()
    {
        ChiCards = new List<int>();
        HandCards = new HashSet<int>();
    }

    // 构造函数
    public ChatMessage(int messageType, string sender, string receiver, int extraMessage)
    {
        MessageType = messageType;
        Sender = sender;
        Receiver = receiver;
        ExtraMessage = extraMessage;
        ChiCards = new List<int>();
        HandCards = new HashSet<int>();
    }

    public ChatMessage(int messageType, string sender, string receiver, int extraMessage, List<int> chiCards)
    {
        MessageType = messageType;
        Sender = sender;
        Receiver = receiver;
        ExtraMessage = extraMessage;
        ChiCards = new List<int>();
        ChiCards.AddRange(chiCards);
        HandCards = new HashSet<int>();
    }

    // 转换为 JSON 字符串
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    // 从 JSON 字符串反序列化为 ChatMessage 对象
    public static ChatMessage FromJson(string json)
    {
        return JsonConvert.DeserializeObject<ChatMessage>(json);
    }
}

public class ResponseMessage
{
    public int MessageType { get; set; }  // 消息类型，使用 MessageType 枚举
    public int Sender { get; set; }    // 发送者
    public int Receiver { get; set; }  // 接收者
    public Content content { get; set; }   // 消息内容
    public DateTime Timestamp { get; set; }  // 时间戳

    public ResponseMessage()
    {
        content = new Content();
        Timestamp = DateTime.Now;
    }

    // 构造函数
    public ResponseMessage(int messageType, int sender, int receiver)
    {
        MessageType = messageType;
        Sender = sender;
        Receiver = receiver;
        content = new Content();
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


public class Content
{
    public List<HashSet<int>> handCard;
    public int getCard;
    public int disCard;
    public List<string> playerName;
    public List<int> chiCard;
    public int remainCards;
    public int curPlayer;
    public int chiPengGangPlayerId;
    public int winner;
    public int loser;
    public int finishType;


    public Content()
    {
        handCard = new List<HashSet<int>>();
        playerName = new List<string>();
        chiCard = new List<int> { };
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static Content FromJson(string json)
    {
        return JsonConvert.DeserializeObject<Content>(json);
    }
}



public enum MessageType
{
    Ready = 0,
    Discard = 1,
    Chi = 2,
    Peng = 3,

    Gang_Zimo = 4,
    Gang_JiaGang = 5,
    Gang_FromOthers = 6,

    Win_Zimo = 7,
    Win_DianPao = 8,

    Liuju = 9,
    StartGame = 10,
    Pass = 11,
    GetCard = 12,

    Stop=13,
    Win=100
}


