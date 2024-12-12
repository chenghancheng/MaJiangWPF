using Majiang.View;
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
    /// GamePage.xaml 的交互逻辑
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
        }


        int Transition(int i)
        {
            return (i <= 108) ? ((i - 1) / 36 * 9 + (i - 1) % 9 + 1) : ((i - 1) / 4 + 1);
        }

    }
}
