using System.Windows.Controls;  // 引用 UserControl 类所在的命名空间

namespace Majiang.View
{
    public partial class GameScene : UserControl
    {
        public GameScene()
        {
            InitializeComponent();
            // 在此处可以对控件进行初始化或绑定
        }

        // 例如，如果你有一个 Board 控件或字段，可以在这里定义
        // 如果 GameScene.xaml 中有 Board 控件
        // public Board Board { get; set; } 
    }
}
