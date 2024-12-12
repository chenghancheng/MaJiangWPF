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

        public MainPage()
        {
            InitializeComponent();


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
            GamePage gamePage = new GamePage();
            NavigationService.Navigate(gamePage);
            gamePage.StartGame();
            // 切换到游戏界面

            StartGame();
        }

        private void introduceButton_Click(object sender, RoutedEventArgs e)
        {
            // 切换到麻将介绍界面
        }

        private void exitGameButton_Click(object sender, RoutedEventArgs e)
        {
            // 退出应用程序
            Application.Current.Shutdown();
        }

        private void backToMainButton_Click(object sender, RoutedEventArgs e)
        {
            // 返回主界面
        }


        private void StartGame()
        {
        }
    }
}
