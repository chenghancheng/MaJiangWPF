using System.Windows;

namespace MaJiangApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
