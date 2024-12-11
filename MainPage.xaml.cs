using System;
using System.Collections.Generic;
using System.IO;
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

namespace Majiang
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        List<BitmapImage> total_card_self;
        List<BitmapImage> total_discarded_card;

        public MainPage()
        {
            InitializeComponent();
            total_card_self = new List<BitmapImage>();
            total_discarded_card = new List<BitmapImage>();
            total_card_self.Add(new BitmapImage());
            total_discarded_card.Add(new BitmapImage());
            LoadImages(total_card_self, @"./Resources/Images/selfcard2", false);
            LoadImages(total_discarded_card, @"./Resources/Images/card2", true);


            ////清空现有的布局
            //test.children.clear();

            //foreach (var image in total_card_self)
            //{
            //    创建新的按钮
            //   button button = new button();

            //    创建一个 image 控件并将图片设置为按钮的内容
            //   image imagecontrol = new image
            //   {
            //       source = image,
            //       width = 100,   // 可以根据需要设置宽度和高度
            //       height = 100
            //   };

            //    将 image 控件作为按钮的内容
            //    button.content = imagecontrol;

            //    可以设置按钮的其他属性，例如点击事件
            //    button.click += (sender, e) =>
            //    {
            //        在按钮点击时执行的操作
            //        messagebox.show("button clicked!");
            //    };

            //    将按钮添加到布局容器中
            //    test.children.add(button);
            //}
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            // 切换到游戏界面
            tabControl.SelectedIndex = 2;

            StartGame();
        }

        private void introduceButton_Click(object sender, RoutedEventArgs e)
        {
            // 切换到麻将介绍界面
            tabControl.SelectedIndex = 1;
        }

        private void exitGameButton_Click(object sender, RoutedEventArgs e)
        {
            // 退出应用程序
            Application.Current.Shutdown();
        }

        private void backToMainButton_Click(object sender, RoutedEventArgs e)
        {
            // 返回主界面
            tabControl.SelectedIndex = 0;
        }


        private void StartGame()
        {
            gameStatus.Text = "游戏开始了！";
        }

        // 方法来加载图像到列表中
        public void LoadImages(List<BitmapImage> picBox, string pathName, bool isRound)
        {
            // 获取文件夹中的所有文件
            string[] fileList = Directory.GetFiles(pathName);

            foreach (var filePath in fileList)
            {
                // 检查文件扩展名是否为图像格式
                if (filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath2 = "pack://application:,,," + filePath.Substring(1);
                    // 加载图片文件
                    BitmapImage bitmap = new BitmapImage(new Uri(filePath2));

                    // 根据 isRound 参数决定是否需要圆角处理
                    if (isRound)
                    {
                        picBox.Add(RoundImage(bitmap));
                    }
                    else
                    {
                        picBox.Add(bitmap);
                    }
                }
            }
        }

        // 创建圆角图像的方法
        private BitmapImage RoundImage(BitmapImage bitmap)
        {
            // 创建一个带圆角的矩形
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;

            var rect = new Rectangle
            {
                Width = width,
                Height = height,
                RadiusX = 15,  // 设置圆角半径
                RadiusY = 15
            };

            // 创建 Image 控件并设置其 Source 属性为传入的 bitmap
            var image = new Image
            {
                Source = bitmap,
                Width = width,
                Height = height
            };

            // 使用 VisualBrush 将 Image 控件作为视觉对象
            var visualBrush = new VisualBrush(image);
            rect.Fill = visualBrush;

            // 创建一个 RenderTargetBitmap 来渲染带圆角的矩形
            var renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(rect);

            // 将渲染后的图像保存到内存流
            var stream = new System.IO.MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            encoder.Save(stream);

            // 创建一个 BitmapImage 对象并将图像流设置为源
            var roundedBitmap = new BitmapImage();
            stream.Seek(0, SeekOrigin.Begin);
            roundedBitmap.BeginInit();
            roundedBitmap.StreamSource = stream;
            roundedBitmap.EndInit();

            return roundedBitmap;
        }

    }
}
