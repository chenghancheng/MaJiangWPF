using Majiang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources; // 引入 MediaElement 所需的命名空间

namespace MaJiangApp
{
    public partial class MainWindow : Window
    {
        public MainPage mainPage;
        public OnlineNameInput onlineNameInput;
        public MainWindow()
        {

            InitializeComponent(); // 初始化控件

            Task.Run(() =>
            {
                // 获取嵌入资源的 URI
                Uri soundUri = new Uri("pack://application:,,,/Resources/Music/music.wav");

                // 获取资源流
                StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);

                // 使用 MemoryStream 复制资源流，避免原始流被占用
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // 将原始流复制到内存流
                    soundStreamInfo.Stream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);  // 重置内存流的读取位置

                    // 使用 SoundPlayer 播放音乐
                    SoundPlayer soundPlayer = new SoundPlayer(memoryStream);

                    // 循环播放音乐
                    soundPlayer.PlayLooping();

                    // 等待直到背景音乐播放完毕（注意：此线程将会保持运行，直到被手动终止）
                    // 如果需要结束播放，可以设置一个外部取消标志
                    while (true)
                    {
                        // 持续等待，直到外部控制停止（这里可以加一些退出条件）
                        Thread.Sleep(100);  // 让线程暂时休眠，避免占用 CPU
                    }
                }
            });


            //// 获取嵌入资源的 URI
            //Uri soundUri = new Uri("pack://application:,,,/Resources/Music/music.wav");
            //// 获取资源流
            //StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);
            //// 使用流创建 SoundPlayer
            //SoundPlayer soundPlayer = new SoundPlayer(soundStreamInfo.Stream);
            //soundPlayer.PlayLooping();

            GamePage.totalDiscardedCard = new List<List<BitmapImage>>();
            for (int i = 0; i < 2; ++i)
            {
                GamePage.totalDiscardedCard.Add(new List<BitmapImage>());
            }
            GamePage.totalCardSelf = new List<BitmapImage>();
            GamePage.totalCardSelf.Add(new BitmapImage());
            GamePage.totalDiscardedCard[0].Add(new BitmapImage());
            GamePage.totalDiscardedCard[1].Add(new BitmapImage());
            ImageMethod.LoadImages(GamePage.totalCardSelf, @"./Resources/Images/selfcard2", false);
            ImageMethod.LoadImages(GamePage.totalDiscardedCard[0], @"./Resources/Images/YuanShui", false);
            ImageMethod.LoadImages(GamePage.totalDiscardedCard[1], @"./Resources/Images/YuanCard", false);
            //}).Wait();
            mainPage = new MainPage();
            onlineNameInput = new OnlineNameInput();
            Connect connect = new Connect();
            // 确保页面导航的操作放在这里
            MainFrame.Navigate(mainPage);
        }
    }
}
