using Majiang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources; // 引入 MediaElement 所需的命名空间

namespace MaJiangApp
{
    public partial class MainWindow : Window
    {
        public MainPage mainPage;
        public OnlineNameInput onlineNameInput;
        MediaPlayer backgroundMusicPlayer;
        public MainWindow()
        {

            InitializeComponent(); // 初始化控件

            string alarmFileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources/Music/music.mp3";
            // 初始化背景音乐播放器
            backgroundMusicPlayer = new MediaPlayer();
            backgroundMusicPlayer.Open(new Uri(alarmFileName, UriKind.RelativeOrAbsolute));
            // 添加 MediaFailed 事件以便调试错误
            backgroundMusicPlayer.MediaFailed += (sender, args) =>
            {
                Console.WriteLine("Media failed: " + args.ErrorException?.Message);
            };

            backgroundMusicPlayer.MediaOpened += (sender, args) =>
            {
                Console.WriteLine("Media failed: ");
            };

            // 监听 MediaEnded 事件
            backgroundMusicPlayer.MediaEnded += (object? sender, EventArgs e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    backgroundMusicPlayer.Position = TimeSpan.Zero; // 重置位置
                }));
            };
            backgroundMusicPlayer.Play();

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
