using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Resources; // 引入 MediaElement 所需的命名空间

namespace MaJiangApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // 初始化控件
            Task.Run(() =>
            {
                MediaPlayer player = new MediaPlayer();//实例化绘图媒体
                player.Open(new Uri("E:\\BUPT_2024\\C#\\MaJiang\\MaJiangWPF\\Resources\\Music\\music.mp3", UriKind.Absolute));
                player.Volume = 0.3;
                //player.MediaEnded += (sender, e) =>
                //{
                //    // 重置播放位置为开头
                //    player.Position = TimeSpan.Zero;
                //    // 重新播放
                //    player.Play();
                //};
                player.Play();
            });


            // 确保页面导航的操作放在这里
            MainFrame.Navigate(new Majiang.MainPage());
        }
    }
}
