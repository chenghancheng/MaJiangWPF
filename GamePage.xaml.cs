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

        private List<BitmapImage> totalCardSelf;
        private List<BitmapImage> totalDiscardedCard;
        private BitmapImage biaoZhi;
        private List<BitmapImage> othersCardImages;

        private StackPanel chiPengGangBox;
        private List<Button> guoChiPengGangHuBtn;
        private List<BitmapImage> guoChiPengGangHuPic;
        private List<List<Button>> chiChoice;
        private ButtonGroup buttonGroupChiPengPang;
        private ButtonGroup multiChiChoiceBtn;

        private List<Dictionary<int, int>> pengAlready;

        private List<Label> direction;
        private List<BitmapImage> directionPic;
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

        private string gameName;
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

        private List<KeyValuePair<int, int>> handCard;




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

            game = new Game("谭杰");

            // 布局的控件
            mainLayout.Children.Add(selfCardBox);
            mainLayout.Children.Add(totalCardBox);

            this.Content = mainLayout;

            // 设置定时器
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(1);

            // 创建Game类的实例
            game = new Game("谭杰");

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

        public void StartGame()
        {
            game = new Game("谭杰");
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
            othersCardImages = new List<BitmapImage>
            {
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/right_normal.png")),
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/back.png")) ,
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/left_normal.png"))
            };

            biaoZhi = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/xia_biao.png")) ;

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


        //todo: 调整长宽
        private void InitDirectionUI()
        {
            // 初始化方向图像列表 (与原始 Qt 图像相同)
            directionPic = new List<BitmapImage>
    {
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_down.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_right.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_up.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_left.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_down1.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_right1.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_up1.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/fangxiang/fangxiang_left1.png"))
    };

            direction = new List<Label>();

            // 创建4个方向标签并将图像设置为内容
            for (int i = 0; i < 4; i++)
            {
                var label = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center, // 水平居中
                    VerticalAlignment = VerticalAlignment.Center, // 垂直居中
                    Width = 80,  // 设定Label的宽度
                    Height = 80  // 设定Label的高度
                };

                // 创建Image并设置Stretch属性
                var image = new Image
                {
                    Source = directionPic[i],  // 将BitmapImage设置为Image的Source
                    Stretch = Stretch.UniformToFill // 确保图像能适应Label的大小
                };

                label.Content = image;  // 将图像控件设置为Label的内容
                direction.Add(label);
            }

            // 初始化总卡片框 (Grid 布局)
            totalCardBox = new Grid();

            // 设置 Grid 的行列
            for (int i = 0; i < 30; i++) // 设置 30 行
                totalCardBox.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 30; i++) // 设置 30 列
                totalCardBox.ColumnDefinitions.Add(new ColumnDefinition());

            // 将4个方向图片添加到网格 (与 Qt 的 QGridLayout 定位相同)

            //下 右 上 左
            Grid.SetRow(direction[0], 15);
            Grid.SetColumn(direction[0], 14);
            Grid.SetRowSpan(direction[0], 1); // 设置行跨度
            Grid.SetColumnSpan(direction[0], 3); // 设置列跨度
            totalCardBox.Children.Add(direction[0]);

            Grid.SetRow(direction[1], 14);
            Grid.SetColumn(direction[1], 15);
            Grid.SetRowSpan(direction[1], 3);
            Grid.SetColumnSpan(direction[1], 1);
            totalCardBox.Children.Add(direction[1]);

            Grid.SetRow(direction[2], 13);
            Grid.SetColumn(direction[2], 14);
            Grid.SetRowSpan(direction[2], 1);
            Grid.SetColumnSpan(direction[2], 3);
            totalCardBox.Children.Add(direction[2]);

            Grid.SetRow(direction[3], 14);
            Grid.SetColumn(direction[3], 13);
            Grid.SetRowSpan(direction[3], 3);
            Grid.SetColumnSpan(direction[3], 1);
            totalCardBox.Children.Add(direction[3]);

            // 设置 totalCardBox 为窗口内容
            this.Content = totalCardBox;
        }



        // 卡牌排序和转换
        private void selfCardTransition()
        {
            handCard.Clear();
            var gg = game.player[0].OwnCard;
            foreach (var card in game.player[0].OwnCard)
            {
                handCard.Add(new KeyValuePair<int, int>(Transition(card), card));
            }

            // 排序
            handCard.Sort((a, b) => a.Key.CompareTo(b.Key));
        }

        // 创建玩家自己的卡牌
        private void createSelfCard(int index)
        {
            // 创建 StackPanel 来代替 QVBoxLayout
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // 创建一个 Image 用于显示卡牌图像
            var label = new Label();
            var image = new Image
            {
                Width = 17,
                Height = 21
            };
            label.Content = image;

            // 创建按钮
            var button = new Button
            {
                Width = 74,
                Height = 106,
                Content = new Image(),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            selfCardTransition();

            if (index >= 0 && index < 13)
            {
                var cardIndex = handCard[index].Key;
                var cardImage = totalCardSelf[cardIndex];
                (button.Content as Image).Source = cardImage;
            }

            button.Click += Button_Click; // 可选的按钮点击事件处理

            // 将控件添加到 StackPanel
            stackPanel.Children.Add(label);
            stackPanel.Children.Add(button);

            selfCard.Add(stackPanel);
            selfCardButton.Add(button);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 处理按钮点击事件
        }

        // 创建其他玩家的卡牌
        private void createOtherCard(int type, BitmapImage pic, int width, int height, int index)
        {
            var label = new Label
            {
                Width = width,
                Height = height,
                Content = new Image { Source = pic }
            };

            // 将标签添加到其他卡片集合中
            othersCard[type].Add(label);
            othersCardBox[type].Children.Add(label);
        }


        private void ResetCardUI()
        {
            bool statu = true;

            // 清除自己卡片的按钮和图片
            //for (int i = selfCardButton.Count - 1; i >= 0; --i)
            //{
            //    selfCardPanel.Children.Remove(selfCardButtons[i]);
            //    selfCardButtons[i].ClearValue(Button.ContentProperty);
            //    selfCardButtons[i].ClearValue(Button.BackgroundProperty);
            //    selfCardButtons[i] = null;
            //    selfCardImages[i].Source = null;
            //    selfCardPanel.Children.Remove(selfCardImages[i]);
            //    selfCardImages[i] = null;
            //}

            //selfCardButtons.Clear();
            //selfCardImages.Clear();

            //// 清除其他玩家卡片
            //foreach (var panel in othersCardPanels)
            //{
            //    panel.Children.Clear();
            //}
            //foreach (var cardList in othersCards)
            //{
            //    cardList.Clear();
            //}

            // 重建自己卡片
            for (int i = 0; i < 13; ++i)
            {
                createSelfCard(i);
            }

            // 在自己卡片区添加间隔
            selfCardBox.Children.Add(new Button { Content = "Space" }); // 可自定义空白控件
            createSelfCard(13);

            // 禁用最后一个卡片的按钮
            selfCardButton[13].IsEnabled = false;

            // 初始化其他玩家卡片
            for (int i = 0; i < 3; ++i)
            {
                int width, length;

                // 设置卡片的宽高
                if (i == 1)
                {
                    width = 74;
                    length = 106;
                }
                else
                {
                    width = 71;
                    length = 50;
                }

                // 清空原有卡片，重新布局
                othersCardBox[i].Children.Clear();

                // 添加 Stretch（伸缩空间），类似于 Qt 中的 addStretch()
                var stretchTop = new FrameworkElement { Height = 20 };  // 使用 FrameworkElement 来模拟
                othersCardBox[i].Children.Add(stretchTop);

                // 创建13张卡片并添加到对应的 StackPanel 中
                for (int j = 0; j < 13; ++j)
                {
                    // 创建一个新的卡片
                    createOtherCard(i, othersCardImages[i], width, length, j);
                }

                // 添加一个间距（Spacing），类似 Qt 中的 addSpacing
                var spacing = new FrameworkElement { Height = 15 };  // 设置间距为 15，可以根据需要调整
                othersCardBox[i].Children.Add(spacing);

                // 创建一张空卡片（这可能是用于显示某种空白状态）
                createOtherCard(i, null, width, length, 13);

                // 再添加一个伸缩空间（类似于 addStretch()）
                var stretchEnd = new FrameworkElement { Height = 20 }; // 再添加一个伸缩空间
                othersCardBox[i].Children.Add(stretchEnd);
            }


        }




    }
}
