using MaJiangApp;
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
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage gamePage = new GamePage(true,-1);
            NavigationService.Navigate(gamePage);
            //gamePage.StartGame();
        }

        private void introduceButton_Click(object sender, RoutedEventArgs e)
        {
            // 切换到麻将介绍界面
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.onlineNameInput);
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
    }
}
