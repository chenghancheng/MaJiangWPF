using System;
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

namespace Majiang
{
    /// <summary>
    /// OnlineNameInput.xaml 的交互逻辑
    /// </summary>
    public partial class OnlineNameInput : Page
    {
        private bool isMatching;
        public OnlineNameInput()
        {
            isMatching = false;
            InitializeComponent();
            matchingLabel.Vis
        }

        private void BackToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            // 获取父窗口中的 Frame 控件
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.mainPage);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取父窗口中的 Frame 控件
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.mainPage);
        }

        private void Reset()
        {
            isMatching = false;
            inputName.Text = "";
        }
    }
}
