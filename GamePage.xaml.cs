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
    using System.Collections.Generic;
    using System.Windows.Controls;

    public class ButtonGroup
    {
        // 存储所有的按钮
        public List<Button> Buttons { get; set; }

        // 构造函数
        public ButtonGroup()
        {
            Buttons = new List<Button>();
        }

        // 添加按钮到 ButtonGroup
        public void AddButton(Button button, int id = -1)
        {
            button.Tag = id; // 使用 Tag 存储 ID
            Buttons.Add(button);
        }

        // 清除所有按钮的选中状态
        public void ClearSelection()
        {
            foreach (var button in Buttons)
            {
                button.IsEnabled = true; // 你可以根据需要启用/禁用按钮
            }
        }

        // 获取选中的按钮的 ID
        public int? GetSelectedButtonId()
        {
            foreach (var button in Buttons)
            {
                if (button.IsEnabled) // 判断按钮是否被启用，可以替换为其他标识选中状态的属性
                {
                    return (int)button.Tag;
                }
            }
            return null;
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

        //private List<BitmapImage> total_card_self;
        //private List<BitmapImage> total_discarded_card;

        private List<KeyValuePair<int, int>> handCard;


        // 用于等待用户选择的 TaskCompletionSource
        private TaskCompletionSource<int> tcs = null;




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
            handCard = new List<KeyValuePair<int, int>>();

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

            totalCardSelf = new List<BitmapImage>();
            totalDiscardedCard = new List<BitmapImage>();
            totalCardSelf.Add(new BitmapImage());
            totalDiscardedCard.Add(new BitmapImage());
            LoadImages(totalCardSelf, @"./Resources/Images/selfcard2", false);
            LoadImages(totalDiscardedCard, @"./Resources/Images/card2", true);
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
                Width = 1035,
                Height = 618
            };

            // 初始化 othersCardBox（其他玩家卡片布局）
            othersCardBox = new List<StackPanel>
            {
                new StackPanel { Orientation = Orientation.Vertical ,HorizontalAlignment=HorizontalAlignment.Center  },  // 右
                new StackPanel { Orientation = Orientation.Horizontal ,VerticalAlignment=VerticalAlignment.Center }, // 上
                new StackPanel { Orientation = Orientation.Vertical ,HorizontalAlignment=HorizontalAlignment.Center }    // 左
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
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/back.png")),
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/left_normal.png"))
            };

            // 检查图片加载
            foreach (var imageUri in othersCardImages)
            {
                if (imageUri == null || string.IsNullOrEmpty(imageUri.ToString()))
                {
                    MessageBox.Show("图片加载失败: " + imageUri.ToString());
                }
            }

            // 设置图片显示
            //for (int i = 0; i < othersCardImages.Count; i++)
            //{
            //    //var image = new Image
            //    //{
            //    //    Source = othersCardImages[i],
            //    //    Stretch = Stretch.Uniform, // 确保图片按比例缩放
            //    //    Width = 50,  // 可以调整宽度
            //    //    Height = 50  // 可以调整高度
            //    //};

            //    //var label = new Label
            //    //{
            //    //    Content = image,
            //    //    HorizontalAlignment = HorizontalAlignment.Center,
            //    //    VerticalAlignment = VerticalAlignment.Center,
            //    //    Width = 120,  // 设置 Label 宽度，保证空间足够
            //    //    Height = 120  // 设置 Label 高度，保证空间足够
            //    //};

            //    Label label = new Label();

            //    // 创建 Image 控件并设置图片源
            //    Image image = new Image
            //    {
            //        Source = othersCardImages[i] // 设置图片路径
            //    };

            //    // 设置 Label 的 Content 为 Image 控件
            //    label.Content = image;

            //    othersCard[i % 3].Add(label); // 分别加入对应的 StackPanel
            //}

            // 初始化按钮组
            buttonGroup = new ButtonGroup();

            ResetCardUI(); // 重新设置卡片 UI

            // 设置布局间距
            foreach (var layout in othersCardBox)
            {
                layout.Margin = new Thickness(0);
            }
            selfCardBox.Margin = new Thickness(0);

            // 设置列和行的伸展比例
            for (int i = 0; i < 38; i++)
            {
                //totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < 27; i++)
            {
                //totalCardBox.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                totalCardBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
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

        // 初始化方向图像
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
                //var label = new Label
                //{
                //    HorizontalAlignment = HorizontalAlignment.Center, // 水平居中
                //    VerticalAlignment = VerticalAlignment.Center, // 垂直居中
                //    Width = 80,  // 设定Label的宽度
                //    Height = 80  // 设定Label的高度
                //};

                //// 创建Image并设置Stretch属性
                //var image = new Image
                //{
                //    Source = directionPic[i],  // 将BitmapImage设置为Image的Source
                //    Stretch = Stretch.UniformToFill // 确保图像能适应Label的大小
                //};

                //label.Content = image;  // 将图像控件设置为Label的内容

                Label label = new Label();

                // 创建 Image 控件并设置图片源
                Image image = new Image
                {
                    Source = directionPic[i] // 设置图片路径
                };

                // 设置 Label 的 Content 为 Image 控件
                label.Content = image;
                direction.Add(label);
            }

            //// 初始化总卡片框 (Grid 布局)
            //totalCardBox = new Grid();

            //// 设置 Grid 的行列
            //for (int i = 0; i < 30; i++) // 设置 30 行
            //    totalCardBox.RowDefinitions.Add(new RowDefinition());
            //for (int i = 0; i < 30; i++) // 设置 30 列
            //    totalCardBox.ColumnDefinitions.Add(new ColumnDefinition());

            // 将4个方向图片添加到网格 (与 Qt 的 QGridLayout 定位相同)
            // 下 右 上 左
            Grid.SetRow(direction[0], 12);
            Grid.SetColumn(direction[0], 17);
            Grid.SetRowSpan(direction[0], 1); // 设置行跨度
            Grid.SetColumnSpan(direction[0], 3); // 设置列跨度
            totalCardBox.Children.Add(direction[0]);

            Grid.SetRow(direction[1], 10);
            Grid.SetColumn(direction[1], 19);
            Grid.SetRowSpan(direction[1], 3);
            Grid.SetColumnSpan(direction[1], 1);
            totalCardBox.Children.Add(direction[1]);

            Grid.SetRow(direction[2], 10);
            Grid.SetColumn(direction[2], 17);
            Grid.SetRowSpan(direction[2], 1);
            Grid.SetColumnSpan(direction[2], 3);
            totalCardBox.Children.Add(direction[2]);

            Grid.SetRow(direction[3], 10);
            Grid.SetColumn(direction[3], 17);
            Grid.SetRowSpan(direction[3], 3);
            Grid.SetColumnSpan(direction[3], 1);
            totalCardBox.Children.Add(direction[3]);

            // 设置 totalCardBox 为窗口内容
            //this.Content = totalCardBox;
        }



        // 卡牌排序和转换
        private void selfCardTransition()
        {
            handCard.Clear();
            var gg = game.player[0].OwnCard;
            Console.WriteLine(gg);
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
                Width = 46,
                Height = 66,
                Content = new Image(),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            selfCardTransition();

            if (index >= 0 && index < 13)
            {
                int siz = handCard.Count;
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
            selfCardLabel.Add(label);
            selfCardBox.Children.Add(selfCard[index]);
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
                Content = new Image { Source = pic, Stretch = Stretch.Uniform },
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                //BorderBrush = Brushes.Transparent, // 去除边框
                BorderThickness = new Thickness(0), // 去除边框厚度
                //Background =Brushes.AliceBlue
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
                    width = 46;
                    length = 66;
                }
                else
                {
                    width = 53;
                    length = 36;
                }

                // 清空原有卡片，重新布局
                othersCardBox[i].Children.Clear();
                //othersCardBox[i].Margin = new Thickness(0);


                // 添加 Stretch（伸缩空间），类似于 Qt 中的 addStretch()
                //var stretchTop = new FrameworkElement { Height = 20 };  // 使用 FrameworkElement 来模拟
                //othersCardBox[i].Children.Add(stretchTop);

                // 左边的“伸缩空间”元素
                //othersCardBox[i].Children.Add(new FrameworkElement
                //{
                //    HorizontalAlignment = HorizontalAlignment.Stretch,
                //    VerticalAlignment=VerticalAlignment.Stretch
                //});

                // 创建13张卡片并添加到对应的 StackPanel 中
                for (int j = 0; j < 13; ++j)
                {
                    // 创建一个新的卡片
                    createOtherCard(i, othersCardImages[i], width, length, j);
                }

                // 添加一个间距（Spacing），类似 Qt 中的 addSpacing
                // var spacing = new FrameworkElement { Height = 15 };  // 设置间距为 15，可以根据需要调整
                //othersCardBox[i].Children.Add(spacing);

                // 创建一张空卡片（这可能是用于显示某种空白状态）
                createOtherCard(i, null, width, length, 13);

                // 再添加一个伸缩空间（类似于 addStretch()）
                //var stretchEnd = new FrameworkElement { Height = 20 }; // 再添加一个伸缩空间
                //othersCardBox[i].Children.Add(stretchEnd);

                // 左边的“伸缩空间”元素
                //othersCardBox[i].Children.Add(new FrameworkElement
                //{
                //    HorizontalAlignment = HorizontalAlignment.Stretch,
                //    VerticalAlignment = VerticalAlignment.Stretch
                //});
            }


        }








        public void InitChiPengGangUI()
        {
            //-----------吃碰杠胡等--------------
            chiPengGangBox = new StackPanel();
            chiPengGangBox.Orientation = Orientation.Horizontal;

            guoChiPengGangHuPic = new List<BitmapImage>
    {
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/chi_peng_gang_hu/guo.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/chi_peng_gang_hu/chi.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/chi_peng_gang_hu/peng.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/chi_peng_gang_hu/gang.png")),
        new BitmapImage(new Uri("pack://application:,,,/Resources/Images/chi_peng_gang_hu/hu.png"))
    };

            // 创建按钮组
            buttonGroupChiPengPang = new ButtonGroup();
            multiChiChoiceBtn = new ButtonGroup();

            // 按钮点击事件处理
            foreach (var button in buttonGroupChiPengPang.Buttons)
            {
                button.Click += async (sender, e) =>
                {
                    var btnId = (int)(button.Tag); // 获取按钮的自定义 ID

                    if (btnId < guoChiPengGangHuPic.Count && waitUserOtherChoice)
                    {
                        tcs = new TaskCompletionSource<int>();

                        switch (guoChiPengGangHu[btnId])
                        {
                            case 0:
                                Console.WriteLine("guo");
                                guoChiPengGangHuChoice = 0;
                                waitUserOtherChoice = false;
                                await tcs.Task;
                                break;
                            case 1 when !multiChi:
                                Console.WriteLine("chi");
                                guoChiPengGangHuChoice = 1;
                                waitUserOtherChoice = false;
                                await tcs.Task;
                                break;
                            case 2:
                                Console.WriteLine("peng");
                                guoChiPengGangHuChoice = 2;
                                waitUserOtherChoice = false;
                                await tcs.Task;
                                break;
                            case 3:
                                Console.WriteLine("gang");
                                guoChiPengGangHuChoice = 3;
                                waitUserOtherChoice = false;
                                await tcs.Task;
                                break;
                            case 4:
                                Console.WriteLine("hu");
                                guoChiPengGangHuChoice = 4;
                                waitUserOtherChoice = false;
                                await tcs.Task;
                                break;
                        }
                    }
                };
            }

            // 创建吃碰杠按钮
            for (int i = 0; i < 5; i++)
            {
                CreateChiPengGangBtn(i); // 创建按钮并加入布局
            }
            for(int i = 0; i < 3; i++)
{
                // 创建一个新的 List<Button> 实例，并添加到 chiChoice 中
                List<Button> qvbtn = new List<Button>();
                chiChoice.Add(qvbtn);
            }

            // 创建 multiChiChoiceBtn 按钮并添加到按钮组
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var button = new Button
                    {
                        Width = 35,
                        Height = 50,
                        Content = $"Button {i}-{j}",  // 设置按钮显示内容
                        IsEnabled = true,
                    };

                    button.Tag = i;
                    multiChiChoiceBtn.AddButton(button, i); // 将按钮加入 ButtonGroup
                    chiChoice[i].Add(button);
                }
            }

            // 连接多选按钮的点击事件
            foreach (var button in multiChiChoiceBtn.Buttons)
            {
                button.Click += (sender, e) =>
                {
                    if (multiChi)
                    {
                        var btnId = (int)button.Tag; // 获取按钮的自定义 ID
                        Console.WriteLine(btnId);
                        if (waitUserOtherChoice)
                        {
                            multiChiChoice = btnId;
                            guoChiPengGangHuChoice = 1; // 选择 "吃"
                            waitUserOtherChoice = false;
                        }
                    }
                };
            }

            // 将按钮添加到布局中
            chiPengGangBox.Children.Add(new UIElement()); // 占位符，适配原布局的空白

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // 使用 Margin 控制按钮之间的间距
                    chiChoice[i][j].Margin = new Thickness(5);  // 设置按钮之间的间距
                    chiPengGangBox.Children.Add(chiChoice[i][j]);
                }

                if (i != 2)
                    chiPengGangBox.Children.Add(new UIElement()); // 空白
            }

            // 添加按钮和间距
            for (int i = 4; i >= 0; i--)
            {
                guoChiPengGangHuBtn[i].Margin = new Thickness(5);  // 设置按钮之间的间距
                chiPengGangBox.Children.Add(guoChiPengGangHuBtn[i]);
                if (i != 0)
                    chiPengGangBox.Children.Add(new UIElement()); // 空白
            }

            // 使用 Grid 或 WrapPanel 来替代 StackPanel
            // WrapPanel 可以帮助自动换行并支持间距
            var wrapPanel = new WrapPanel();
            wrapPanel.Children.Add(chiPengGangBox);

            // 将布局添加到父布局中（假设 totalCardBox 是你的父布局）
            totalCardBox.Children.Add(wrapPanel);  // 使用 wrapPanel 而不是直接使用 StackPanel
        }




        // 创建按钮的方法
        private void CreateChiPengGangBtn(int id)
        {
            // 创建一个 Button，代替 RadioButton
            Button button = new Button
            {
                Width = 40,
                Height = 40,
                Content = new Image
                {
                    Source = new BitmapImage(), // 图像路径，可以根据需要设置具体的图片路径
                    Stretch = Stretch.Fill
                },
                Tag = id // 将按钮的 ID 存储在 Tag 属性中
            };

            // 按钮点击事件绑定
            button.Click += (sender, e) =>
            {
                var btnId = (int)((Button)sender).Tag; // 获取按钮的自定义 ID
                                                       // 处理点击事件，根据按钮的 ID 进行逻辑处理
                Console.WriteLine($"Button {btnId} clicked");

                // 在这里执行按钮点击后的逻辑
            };

            // 将 Button 添加到 StackPanel 或者其他容器中
            guoChiPengGangHuBtn.Add(button); // 将 Button 加入到按钮列表中
            buttonGroupChiPengPang.AddButton(button, id);
        }




        private void CompleteUserChoice()
        {
            // 完成用户选择时，设置 TaskCompletionSource 的结果
            if (tcs != null && !tcs.Task.IsCompleted)
            {
                tcs.SetResult(guoChiPengGangHuChoice); // 传递用户的选择
            }
        }


        private void InitDiscardedCardUI()
        {
            // 初始化丢弃的卡牌（方向上4个）
            discarded = new List<List<Label>>();
            for (int i = 0; i < 4; i++)
            {
                discarded.Add(new List<Label>());
            }

            discardedNowIndex = new List<int> { 0, 0, 0, 0 };

            // 上方丢弃的卡牌
            for (int i = 7, j = 23, index = 0; i >= 5 && j >= 13; --j, ++index)
            {
                var label = new Label
                {
                    Width = 44,
                    Height = 60,
                    Background = Brushes.Gray,  // 使用背景色作为占位符
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new TextBlock
                    {
                        Text = "占位符", // 可以显示一些文本内容作为占位符
                        Foreground = Brushes.White
                    }
                };

                discarded[2].Add(label);

                // 在 Grid 中添加控件，假设 `totalCardBox` 是一个 Grid 控件
                totalCardBox.Children.Add(label);
                Grid.SetRow(label, i);   // 设置行
                Grid.SetColumn(label, j); // 设置列

                if (j == 13 && i > 5)
                {
                    j = 24;
                    i--;
                }
            }

            // 左侧丢弃的卡牌
            for (int i = 8, j = 12, index = 0; i <= 15 && j >= 9; ++i, ++index)
            {
                var label = new Label
                {
                    Width = 60,
                    Height = 44,
                    Background = Brushes.Gray, // 使用背景色作为占位符
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new TextBlock
                    {
                        Text = "占位符", // 可以显示一些文本内容作为占位符
                        Foreground = Brushes.White
                    }
                };

                discarded[3].Add(label);

                totalCardBox.Children.Add(label);
                Grid.SetRow(label, i);    // 设置行
                Grid.SetColumn(label, j); // 设置列

                if (i == 15 && j > 9)
                {
                    i = 7;
                    j--;
                }
            }

            // 右侧丢弃的卡牌
            for (int i = 15, j = 24, index = 0; i >= 8 && j <= 27; --i, ++index)
            {
                var label = new Label
                {
                    Width = 600,
                    Height = 444,
                    Background = Brushes.Gray, // 使用背景色作为占位符
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new TextBlock
                    {
                        Text = "占位符dasjdadoajdoijqoi wdio jqwiod oqwidj oisajdoiasjdiojasodjoqiwjdqdwiojqwiodjoqwdqw", // 可以显示一些文本内容作为占位符
                        Foreground = Brushes.Red
                    }
                };

                discarded[1].Add(label);

                totalCardBox.Children.Add(label);
                Grid.SetRow(label, i);    // 设置行
                Grid.SetColumn(label, j); // 设置列

                if (i == 8 && j < 27)
                {
                    i = 16;
                    j++;
                }
            }

            // 下方丢弃的卡牌
            for (int i = 16, j = 13, index = 0; i <= 18 && j <= 23; ++j, ++index)
            {
                var label = new Label
                {
                    Width = 44,
                    Height = 60,
                    Background = Brushes.Gray, // 使用背景色作为占位符
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new TextBlock
                    {
                        Text = "占位符", // 可以显示一些文本内容作为占位符
                        Foreground = Brushes.White
                    }
                };

                discarded[0].Add(label);

                totalCardBox.Children.Add(label);
                Grid.SetRow(label, i);    // 设置行
                Grid.SetColumn(label, j); // 设置列

                if (j == 23 && i < 18)
                {
                    j = 12;
                    i++;
                }
            }

            // 设置 Grid 的间距（你可以根据需要设置）
            totalCardBox.Margin = new Thickness(2);
        }







    }
}
