﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void BackToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            // 获取父窗口中的 Frame 控件
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.mainPage);
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取父窗口中的 Frame 控件
            if (isMatching || inputName.Text.Trim() == "")
            {
                return;
            }
            isMatching = true;
            matchingLabel.Visibility = Visibility.Visible;
            await Connect.ws.StartAsync();
            ChatMessage chatMessage = new ChatMessage(0, inputName.Text, "kjj", 0);
            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
            Connect.ws.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, string message)
        {
            ResponseMessage responseMessage = ResponseMessage.FromJson(message);
            if (responseMessage.MessageType == 0)
            {
                if (serial == -1)
                {
                    serial = int.Parse(responseMessage.Content)-1;
                }
                if (int.Parse(responseMessage.Content) == 4)
                {
                    Reset();
                    // 获取父窗口中的 Frame 控件
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.MainFrame.Navigate(new GamePage(false,serial));
                }
            }
        }

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
        }
    }
}