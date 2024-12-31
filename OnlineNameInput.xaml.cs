using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MaJiangApp;

namespace Majiang
{

    /// <summary>
    /// OnlineNameInput.xaml 的交互逻辑
    /// </summary>
    public partial class OnlineNameInput : Page
    {
        private bool isMatching;
        private int serial;
        public OnlineNameInput()
        {
            isMatching = false;
            serial = -1;
            InitializeComponent();
            matchingLabel.Visibility = Visibility.Hidden;
        }

        // 在点击返回时，断开连接并确保连接被重置
        private async void BackToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            // 断开连接
            await Connect.ws.CloseAsync();

            // 重置页面状态
            Reset();

            // 获取父窗口中的 Frame 控件，跳转到主页面
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.mainPage);
        }

        // 在点击开始时，确保 ws 重新启动并发送消息
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isMatching || inputName.Text.Trim() == "")
            {
                return;
            }

            Connect.reset();

            isMatching = true;
            matchingLabel.Visibility = Visibility.Visible;

            // 确保 WebSocket 连接已经建立
            await Connect.ws.StartAsync();  // 如果没有连接，则启动
            // 创建并发送连接消息
            ChatMessage chatMessage = new ChatMessage(0, inputName.Text, "", 0);
            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());

            // 禁用开始按钮的视觉效果
            start.Opacity = 0.5;
            start.Foreground = new SolidColorBrush(Colors.Gray);

            // 设置游戏名称
            GamePage.name = inputName.Text;

            // 监听消息接收
            Connect.ws.MessageReceived += OnMessageReceived;
        }

        // 处理接收到的消息
        private void OnMessageReceived(object sender, string message)
        {
            ResponseMessage responseMessage = ResponseMessage.FromJson(message);
            if (responseMessage.MessageType == 0)
            {
                if (serial == -1)
                {
                    serial = responseMessage.Receiver;
                }

                if (responseMessage.Receiver == 3)
                {
                    // 获取父窗口中的 Frame 控件，跳转到游戏页面
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.MainFrame.Navigate(new GamePage(false, serial));

                    // 重置状态
                    Reset();
                }

                matchingLabel.Content = $"等待其他玩家加入({responseMessage.Receiver + 1} / 4)...";
            }
        }

        // 重置页面状态
        private void Reset()
        {
            if (isMatching)
            {
                Connect.ws.MessageReceived -= OnMessageReceived;
            }

            isMatching = false;
            serial = -1;
            inputName.Text = "";
            matchingLabel.Visibility = Visibility.Hidden;

            // 恢复开始按钮的样式
            start.Opacity = 1;
            start.Foreground = new SolidColorBrush(Colors.Black);
        }

    }
}

