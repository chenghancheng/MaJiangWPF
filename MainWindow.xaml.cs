using Majiang;
using System;
using System.Collections.Generic;
using System.Media;
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
                // 在UI线程中执行播放
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MediaPlayer player = new MediaPlayer(); // 实例化媒体播放器
                                                            // 使用相对路径
                    player.Open(new Uri("Resources/Music/music.mp3", UriKind.Relative));
                    player.Volume = 0.3;

                    // 设置循环播放
                    player.MediaEnded += (sender, e) =>
                    {
                        // 重置播放位置为开头
                        player.Position = TimeSpan.Zero;
                        // 重新播放
                        player.Play();
                    };

                    // 开始播放
                    player.Play();
                });
            });



            //Task.Run(() =>
            //{
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
