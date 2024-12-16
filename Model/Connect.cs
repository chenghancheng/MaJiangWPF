using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TouchSocket;

using System.Net.WebSockets;
using System.Threading;

public class WebSocketClient
{
    private ClientWebSocket _webSocket;
    private Uri _serverUri;
    private bool isConnect;
    // 定义一个事件，用来触发消息接收
    public event EventHandler<string> MessageReceived;

    public WebSocketClient(string serverUri)
    {
        _serverUri = new Uri(serverUri);
        _webSocket = new ClientWebSocket();
        isConnect = false;
    }

    public async Task StartAsync()
    {
        try
        {
            isConnect = true;
            // 连接到 WebSocket 服务器
            await _webSocket.ConnectAsync(_serverUri, CancellationToken.None);
            Console.WriteLine("已连接到服务器，等待输入...");

            // 启动接收消息的任务
            _ = ReceiveMessagesAsync();

            // 主线程负责等待用户输入并发送消息
            //await SendMessagesAsyncs(message);
        }
        catch (Exception ex)
        {
            isConnect = false;
            Console.WriteLine($"连接失败: {ex.Message}");
        }
    }

    public async Task SendMessagesAsyncs(string message)
    {
        // 循环等待用户输入并发送消息
        if (_webSocket.State == WebSocketState.Open)
        {
            if (string.IsNullOrEmpty(message)) return;

            // 将消息转换为字节数组
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            // 发送消息
            await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("消息已发送: " + message);
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024];

        try
        {
            // 持续接收服务器的消息
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("收到消息: " + receivedMessage);
                    // 当消息接收完成时，触发自定义事件
                    OnMessageReceived(receivedMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"接收消息时发生错误: {ex.Message}");
        }
    }

    // 触发自定义事件
    protected virtual void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(this, message);
    }

    public async Task CloseAsync()
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "客户端关闭连接", CancellationToken.None);
            isConnect = false;
            Console.WriteLine("WebSocket 连接已关闭");
        }
    }
}

public class Connect
{
    //public string url = "ws://localhost:8088";
    public string url="ws://10.29.61.159:8088";
    public static WebSocketClient ws;

    public Connect()
    {
        ws = new WebSocketClient(url);
    }

}

