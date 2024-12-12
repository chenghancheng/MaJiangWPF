using Majiang.View;
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
using System.Windows.Threading;

namespace Majiang
{
    public class ButtonGroup
    {
        public List<RadioButton> Buttons { get; set; }
        public ButtonGroup()
        {
            Buttons = new List<RadioButton>();
        }

        public void AddButton(RadioButton button)
        {
            Buttons.Add(button);
        }

        public void ClearSelection()
        {
            foreach (var button in Buttons)
            {
                button.IsChecked = false;
            }
        }
    }

    public class Game
    {
        // 游戏核心类（此处作为占位，具体实现需要根据需求补充）
    }

    /// <summary>
    /// GamePage.xaml 的交互逻辑
    /// </summary>
    public partial class GamePage : Page
    {
        // 定义界面控件
        private StackPanel selfCardBox;
        private Grid totalCardBox;
        private List<StackPanel> othersCardBox;
        private List<StackPanel> selfCard;
        private List<Button> selfCardButton;
        private List<Label> selfCardLabel;
        private List<List<Label>> othersCard;
        private ButtonGroup buttonGroup;
        private int choiceBtn = -1;

        private List<Image> totalCardSelf;
        private List<Image> totalDiscardedCard;
        private Image biaoZhi;
        private List<Image> othersCardImages;

        private StackPanel chiPengGangBox;
        private List<Button> guoChiPengGangHuBtn;
        private List<Image> guoChiPengGangHuPic;
        private List<List<Button>> chiChoice;
        private ButtonGroup buttonGroupChiPengPang;
        private ButtonGroup multiChiChoiceBtn;

        private List<Dictionary<int, int>> pengAlready;

        private List<Label> direction;
        private List<Image> directionPic;
        private List<List<Label>> discarded;
        private List<int> discardedNowIndex;

        private StackPanel stackWidget;

        private Grid checkout;
        private StackPanel checkoutLayout;
        private Label settlementPic;
        private Label settlementState;
        private StackPanel checkoutBtn;
        private Button continueGame;
        private Button backToMainStage;
        private Button hideCheckOut;
        private string statement;
        private bool statu = true;

        private Game game;
        private bool waitUserChoice = false;
        private bool waitUserOtherChoice = false;
        private int discardIndex = 0;
        private int cur = 0;
        private List<int> guoChiPengGangHu;
        private int guoChiPengGangHuChoice = 0;
        private bool multiChi = false;
        private int multiChiChoice = -1;
        private bool deal = true;
        private bool isFinish = false;
        private int typeHu = 0;

        private Label remained;
        private string remainedText;

        private DispatcherTimer timer;

        private List<BitmapImage> total_card_self;
        private List<BitmapImage> total_discarded_card;
        public GamePage()
        {
            // StackPanel用于自定义布局
            var mainLayout = new StackPanel();

            // 初始化布局
            selfCardBox = new StackPanel { Orientation = Orientation.Horizontal };
            totalCardBox = new Grid();
            othersCardBox = new List<StackPanel> { new StackPanel(), new StackPanel(), new StackPanel() };  // 假设有3个对手
            selfCard = new List<StackPanel>();
            selfCardButton = new List<Button>();
            selfCardLabel = new List<Label>();
            othersCard = new List<List<Label>>();

            // 布局的控件
            mainLayout.Children.Add(selfCardBox);
            mainLayout.Children.Add(totalCardBox);

            this.Content = mainLayout;

            // 设置定时器
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(1);

            // 创建Game类的实例
            game = new Game();

            InitializeComponent();

            total_card_self = new List<BitmapImage>();
            total_discarded_card = new List<BitmapImage>();
            total_card_self.Add(new BitmapImage());
            total_discarded_card.Add(new BitmapImage());
            LoadImages(total_card_self, @"./Resources/Images/selfcard2", false);
            LoadImages(total_discarded_card, @"./Resources/Images/card2", true);
            InitCardUI();
            InitDirectionUI();

        }

        private void TimerTick(object sender, EventArgs e)
        {
            // 定时器事件处理
            if (waitUserChoice || waitUserOtherChoice)
            {
                // 游戏等待用户输入
            }
        }

        int Transition(int i)
        {
            return (i <= 108) ? ((i - 1) / 36 * 9 + (i - 1) % 9 + 1) : ((i - 1) / 4 + 1);
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

        public static BitmapSource AdaptImageSize(BitmapSource source, int width, int height, int rotate)
        {
            // 创建一个旋转变换
            RotateTransform rotateTransform = new RotateTransform(rotate);

            // 应用旋转变换
            TransformedBitmap transformedBitmap = new TransformedBitmap();
            transformedBitmap.BeginInit();
            transformedBitmap.Source = source;
            transformedBitmap.Transform = rotateTransform;
            transformedBitmap.EndInit();

            // 缩放图像（保持纵横比，使用平滑变换）
            var scaledBitmap = new TransformedBitmap(transformedBitmap, new ScaleTransform(
                (double)width / source.PixelWidth, (double)height / source.PixelHeight));

            return scaledBitmap;
        }
        public void InitCardUI()
        {
            // 创建 GridLayout 控件，并设置大小
            totalCardBox = new Grid
            {
                Width = 648,
                Height = 1152
            };

            // 初始化 othersCardBox（其他玩家卡片布局）
            othersCardBox = new List<StackPanel>
            {
                new StackPanel { Orientation = Orientation.Vertical },  // 右
                new StackPanel { Orientation = Orientation.Horizontal }, // 上
                new StackPanel { Orientation = Orientation.Vertical }    // 左
            };

            selfCardBox = new StackPanel { Orientation = Orientation.Horizontal };

            // 初始化 othersCard（其他玩家卡片的标签）
            othersCard = new List<List<Label>>();
            for (int i = 0; i < 3; i++)
            {
                othersCard.Add(new List<Label>());
            }

            // 加载图片资源
            othersCardImages = new List<Image>
            {
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/right_normal.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/back.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/left_normal.png")) }
            };

            biaoZhi = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/xia_biao.png")) };

            // 初始化按钮组
            buttonGroup = new ButtonGroup();
            //buttonGroup.Exclusive = true; // 设置按钮组为互斥

            // 事件处理：按钮点击时的行为
            //foreach (var button in buttonGroup.Buttons)
            //{
            //    button.Click += (sender, e) =>
            //    {
            //        var clickedButton = sender as Button;
            //        if (statu && waitUserChoice)
            //        {
            //            if (clickedButton.IsChecked == true)
            //            {
            //                if (choiceBtn == buttonGroup.CheckedId)
            //                {
            //                    selfCardLabel[choiceBtn].Content = null; // 清除标识
            //                    waitUserChoice = false;
            //                    discardIndex = choiceBtn;
            //                    choiceBtn = -1;
            //                }
            //                else
            //                {
            //                    if (choiceBtn != -1)
            //                    {
            //                        selfCardLabel[choiceBtn].Content = null; // 清除标识
            //                    }
            //                    choiceBtn = buttonGroup.CheckedId;
            //                    selfCardLabel[choiceBtn].Content = biaoZhi; // 显示标识
            //                }
            //            }
            //        }
            //    };
            //}

            //ResetCardUI(); // 重新设置卡片 UI

            // 设置布局间距
            foreach (var layout in othersCardBox)
            {
                layout.Margin = new Thickness(0);
            }
            selfCardBox.Margin = new Thickness(0);

            // 设置列和行的伸展比例
            //for (int i = 0; i < 38; i++)
            //{
            //    totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1*) });
            //}
            //for (int i = 0; i < 27; i++)
            //{
            //    totalCardBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1*) });
            //}
            for (int i = 0; i < 38; i++)
            {
                totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 27; i++)
            {
                totalCardBox.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // 添加布局到 Grid
            totalCardBox.Children.Add(othersCardBox[0]);
            Grid.SetRow(othersCardBox[0], 4);
            Grid.SetColumn(othersCardBox[0], 31);
            Grid.SetRowSpan(othersCardBox[0], 19);
            Grid.SetColumnSpan(othersCardBox[0], 1);

            totalCardBox.Children.Add(othersCardBox[1]);
            Grid.SetRow(othersCardBox[1], 1);
            Grid.SetColumn(othersCardBox[1], 9);
            Grid.SetRowSpan(othersCardBox[1], 2);
            Grid.SetColumnSpan(othersCardBox[1], 20);

            totalCardBox.Children.Add(othersCardBox[2]);
            Grid.SetRow(othersCardBox[2], 4);
            Grid.SetColumn(othersCardBox[2], 5);
            Grid.SetRowSpan(othersCardBox[2], 19);
            Grid.SetColumnSpan(othersCardBox[2], 1);

            totalCardBox.Children.Add(selfCardBox);
            Grid.SetRow(selfCardBox, 24);
            Grid.SetColumn(selfCardBox, 9);
            Grid.SetRowSpan(selfCardBox, 3);
            Grid.SetColumnSpan(selfCardBox, 20);

            // 添加剩余牌数标签
            remained = new Label
            {
                Content = remainedText + "150", // 示例，实际需要动态更新
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGoldenrodYellow),
                Width = 170,
                Height = 30,
                FontFamily = new System.Windows.Media.FontFamily("楷体"),
                FontSize = 10
            };

            Grid.SetRow(remained, 1);
            Grid.SetColumn(remained, 34);
            Grid.SetRowSpan(remained, 1);
            Grid.SetColumnSpan(remained, 4);

            totalCardBox.Children.Add(remained);

            // 将整个 UI 设置为布局
            this.Content = totalCardBox;
        }

        private void InitDirectionUI()
        {
            // Initialize the list of direction images (same as the original Qt images)
            directionPic = new List<Image>
            {
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_down.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_right.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_up.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_left.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_down1.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_right1.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_up1.png")) },
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_left1.png")) }
            };

            direction = new List<Label>();

            // Create 4 direction labels and set images as content
            for (int i = 0; i < 4; i++)
            {
                var label = new Label
                {
                    Content = directionPic[i],  // Set the image as the label's content
                    Width = directionPic[i].Source.Width,  // Set size to match the image size
                    Height = directionPic[i].Source.Height
                };
                direction.Add(label);
            }

            // Initialize the total card box (Grid layout)
            totalCardBox = new Grid();

            // Add 4 direction images to the grid (same as Qt's QGridLayout positioning)
            Grid.SetRow(direction[0], 12);
            Grid.SetColumn(direction[0], 17);
            //direction[0].SetValue(Grid.RowSpanProperty, 1);
            //direction[0].SetValue(Grid.ColumnSpanProperty, 3);
            totalCardBox.Children.Add(direction[0]);

            Grid.SetRow(direction[1], 10);
            Grid.SetColumn(direction[1], 19);
            //direction[1].SetValue(Grid.RowSpanProperty, 3);
            //direction[1].SetValue(Grid.ColumnSpanProperty, 1);
            totalCardBox.Children.Add(direction[1]);

            Grid.SetRow(direction[2], 10);
            Grid.SetColumn(direction[2], 17);
            //direction[2].SetValue(Grid.RowSpanProperty, 1);
            //direction[2].SetValue(Grid.ColumnSpanProperty, 3);
            totalCardBox.Children.Add(direction[2]);

            Grid.SetRow(direction[3], 10);
            Grid.SetColumn(direction[3], 17);
            //direction[3].SetValue(Grid.RowSpanProperty, 3);
            //direction[3].SetValue(Grid.ColumnSpanProperty, 1);
            totalCardBox.Children.Add(direction[3]);

            // Set totalCardBox as the window content
            this.Content = totalCardBox;
        }


    }
}
