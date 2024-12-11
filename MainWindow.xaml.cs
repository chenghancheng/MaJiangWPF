using System.Windows;

namespace MaJiangApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Majiang.MainPage());
        }
    }
}
