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
    using MaJiangApp;
    using System.Collections;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Threading;


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
        //private List<List<Label>> othersCard;
        private List<List<Image>> othersCard;
        private ButtonGroup buttonGroup;
        private int choiceBtn = -1;

        public static List<BitmapImage> totalCardSelf;
        public static List<List<BitmapImage>> totalDiscardedCard;
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
        //private List<List<Label>> discarded;
        private List<List<Image>> discarded;
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
        private string remainedText = "剩余牌数：";

        private DispatcherTimer timer;

        //private List<BitmapImage> total_card_self;
        //private List<BitmapImage> total_discarded_card;

        private List<KeyValuePair<int, int>> handCard;


        // 用于等待用户选择的 TaskCompletionSource
        private TaskCompletionSource<int> tcs = null;
        private Button backToMainPage;

        //联机部分
        bool type;
        int serial = -1;
        public static string name;
        private List<List<int>> chiTempOnline;//将多种吃法存入该数组中
        private List<List<int>> transitionChiOnline;//将多种吃法的牌所对应的图片的下标存入该数组中
        private List<string> playerNames;

        public GamePage(bool type, int serial)
        {
            // StackPanel用于自定义布局
            //var mainLayout = new StackPanel();
            this.type = type;
            // 初始化布局
            selfCardBox = new StackPanel { Orientation = Orientation.Horizontal };
            totalCardBox = new Grid();
            othersCardBox = new List<StackPanel> { new StackPanel(), new StackPanel(), new StackPanel() };  // 假设有3个对手
            selfCard = new List<StackPanel>();
            selfCardButton = new List<Button>();
            selfCardLabel = new List<Label>();
            //othersCard = new List<List<Label>>();
            othersCard = new List<List<Image>>();
            handCard = new List<KeyValuePair<int, int>>();
            guoChiPengGangHuBtn = new List<Button>();
            chiChoice = new List<List<Button>>();
            othersCardImages = new List<BitmapImage>();
            guoChiPengGangHu = new List<int>();
            //totalDiscardedCard = new List<List<BitmapImage>>();
            //for (int i = 0; i < 2; ++i)
            //{
            //    totalDiscardedCard.Add(new List<BitmapImage>());
            //}
            discardedNowIndex = new List<int>();
            pengAlready = new List<Dictionary<int, int>>();



            // 布局的控件
            //mainLayout.Children.Add(selfCardBox);
            //mainLayout.Children.Add(totalCardBox);

            //////这个不要用task
            //this.Content = mainLayout;


            InitializeComponent();

            //totalCardSelf = new List<BitmapImage>();
            ////totalDiscardedCard = new List<BitmapImage>();
            //totalCardSelf.Add(new BitmapImage());
            //totalDiscardedCard[0].Add(new BitmapImage());
            //totalDiscardedCard[1].Add(new BitmapImage()); 
            //ImageMethod.LoadImages(totalCardSelf, @"./Resources/Images/selfcard2", false);
            ////LoadImages(totalDiscardedCard, @"./Resources/Images/card2", true);
            ////ImageMethod.LoadImages(totalDiscardedCard, @"./Resources/Images/card2", false);
            //ImageMethod.LoadImages(totalDiscardedCard[0], @"./Resources/Images/YuanShui", false);
            //ImageMethod.LoadImages(totalDiscardedCard[1], @"./Resources/Images/YuanCard", false);
            InitMainFrameUI();

            // 设置定时器
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(1);

            // 创建Game类的实例
            game = new Game("谭杰");

            InitCardUI();
            InitDirectionUI();
            InitChiPengGangUI();
            InitDiscardedCardUI();
            InitCheckoutUI();
            if (type)
            {
                StartGame();
            }
            else
            {
                this.serial = serial;
                Connect.ws.MessageReceived += OnMessageReceived;
                chiTempOnline = new List<List<int>>();//将多种吃法存入该数组中
                transitionChiOnline = new List<List<int>>();//将多种吃法的牌所对应的图片的下标存入该数组中
                playerNames = new List<string>();
            }
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

        int TransitionCur(int Cur)
        {
            return (Cur + 4 - serial) % 4;
        }

        // 自定义事件处理程序（跨文件定义）
        private async void OnMessageReceived(object sender, string message)
        {
            // 在此处理收到的消息
            Console.WriteLine("事件触发：收到消息：" + message);
            // 你可以在这里做更多的逻辑处理，比如解析消息、更新界面等
            ResponseMessage responseMessage = ResponseMessage.FromJson(message);
            switch (responseMessage.MessageType)
            {
                case (int)MessageType.StartGame:
                    {
                        game.player[0].OwnCard.Clear();
                        game.player[0].OwnCard.UnionWith(responseMessage.content.handCard[serial]);
                        for (int i = 0; i < 4; ++i)
                        {
                            game.player[i].Name = responseMessage.content.playerName[(i + serial) % 4];
                        }
                        remained.Content = remainedText + responseMessage.content.remainCards;
                    }
                    break;
                case (int)MessageType.GetCard:
                    {
                        int receiverId = responseMessage.content.curPlayer;
                        int transCur = TransitionCur(cur);
                        direction[transCur].Content = new Image
                        {
                            Source = directionPic[transCur]
                        };
                        cur = receiverId;
                        transCur = TransitionCur(cur);
                        direction[transCur].Content = new Image
                        {
                            Source = directionPic[transCur + 4]
                        };
                        remained.Content = remainedText + responseMessage.content.remainCards;
                        if (receiverId == serial)
                        {
                            selfCardTransition();
                            for (int i = 0; i < handCard.Count; ++i)
                                selfCardButton[i].Content = new Image
                                {
                                    Source = totalCardSelf[handCard[i].Key]
                                };

                            int getCard = responseMessage.content.getCard;

                            game.player[0].CurNum++;
                            game.player[0].OwnCard.Add(getCard);
                            handCard.Add(new KeyValuePair<int, int>(Transition(getCard), getCard));
                            selfCardButton[selfCardButton.Count - 1].Content = new Image
                            {
                                Source = totalCardSelf[Transition(getCard)]
                            };
                            selfCardButton[selfCardButton.Count - 1].IsEnabled = true;

                            guoChiPengGangHu.Clear();
                            guoChiPengGangHu.Add(0);
                            bool gangType = true;
                            if (game.player[transCur].CheckGang() != -1)
                            {
                                guoChiPengGangHu.Add(3);
                                gangType = true;
                                waitUserOtherChoice = true;
                            }
                            if (game.player[transCur].CheckAddGang(getCard))
                            {
                                guoChiPengGangHu.Add(3);
                                gangType = false;
                                waitUserOtherChoice = true;
                            }
                            if (game.player[transCur].CheckWin())
                            {
                                guoChiPengGangHu.Add(4);
                                waitUserOtherChoice = true;
                            }

                            if (waitUserOtherChoice)
                            {
                                for (int i = 0; i < guoChiPengGangHu.Count; i++)
                                {
                                    guoChiPengGangHuBtn[i].Content = new Image
                                    {
                                        Source = guoChiPengGangHuPic[guoChiPengGangHu[i]]
                                    };
                                    guoChiPengGangHuBtn[i].Visibility = Visibility.Visible;
                                }
                            }

                            // 暂停程序
                            if (waitUserOtherChoice)
                            {
                                await WaitForPlayer();
                            }

                            if (guoChiPengGangHu.Count > 1)
                            {
                                if (guoChiPengGangHuChoice == 4)
                                {
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    typeHu = 1;
                                    statement = statement + game.player[game.cur].GetName() + "自摸";

                                    ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Win_Zimo, name, "", -1);
                                    await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    return;
                                }
                                else if (guoChiPengGangHuChoice == 3)
                                {
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    if (gangType)
                                    {
                                        game.player[transCur].Gang(getCard, 0);
                                        PengGangSelf(false, Transition(getCard));

                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Gang_Zimo, name, "", 0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    }
                                    else
                                    {
                                        game.player[transCur].AddGang(getCard);
                                        AddGangSelf(Transition(getCard));

                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Gang_JiaGang, name, "", 0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    }
                                    return;
                                }
                            }

                            waitUserChoice = true;
                            if (waitUserChoice)
                            {
                                await WaitForPlayer();
                            }

                            KeyValuePair<int, int> discardThisRound = new KeyValuePair<int, int>();
                            if (discardIndex >= 0 && discardIndex < game.player[transCur].OwnCard.Count)
                                discardThisRound = new KeyValuePair<int, int>(handCard[discardIndex].Key, handCard[discardIndex].Value);
                            game.player[transCur].Discard(discardThisRound.Value);//出牌

                            discarded[0][discardedNowIndex[0]].Source = totalDiscardedCard[1][discardThisRound.Key];

                            selfCardButton[selfCardButton.Count - 1].Content = null;//更新按钮
                            selfCardButton[selfCardButton.Count - 1].IsEnabled = false;

                            discardedNowIndex[0]++;

                            selfCardTransition();
                            for (int i = 0; i < handCard.Count; ++i)
                                selfCardButton[i].Content = new Image
                                {
                                    Source = totalCardSelf[handCard[i].Key]
                                };

                            ChatMessage chatMessage = new ChatMessage((int)MessageType.Discard, name, "", discardThisRound.Value);
                            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
                        }
                        else
                        {
                            othersCard[transCur - 1][othersCard[transCur - 1].Count - 1].Source = othersCardImages[transCur - 1];
                        }
                    }
                    break;
                case (int)MessageType.Discard:
                    {
                        if (responseMessage.content.curPlayer == serial)
                        {
                            ChatMessage chatMessage = new ChatMessage((int)MessageType.Pass, name, "", 0);
                            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
                        }
                        else
                        {
                            int discardThisRound = responseMessage.content.disCard;

                            int transCur = TransitionCur(responseMessage.content.curPlayer);
                            othersCard[transCur - 1][othersCard[transCur - 1].Count - 1].Source = null;
                            //discarded[transCur][discardedNowIndex[transCur]].Source = totalDiscardedCard[1][discardThisRound.Key];
                            discarded[transCur][discardedNowIndex[transCur]].Source = totalDiscardedCard[(transCur - 1) % 2][Transition(discardThisRound)];
                            discarded[transCur][discardedNowIndex[transCur]].RenderTransform = new RotateTransform(transCur / 2 * (-180));
                            discarded[transCur][discardedNowIndex[transCur]].RenderTransformOrigin = new Point(0.5, 0.5);
                            
                            discardedNowIndex[transCur]++;

                            selfCardTransition();
                            for (int i = 0; i < handCard.Count; ++i)
                                selfCardButton[i].Content = new Image
                                {
                                    Source = totalCardSelf[handCard[i].Key]
                                };

                            guoChiPengGangHu.Clear();
                            guoChiPengGangHu.Add(0);
                            deal = true;
                            if (deal)
                            {
                                multiChi = false;//当前是否有多种吃法
                                chiTempOnline = game.player[0].CheckChi(discardThisRound);//检查当前牌是否能吃
                                chiTempOnline.Sort((a, b) => Transition(a[0]).CompareTo(Transition(b[0])));
                                if (chiTempOnline.Count > 0)//能吃牌
                                {
                                    guoChiPengGangHu.Add(1);
                                    waitUserOtherChoice = true;
                                }
                                if (chiTempOnline.Count > 1)//能吃牌且有多种吃法
                                {
                                    transitionChiOnline.Clear();
                                    multiChi = true;
                                    for (int i = 0; i < chiTempOnline.Count; i++)
                                    {
                                        List<int> t = new List<int>();
                                        transitionChiOnline.Add(t);
                                        transitionChiOnline[i].AddRange(chiTempOnline[i]);
                                        transitionChiOnline[i].Add(discardThisRound);
                                        for (int j = 0; j < 3; j++)
                                        {
                                            chiChoice[i][j].IsEnabled = true;//设置按钮可用
                                            transitionChiOnline[i][j] = Transition(transitionChiOnline[i][j]);
                                        }
                                        transitionChiOnline[i].Sort();
                                        for (int j = 0; j < 3; j++)
                                        {
                                            //设置按钮牌图片
                                            chiChoice[i][j].Content = new Image
                                            {
                                                Source = totalDiscardedCard[1][transitionChiOnline[i][j]]
                                            };
                                            chiChoice[i][j].Visibility = Visibility.Visible;
                                        }
                                    }

                                }
                                if (game.player[0].CheckPeng(discardThisRound))//检查是否能碰
                                {
                                    guoChiPengGangHu.Add(2);
                                    waitUserOtherChoice = true;
                                }
                                if (game.player[0].CheckGang(discardThisRound))//检查是否能杠
                                {
                                    guoChiPengGangHu.Add(3);
                                    waitUserOtherChoice = true;
                                }
                                if (game.player[0].CheckWin(discardThisRound))//检查是否能胡
                                {
                                    guoChiPengGangHu.Add(4);
                                    waitUserOtherChoice = true;
                                }

                                if (guoChiPengGangHu.Count > 1)//大于1说明能吃碰杠胡中的一种或多种
                                {
                                    guoChiPengGangHu.Sort();
                                    waitUserOtherChoice = true;

                                    for (int i = 0; i < guoChiPengGangHu.Count; i++)
                                    {
                                        //设置吃碰杠胡区按钮图片
                                        guoChiPengGangHuBtn[i].Content = new Image
                                        {
                                            Source = guoChiPengGangHuPic[guoChiPengGangHu[i]]
                                        };
                                        guoChiPengGangHuBtn[i].Visibility = Visibility.Visible;
                                    }

                                    if (waitUserOtherChoice)
                                    {
                                        await WaitForPlayer();
                                    }

                                    if (guoChiPengGangHuChoice == 4)
                                    {
                                        foreach (var btn in guoChiPengGangHuBtn)
                                            btn.Visibility = Visibility.Hidden;
                                        typeHu = 1;
                                        statement = statement + game.player[game.cur].GetName() + "点炮" + game.player[0].GetName();

                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Win_DianPao,name,"",0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                        return;
                                    }
                                    else if (guoChiPengGangHuChoice == 3)
                                    {
                                        //game.player[serial].Gang(discardThisRound, 1);
                                        //PengGangSelf(false, Transition(discardThisRound));
                                        //discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                        foreach (var btn in guoChiPengGangHuBtn)
                                            btn.Visibility = Visibility.Hidden;

                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Gang_FromOthers, name, "", 0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    }
                                    else if (guoChiPengGangHuChoice == 2)
                                    {
                                        //game.player[serial].Peng(discardThisRound);
                                        //PengGangSelf(true, Transition(discardThisRound));
                                        //discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                        foreach (var btn in guoChiPengGangHuBtn)
                                            btn.Visibility = Visibility.Hidden;

                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Peng, name, "", 0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    }
                                    else if (guoChiPengGangHuChoice == 1)//吃
                                    {
                                        if (multiChi)//有多种情况
                                        {
                                            //game.player[serial].Chi(chiTempOnline[multiChiChoice]);//更新手牌
                                            //ChiSelf(transitionChiOnline[multiChiChoice]);//更新UI
                                            for (int i = 0; i < chiTempOnline.Count; i++)
                                            {
                                                for (int j = 0; j < 3; j++)
                                                {
                                                    chiChoice[i][j].Content = null;//设置图片为空
                                                    chiChoice[i][j].IsEnabled = false;//设置按钮不可用
                                                    chiChoice[i][j].Visibility = Visibility.Hidden;
                                                }
                                            }
                                        }
                                        else//无
                                        {
                                            //game.player[serial].Chi(chiTempOnline[0]);//更新手牌
                                            chiTempOnline[0].Add(discardThisRound);
                                            for (int i = 0; i < 3; i++)
                                                chiTempOnline[0][i] = Transition(chiTempOnline[0][i]);
                                            chiTempOnline[0].Sort();
                                            //ChiSelf(chiTempOnline[0]);//更新UI
                                        }
                                        //discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                        foreach (var btn in guoChiPengGangHuBtn)
                                            btn.Visibility = Visibility.Hidden;
                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Chi, name, "", 0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    }
                                    else if (guoChiPengGangHuChoice == 0)
                                    {
                                        //当用户选择过时，也需要将多吃部分进行修改
                                        for (int i = 0; i < chiTempOnline.Count; i++)
                                        {
                                            for (int j = 0; j < 3; j++)
                                            {
                                                chiChoice[i][j].Content = null;//设置图片为空
                                                chiChoice[i][j].IsEnabled = false;//设置按钮不可用
                                                chiChoice[i][j].Visibility = Visibility.Hidden;
                                            }
                                        }
                                        foreach (var btn in guoChiPengGangHuBtn)
                                            btn.Visibility = Visibility.Hidden;

                                        ChatMessage chatMessage1 = new ChatMessage((int)MessageType.Pass, name, "", 0);
                                        await Connect.ws.SendMessagesAsyncs(chatMessage1.ToJson());
                                    }
                                    return;
                                }
                            }
                            ChatMessage chatMessage = new ChatMessage((int)MessageType.Pass, name, "", 0);
                            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
                        }
                    }
                    break;
                //case (int)MessageType.Chi:
                //    {
                //        int receiverId = responseMessage.Receiver;
                //        int transCur = TransitionCur(cur);
                //        direction[transCur].Content = new Image
                //        {
                //            Source = directionPic[transCur]
                //        };
                //        cur = receiverId;
                //        transCur = TransitionCur(cur);
                //        direction[transCur].Content = new Image
                //        {
                //            Source = directionPic[transCur]
                //        };
                //        if (responseMessage.Sender == serial)
                //        {
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur]
                //            };
                //            cur = game.cur;
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur + 4]
                //            };
                //            ChiSelf();
                //            waitUserChoice = true;
                //            if (waitUserChoice)
                //            {
                //                await WaitForPlayer();
                //            }
                //            KeyValuePair<int, int> discardThisRound = new KeyValuePair<int, int>();
                //            if (discardIndex >= 0 && discardIndex < game.player[serial].OwnCard.Count)
                //                discardThisRound = new KeyValuePair<int, int>(handCard[discardIndex].Key, handCard[discardIndex].Value);
                //            game.player[serial].Discard(discardThisRound.Value);//出牌

                //            ChatMessage chatMessage = new ChatMessage((int)MessageType.Discard, $"{serial}", " ", 0);
                //            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
                //        }
                //        else
                //        {
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur]
                //            };
                //            cur = game.cur;
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur + 4]
                //            };
                //            ChiOthers();
                //        }
                //    }
                //    break;
                //case (int)MessageType.Peng:
                //    {
                //        if (responseMessage.Sender == serial)
                //        {
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur]
                //            };
                //            cur = game.cur;
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur + 4]
                //            };
                //            PengGangSelf();
                //            waitUserChoice = true;
                //            if (waitUserChoice)
                //            {
                //                await WaitForPlayer();
                //            }
                //            KeyValuePair<int, int> discardThisRound = new KeyValuePair<int, int>();
                //            if (discardIndex >= 0 && discardIndex < game.player[serial].OwnCard.Count)
                //                discardThisRound = new KeyValuePair<int, int>(handCard[discardIndex].Key, handCard[discardIndex].Value);
                //            game.player[serial].Discard(discardThisRound.Value);//出牌

                //            ChatMessage chatMessage = new ChatMessage((int)MessageType.Discard, $"{serial}", " ", 0);
                //            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
                //        }
                //        else
                //        {
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur]
                //            };
                //            cur = game.cur;
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur + 4]
                //            };
                //            PengGangOthers();
                //        }
                //    }
                //    break;
                //case (int)MessageType.Gang_Zimo:
                //    {
                //        if (responseMessage.Sender != serial)
                //        {
                //            PengGangOthers();
                //        }
                //    }
                //    break;
                //case (int)MessageType.Gang_JiaGang:
                //    {
                //        if (responseMessage.Sender != serial)
                //        {
                //            AddGangOthers();
                //        }
                //    }
                //    break;
                //case (int)MessageType.Gang_FromOthers:
                //    {
                //        if (responseMessage.Sender == serial)
                //        {
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur]
                //            };
                //            cur = game.cur;
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur + 4]
                //            };
                //            PengGangSelf();
                //            waitUserChoice = true;
                //            if (waitUserChoice)
                //            {
                //                await WaitForPlayer();
                //            }
                //            KeyValuePair<int, int> discardThisRound = new KeyValuePair<int, int>();
                //            if (discardIndex >= 0 && discardIndex < game.player[serial].OwnCard.Count)
                //                discardThisRound = new KeyValuePair<int, int>(handCard[discardIndex].Key, handCard[discardIndex].Value);
                //            game.player[serial].Discard(discardThisRound.Value);//出牌

                //            ChatMessage chatMessage = new ChatMessage((int)MessageType.Discard, $"{serial}", " ", 0);
                //            await Connect.ws.SendMessagesAsyncs(chatMessage.ToJson());
                //        }
                //        else
                //        {
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur]
                //            };
                //            cur = game.cur;
                //            direction[cur].Content = new Image
                //            {
                //                Source = directionPic[cur + 4]
                //            };
                //            PengGangOthers();
                //        }
                //    }
                //    break;
                //case (int)MessageType.Win_Zimo:
                //    {
                //        if (responseMessage.Sender == serial)
                //        {
                //            settlementPic.Content = new Image
                //            {
                //                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/win.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        else
                //        {
                //            settlementPic.Content = new Image
                //            {
                //                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/lose.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        ResetCheckoutUI();
                //    }
                //    break;
                //case (int)MessageType.Win_DianPao:
                //    {
                //        if (responseMessage.Sender == serial)
                //        {
                //            settlementPic.Content = new Image
                //            {
                //                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/win.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        else
                //        {
                //            settlementPic.Content = new Image
                //            {
                //                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/lose.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        ResetCheckoutUI();
                //    }
                //    break;
                //case (int)MessageType.Liuju:
                //    {
                //        settlementPic.Content = new Image
                //        {
                //            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/dogfall.png")),
                //            Stretch = Stretch.Uniform
                //        };
                //        statement += "牌堆已空";
                //        ResetCheckoutUI();
                //    }
                //    break;
            }
        }

        public void InitMainFrameUI()
        {
            totalCardBox = new Grid
            {
                //Width = 1035,
                //Height = 618
            };
            // 设置列和行的伸展比例
            for (int i = 0; i < 38; i++)
            {
                totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < 27; i++)
            {
                totalCardBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            backToMainPage = new Button
            {
                Width = 30,
                Height = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/back.png"))
                },
                Background = Brushes.Transparent
            };
            backToMainPage.Click += BackToMainPageButtonClick;
            Grid.SetRow(backToMainPage, 1);
            Grid.SetColumn(backToMainPage, 1);
            Grid.SetRowSpan(backToMainPage, 1);
            Grid.SetColumnSpan(backToMainPage, 1);
            totalCardBox.Children.Add(backToMainPage);

            this.Content = totalCardBox;
        }

        public void InitCardUI()
        {
            // 创建 GridLayout 控件，并设置大小
            //totalCardBox = new Grid
            //{
            //    //Width = 1035,
            //    //Height = 618
            //};

            // 初始化 othersCardBox（其他玩家卡片布局）
            othersCardBox = new List<StackPanel>
            {
                new StackPanel { Orientation = Orientation.Vertical ,HorizontalAlignment=HorizontalAlignment.Center  },  // 右
                new StackPanel { Orientation = Orientation.Horizontal ,VerticalAlignment=VerticalAlignment.Center }, // 上
                new StackPanel { Orientation = Orientation.Vertical ,HorizontalAlignment=HorizontalAlignment.Center }    // 左
            };

            selfCardBox = new StackPanel { Orientation = Orientation.Horizontal };

            // 初始化 othersCard（其他玩家卡片的标签）
            //othersCard = new List<List<Label>>();
            //for (int i = 0; i < 3; i++)
            //{
            //    othersCard.Add(new List<Label>());
            //}
            othersCard = new List<List<Image>>();
            for (int i = 0; i < 3; i++)
            {
                othersCard.Add(new List<Image>());
            }

            // 加载图片资源
            othersCardImages = new List<BitmapImage>
            {
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/right_normal.png")),
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/back.png")),
                new BitmapImage(new Uri("pack://application:,,,/Resources/Images/left_normal.png"))
            };

            biaoZhi = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/xia_biao.png"));

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

            //// 设置列和行的伸展比例
            ////totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //for (int i = 0; i < 38; i++)
            //{
            //    //totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //    totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //}
            ////totalCardBox.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ////totalCardBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //for (int i = 0; i < 27; i++)
            //{
            //    //totalCardBox.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //    totalCardBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //}
            //totalCardBox.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // 添加布局到 Grid
            totalCardBox.Children.Add(othersCardBox[0]);
            Grid.SetRow(othersCardBox[0], 4);
            Grid.SetColumn(othersCardBox[0], 31);
            Grid.SetRowSpan(othersCardBox[0], 19);
            //Grid.SetColumnSpan(othersCardBox[0], 1);
            Grid.SetColumnSpan(othersCardBox[0], 2);

            totalCardBox.Children.Add(othersCardBox[1]);
            Grid.SetRow(othersCardBox[1], 1);
            Grid.SetColumn(othersCardBox[1], 9);
            Grid.SetRowSpan(othersCardBox[1], 2);
            Grid.SetColumnSpan(othersCardBox[1], 20);

            totalCardBox.Children.Add(othersCardBox[2]);
            Grid.SetRow(othersCardBox[2], 4);
            //Grid.SetColumn(othersCardBox[2], 5);
            Grid.SetColumn(othersCardBox[2], 4);
            Grid.SetRowSpan(othersCardBox[2], 19);
            //Grid.SetColumnSpan(othersCardBox[2], 1);
            Grid.SetColumnSpan(othersCardBox[2], 2);

            totalCardBox.Children.Add(selfCardBox);
            Grid.SetRow(selfCardBox, 24);
            Grid.SetColumn(selfCardBox, 9);
            Grid.SetRowSpan(selfCardBox, 3);
            Grid.SetColumnSpan(selfCardBox, 20);

            // 添加剩余牌数标签
            remained = new Label
            {
                //Content = remainedText + "150", // 示例，实际需要动态更新
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGoldenrodYellow),
                Width = 170,
                Height = 30,
                FontFamily = new System.Windows.Media.FontFamily("幼圆"),
                FontSize = 17
            };

            Grid.SetRow(remained, 1);
            Grid.SetColumn(remained, 34);
            Grid.SetRowSpan(remained, 1);
            Grid.SetColumnSpan(remained, 4);

            totalCardBox.Children.Add(remained);

            //backToMainPage = new Button
            //{
            //    Width = 30,
            //    Height = 30,
            //    HorizontalContentAlignment = HorizontalAlignment.Center,
            //    VerticalContentAlignment = VerticalAlignment.Center,
            //    Margin = new Thickness(0),
            //    Padding = new Thickness(0), // 去除 Padding
            //    BorderThickness = new Thickness(0), // 去除边框厚度
            //    Content =new Image
            //    {
            //        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/back.png"))
            //    },
            //    Background=Brushes.Transparent
            //};
            //backToMainPage.Click += BackToMainPageButtonClick;
            //Grid.SetRow(backToMainPage, 1);
            //Grid.SetColumn(backToMainPage, 1);
            //Grid.SetRowSpan(backToMainPage, 1);
            //Grid.SetColumnSpan(backToMainPage, 1);
            //totalCardBox.Children.Add(backToMainPage);

            // 将整个 UI 设置为布局
            //this.Content = totalCardBox;
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

        private void selfCardTransitionOnline()
        {
            handCard.Clear();
            var gg = game.player[serial].OwnCard;
            Console.WriteLine(gg);
            foreach (var card in game.player[serial].OwnCard)
            {
                handCard.Add(new KeyValuePair<int, int>(Transition(card), card));
            }

            // 排序
            handCard.Sort((a, b) => a.Key.CompareTo(b.Key));
        }

        // 创建玩家自己的卡牌
        private async void createSelfCard(int index)
        {
            // 创建 StackPanel 来代替 QVBoxLayout
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // 创建一个 Image 用于显示卡牌图像
            var label = new Label
            {
                Width = 9,
                Height = 10,
                //Background=Brushes.AliceBlue,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
            };
            var image = new Image
            {
                Width = 9,
                Height = 10
            };
            label.Content = image;

            // 创建自定义的按钮模板
            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button))
            {
                VisualTree = new FrameworkElementFactory(typeof(Border))
            };

            // 获取 Border
            FrameworkElementFactory borderFactory = buttonTemplate.VisualTree;
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(2));

            // 将 Border 的内容设置为按钮的内容
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            borderFactory.AppendChild(contentPresenter);

            // 创建按钮
            var button = new Button
            {
                Width = 37,
                Height = 53,
                Content = new Image(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                Template = buttonTemplate,
                Tag = index
            };


            selfCardTransition();

            if (index >= 0 && index < 13)
            {
                int siz = handCard.Count;
                var cardIndex = handCard[index].Key;
                var cardImage = totalCardSelf[cardIndex];
                Task.Run(() =>
                {
                    button.Dispatcher.BeginInvoke((() =>
                    {
                        button.Content = new Image
                        {
                            Source = cardImage,
                            Stretch = Stretch.UniformToFill
                        };
                        //(button.Content as Image).Source = cardImage;
                    }));
                }).Wait();

            }

            button.Click += SelfCardButtonClick; // 可选的按钮点击事件处理

            // 将控件添加到 StackPanel
            stackPanel.Children.Add(label);
            stackPanel.Children.Add(button);

            selfCard.Add(stackPanel);
            selfCardButton.Add(button);
            selfCardLabel.Add(label);
            selfCardBox.Children.Add(selfCard[index]);
        }

        private async void SelfCardButtonClick(object sender, RoutedEventArgs e)
        {
            // 处理按钮点击事件
            // 用户点击按钮后，完成任务，程序继续
            // 检查按钮是否被点击且处于选中状态
            if (statu)
            {
                var button = sender as Button;

                if (choiceBtn == (int)button.Tag)
                {
                    if (!waitUserChoice)
                    {
                        return;
                    }
                    //await Task.Run(() =>
                    //{
                    //    selfCardLabel[choiceBtn].Dispatcher.Invoke(() =>
                    //    {
                    selfCardLabel[choiceBtn].Content = null;
                    //    });
                    //});
                    waitUserChoice = false;
                    discardIndex = choiceBtn;
                    choiceBtn = -1;

                    if (tcs != null)
                    {
                        tcs.SetResult(1); // 设置结果，结束等待
                        tcs = null; // 任务完成后，清空 tcs 以便下一次使用
                    }
                }
                else
                {
                    if (choiceBtn != -1)
                        //await Task.Run(() =>
                        //{
                        //    selfCardLabel[choiceBtn].Dispatcher.Invoke(() =>
                        //    {
                        selfCardLabel[choiceBtn].Content = null;
                    //    });
                    //});
                    choiceBtn = (int)button.Tag;
                    //await Task.Run(() =>
                    //{
                    //Dispatcher.Invoke(() =>
                    //{
                    selfCardLabel[choiceBtn].Content = new Image
                    {
                        Source = biaoZhi,
                        Stretch = Stretch.Fill
                    };
                    //    });
                    //});
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 处理按钮点击事件
        }

        // 创建其他玩家的卡牌
        private void createOtherCard(int type, BitmapImage pic, int width, int height, int index)
        {
            //var label = new Label
            //{
            //    Width = width,
            //    Height = height,
            //    Content = new Image { Source = pic, Stretch = Stretch.Uniform },
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Center,
            //    Margin = new Thickness(0),
            //    Padding = new Thickness(0), // 去除 Padding
            //    //BorderBrush = Brushes.Transparent, // 去除边框
            //    BorderThickness = new Thickness(0), // 去除边框厚度

            //    Background =Brushes.AliceBlue
            //};

            var image = new Image
            {
                Width = width,
                Height = height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Source = pic
            };

            // 将标签添加到其他卡片集合中
            //othersCard[type].Add(label);
            othersCard[type].Add(image);
            othersCardBox[type].Children.Add(image);
            //othersCardBox[type].Children.Add(label);
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
            //

            // 在自己卡片区添加间隔
            selfCardBox.Children.Add(new Button { Content = "Space", Visibility = Visibility.Hidden, Width = 15, Height = 15 }); // 可自定义空白控件
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
                    width = 37;
                    length = 53;
                }
                else
                {
                    width = 35;
                    length = 25;
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
                othersCardBox[i].Children.Add(new Button { Content = "Space", Visibility = Visibility.Hidden, Width = 15, Height = 15 }); // 可自定义空白控件
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
            chiPengGangBox = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

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
                button.Click += ChiPengGangButtonClick;
            }

            // 创建吃碰杠按钮
            for (int i = 0; i < 5; i++)
            {
                CreateChiPengGangBtn(i); // 创建按钮并加入布局
            }
            for (int i = 0; i < 4; i++)
            {
                pengAlready.Add(new Dictionary<int, int>());
            }


            for (int i = 0; i < 3; i++)
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
                        Width = 17,
                        Height = 25,
                        IsEnabled = true,
                        Background = Brushes.Transparent,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(1),
                        Padding = new Thickness(0), // 去除 Padding
                        BorderThickness = new Thickness(0), // 去除边框厚度
                    };

                    button.Tag = i;

                    button.Visibility = Visibility.Hidden;

                    multiChiChoiceBtn.AddButton(button, i); // 将按钮加入 ButtonGroup
                    chiChoice[i].Add(button);
                }
            }

            // 连接多选按钮的点击事件
            foreach (var button in multiChiChoiceBtn.Buttons)
            {
                button.Click += MultiChiButtonClick;
            }

            // 将按钮添加到布局中
            chiPengGangBox.Children.Add(new UIElement()); // 占位符，适配原布局的空白

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // 使用 Margin 控制按钮之间的间距
                    chiChoice[i][j].Margin = new Thickness(0);  // 设置按钮之间的间距
                    chiPengGangBox.Children.Add(chiChoice[i][j]);
                }
                chiChoice[i][2].Margin = new Thickness(0, 0, 10, 0);

                if (i != 2)
                    chiPengGangBox.Children.Add(new UIElement()); // 空白
            }

            chiPengGangBox.Children.Add(new Label { Margin = new Thickness(100) });

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
            //var wrapPanel = new WrapPanel();
            //wrapPanel.Children.Add(chiPengGangBox);

            //Grid.SetRow(wrapPanel, 23);
            //Grid.SetColumn(wrapPanel, 9);
            //Grid.SetRowSpan(wrapPanel, 1);
            //Grid.SetColumnSpan(wrapPanel, 20);

            //// 将布局添加到父布局中（假设 totalCardBox 是你的父布局）
            //totalCardBox.Children.Add(wrapPanel);  // 使用 wrapPanel 而不是直接使用 StackPanel
            Grid.SetRow(chiPengGangBox, 22);
            Grid.SetColumn(chiPengGangBox, 9);
            Grid.SetRowSpan(chiPengGangBox, 2);
            Grid.SetColumnSpan(chiPengGangBox, 20);

            // 将布局添加到父布局中（假设 totalCardBox 是你的父布局）
            totalCardBox.Children.Add(chiPengGangBox);  // 使用 wrapPanel 而不是直接使用 StackPanel
        }


        private void MultiChiButtonClick(object sender, RoutedEventArgs e)
        {
            // 处理按钮点击事件
            if (multiChi)
            {
                var button = sender as Button;
                int btnId = (int)button.Tag;
                if (waitUserOtherChoice)
                {
                    if (btnId == 0)
                    {
                        multiChiChoice = 0;
                    }
                    else if (btnId == 1)
                    {
                        multiChiChoice = 1;
                    }
                    else if (btnId == 2)
                    {
                        multiChiChoice = 2;
                    }
                    guoChiPengGangHuChoice = 1;
                    waitUserOtherChoice = false;
                    if (tcs != null)
                    {
                        tcs.SetResult(1); // 设置结果，结束等待
                        tcs = null; // 任务完成后，清空 tcs 以便下一次使用
                    }
                }
            }
        }

        // 创建按钮的方法
        private void CreateChiPengGangBtn(int id)
        {
            // 创建一个 Button，代替 RadioButton
            Button button = new Button
            {
                Width = 30,
                Height = 30,
                Content = new Image
                {
                    Source = new BitmapImage(), // 图像路径，可以根据需要设置具体的图片路径
                    Stretch = Stretch.UniformToFill
                },
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag = id // 将按钮的 ID 存储在 Tag 属性中
            };

            button.Visibility = Visibility.Hidden;

            // 按钮点击事件绑定
            button.Click += ChiPengGangButtonClick;

            // 将 Button 添加到 StackPanel 或者其他容器中
            guoChiPengGangHuBtn.Add(button); // 将 Button 加入到按钮列表中
            buttonGroupChiPengPang.AddButton(button, id);
        }

        private void ChiPengGangButtonClick(object sender, RoutedEventArgs e)
        {
            // 处理按钮点击事件
            var button = sender as Button;
            int btnId = (int)button.Tag;
            if (btnId < guoChiPengGangHu.Count && waitUserOtherChoice)
            {
                if (guoChiPengGangHu[btnId] == 0)
                {
                    guoChiPengGangHuChoice = 0;
                    waitUserOtherChoice = false;
                }
                if (guoChiPengGangHu[btnId] == 1)
                {
                    if (multiChi)
                    {
                        return;
                    }
                    guoChiPengGangHuChoice = 1;
                    waitUserOtherChoice = false;
                }
                if (guoChiPengGangHu[btnId] == 2)
                {
                    guoChiPengGangHuChoice = 2;
                    waitUserOtherChoice = false;
                }
                if (guoChiPengGangHu[btnId] == 3)
                {
                    guoChiPengGangHuChoice = 3;
                    waitUserOtherChoice = false;
                }
                if (guoChiPengGangHu[btnId] == 4)
                {
                    guoChiPengGangHuChoice = 4;
                    waitUserOtherChoice = false;
                }
                if (tcs != null)
                {
                    tcs.SetResult(1); // 设置结果，结束等待
                    tcs = null; // 任务完成后，清空 tcs 以便下一次使用
                }
            }
        }

        public void ChiSelf(List<int> chiCard)
        {
            if (choiceBtn != -1)
            {
                selfCardLabel[choiceBtn].Content = null; // 清空选中的Label
                choiceBtn = -1;
            }

            // 移除按钮和控件
            for (int i = 0; i < 3; ++i)
            {
                var lastIndex = selfCardButton.Count - 2;

                // 移除自定义的控件和按钮
                selfCardBox.Children.Remove(selfCard[selfCard.Count - 2]);
                selfCardButton[selfCardButton.Count - 2].Content = null;
                selfCardButton[selfCardButton.Count - 2].Click -= SelfCardButtonClick; // 如果有事件绑定需要解除
                selfCardButton.RemoveAt(selfCardButton.Count - 2);

                selfCardLabel[selfCardLabel.Count - 2].Content = null;
                selfCardLabel.RemoveAt(selfCardLabel.Count - 2);

                selfCard.RemoveAt(selfCard.Count - 2);
            }

            // 插入间距
            selfCardBox.Children.Add(new Label { Margin = new Thickness(5) });

            // 插入新的图片标签
            for (int i = 0; i < 3; ++i)
            {
                var label = new Label
                {
                    Width = 27,
                    Height = 39,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0), // 去除 Padding
                    BorderThickness = new Thickness(0), // 去除边框厚度
                    Content = new Image
                    {
                        Source = totalDiscardedCard[1][chiCard[i]],
                        Stretch = Stretch.Uniform,
                    }
                };
                selfCardBox.Children.Add(label);
            }
            selfCardButton[selfCardButton.Count - 1].Tag = selfCardButton.Count - 1;

            // 更新 UI
            Dispatcher.Invoke(() =>
            {
                //强制刷新 UI 界面
                selfCardBox.UpdateLayout();
            });
        }

        public void ChiOthers(int type, List<int> chiCard)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (othersCard[type].Count > 2)
                {
                    var label = othersCard[type][othersCard[type].Count - 2];
                    othersCard[type].RemoveAt(othersCard[type].Count - 2);
                    othersCardBox[type].Children.Remove(label);
                }
            }

            // 插入间距
            othersCardBox[type].Children.Add(new Label { Margin = new Thickness(5) });

            for (int i = 0; i < 3; ++i)
            {
                //var label = new Label
                //{
                //    HorizontalContentAlignment = HorizontalAlignment.Center,
                //    VerticalContentAlignment = VerticalAlignment.Center,
                //    Margin = new Thickness(0),
                //    Padding = new Thickness(0), // 去除 Padding
                //    BorderThickness = new Thickness(0), // 去除边框厚度
                //};
                var image = new Image
                {
                    Width = (type == 1) ? 27 : 30,
                    Height = (type == 1) ? 39 : 22,
                    //Width = 27,
                    //Height = 39,
                    Source = totalDiscardedCard[type % 2][chiCard[i]],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Uniform,
                    RenderTransform = new RotateTransform((type + 1) / 2 * (-180)),
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    Margin = new Thickness(0),
                };
                //if (type == 1)
                //{
                //    //label.Width = 27;
                //    //label.Height = 39;
                //    //label.Content = new Image
                //    //{
                //    //    Source = totalDiscardedCard[chiCard[i]],
                //    //    Stretch = Stretch.Uniform,
                //    //    RenderTransform = new RotateTransform((type + 1) * (-90)),
                //    //    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                //    //};
                //    image.Width = 27;
                //    image.Height = 39;
                //    image.Source = totalDiscardedCard[chiCard[i]];
                //    image.RenderTransform = new RotateTransform((type + 1) * (-90));
                //    image.RenderTransformOrigin = new Point(0.5, 0.5);
                //}
                //else
                //{
                //    //label.Width = 30;
                //    //label.Height = 22;
                //    //label.Content = new Image
                //    //{
                //    //    Source = totalDiscardedCard[chiCard[i]],
                //    //    Stretch = Stretch.Uniform,
                //    //    RenderTransform = new RotateTransform((type + 1) * (-90)),
                //    //    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                //    //};
                //    image.Width = 30;
                //    image.Height = 22;
                //    image.Source = totalDiscardedCard[chiCard[i]];
                //    image.RenderTransform = new RotateTransform((type + 1) * (-90));
                //    image.RenderTransformOrigin = new Point(0.5, 0.5);
                //}
                //othersCardBox[type].Children.Add(label);
                othersCardBox[type].Children.Add(image);
            }

            // 强制刷新 UI
            Dispatcher.Invoke(() =>
            {
                othersCardBox[type].UpdateLayout();
            });
        }

        public void PengGangSelf(bool choice, int pengGangCard)
        {
            if (choiceBtn != -1)
            {
                // 清除当前选择的按钮
                selfCardLabel[choiceBtn].Content = null; // 清除内容
                choiceBtn = -1;
            }

            // 根据 choice 决定是碰（3张卡）还是杠（4张卡）
            int num = choice ? 3 : 4;

            // 移除按钮和控件
            for (int i = 0; i < 3; ++i)
            {
                int lastIndex = selfCardButton.Count - 2;

                // 移除控件和按钮
                selfCardBox.Children.Remove(selfCard[selfCard.Count - 2]);

                selfCardButton[selfCardButton.Count - 2].Content = null;
                selfCardButton[selfCardButton.Count - 2].Click -= SelfCardButtonClick; // 如果有事件绑定，需要解除绑定
                selfCardButton.RemoveAt(selfCardButton.Count - 2);

                selfCardLabel[selfCardLabel.Count - 2].Content = null;
                selfCardLabel.RemoveAt(selfCardLabel.Count - 2);

                selfCard.RemoveAt(selfCard.Count - 2);

            }

            // 添加间距
            selfCardBox.Children.Add(new Label { Margin = new Thickness(5) });

            // 添加新的图片标签
            for (int i = 0; i < num; ++i)
            {
                var label = new Label
                {
                    Width = 27,
                    Height = 39,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0), // 去除 Padding
                    BorderThickness = new Thickness(0), // 去除边框厚度
                    Content = new Image
                    {
                        Source = totalDiscardedCard[1][pengGangCard],
                        Stretch = Stretch.Uniform,
                    }
                };
                selfCardBox.Children.Add(label);

                // 更新pengAlready
                if (choice && i == 0)
                {
                    pengAlready[0][pengGangCard] = selfCardBox.Children.IndexOf(label) - selfCardButton.Count;
                }
            }

            selfCardButton[selfCardButton.Count - 1].Tag = selfCardButton.Count - 1;

            // 强制刷新 UI
            Dispatcher.Invoke(() =>
            {
                selfCardBox.UpdateLayout();
            });
        }

        public void PengGangOthers(bool choice, int type, int pengGangCard)
        {
            int num = choice ? 3 : 4; // 根据选择来确定数字

            for (int i = 0; i < 3; ++i)
            {
                if (othersCard[type].Count > 2)
                {
                    var label = othersCard[type][othersCard[type].Count - 2];
                    othersCard[type].RemoveAt(othersCard[type].Count - 2);
                    othersCardBox[type].Children.Remove(label);
                }
            }

            // 插入间距
            othersCardBox[type].Children.Add(new Label { Margin = new Thickness(5) });

            for (int i = 0; i < num; ++i)
            {
                //var label = new Label
                //{
                //    HorizontalContentAlignment = HorizontalAlignment.Center,
                //    VerticalContentAlignment = VerticalAlignment.Center,
                //    Margin = new Thickness(0),
                //    Padding = new Thickness(0), // 去除 Padding
                //    BorderThickness = new Thickness(0), // 去除边框厚度
                //};
                var image = new Image
                {
                    Width = (type == 1) ? 27 : 30,
                    Height = (type == 1) ? 39 : 22,
                    //Width = 27,
                    //Height = 39,
                    Source = totalDiscardedCard[type % 2][pengGangCard],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Uniform,
                    RenderTransform = new RotateTransform((type + 1) / 2 * (-180)),
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    Margin = new Thickness(0),
                };
                //var image = new Image
                //{
                //    Margin = new Thickness(0),
                //};
                //if (type == 1)
                //{
                //    //label.Width = 27;
                //    //label.Height = 39;
                //    //label.Content = new Image
                //    //{
                //    //    Source = totalDiscardedCard[pengGangCard],
                //    //    Stretch = Stretch.Uniform,
                //    //    RenderTransform = new RotateTransform((type + 1) * (-90)),
                //    //    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                //    //};
                //    image.Width = 27;
                //    image.Height = 39;
                //    image.Source = totalDiscardedCard[pengGangCard];
                //    image.RenderTransform = new RotateTransform((type + 1) * (-90));
                //    image.RenderTransformOrigin = new Point(0.5, 0.5);
                //}
                //else
                //{
                //    //label.Width = 30;
                //    //label.Height = 22;
                //    //label.Content = new Image
                //    //{
                //    //    Source = totalDiscardedCard[pengGangCard],
                //    //    Stretch = Stretch.Uniform,
                //    //    RenderTransform = new RotateTransform((type + 1) * (-90)),
                //    //    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                //    //};
                //    image.Width = 30;
                //    image.Height = 22;
                //    image.Source = totalDiscardedCard[pengGangCard];
                //    image.RenderTransform = new RotateTransform((type + 1) * (-90));
                //    image.RenderTransformOrigin = new Point(0.5, 0.5);
                //}
                //othersCardBox[type].Children.Add(label);
                othersCardBox[type].Children.Add(image);
                if (choice && i == 1)
                {
                    //pengAlready[type][pengGangCard] = othersCardBox[type].Children.IndexOf(label) - othersCard[type].Count;
                    pengAlready[type][pengGangCard] = othersCardBox[type].Children.IndexOf(image) - othersCard[type].Count;
                }
            }

            // 强制刷新界面更新
            Dispatcher.Invoke(() =>
            {
                selfCardBox.UpdateLayout();
            });
        }

        public void AddGangSelf(int pengGangCard)
        {
            // 计算插入位置
            if (!pengAlready[0].ContainsKey(pengGangCard))
            {
                pengAlready[0][pengGangCard] = 0;
            }
            int index = pengAlready[0][pengGangCard] + selfCardButton.Count;

            var label = new Label
            {
                Width = 27,
                Height = 39,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                Content = new Image
                {
                    Source = totalDiscardedCard[1][pengGangCard],
                    Stretch = Stretch.Uniform,
                }
            };

            selfCardBox.Children.Insert(index, label);

            Dispatcher.Invoke(() =>
            {
                selfCardBox.UpdateLayout();
            });
        }

        public void AddGangOthers(int type, int pengGangCard)
        {
            if (!pengAlready[type].ContainsKey(pengGangCard))
            {
                pengAlready[type][pengGangCard] = 0;
            }
            // 计算要插入的位置
            int index = pengAlready[type][pengGangCard] + othersCard[type].Count;

            //// 创建 Image 控件，并设置其大小
            //Label label = new Label
            //{
            //    Width = (type == 1) ? 27 : 30,
            //    Height = (type == 1) ? 39 : 22,
            //    HorizontalContentAlignment = HorizontalAlignment.Center,
            //    VerticalContentAlignment = VerticalAlignment.Center,
            //    Margin = new Thickness(0),
            //    Padding = new Thickness(0), // 去除 Padding
            //    BorderThickness = new Thickness(0), // 去除边框厚度
            //    Content = new Image
            //    {
            //        Source = totalDiscardedCard[pengGangCard],
            //        Stretch = Stretch.Uniform,
            //        RenderTransform = new RotateTransform((type + 1) * (-90)),
            //        RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
            //    }
            //};
            // 创建 Image 控件，并设置其大小
            Image image = new Image
            {
                Width = (type == 1) ? 27 : 30,
                Height = (type == 1) ? 39 : 22,
                //Width = 27,
                //Height = 39,
                Margin = new Thickness(0),
                Source = totalDiscardedCard[type % 2][pengGangCard],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Uniform,
                RenderTransform = new RotateTransform((type + 1) / 2 * (-180)),
                RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
            };


            //othersCardBox[type].Children.Insert(index, label);
            othersCardBox[type].Children.Insert(index, image);
            Dispatcher.Invoke(() =>
            {
                selfCardBox.UpdateLayout();
            });
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
            //discarded = new List<List<Label>>();
            //for (int i = 0; i < 4; i++)
            //{
            //    discarded.Add(new List<Label>());
            //}
            discarded = new List<List<Image>>();
            for (int i = 0; i < 4; i++)
            {
                discarded.Add(new List<Image>());
            }

            discardedNowIndex = new List<int> { 0, 0, 0, 0 };

            // 上方丢弃的卡牌
            for (int i = 7, j = 23, index = 0; i >= 5 && j >= 13; --j, ++index)
            {
                var label = new Label
                {
                    Width = 22,
                    Height = 30,
                    //Background = Brushes.Transparent,  // 使用背景色作为占位符
                    Background = Brushes.Black,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new Image
                    {

                    },
                    Margin = new Thickness(0),
                    Padding = new Thickness(0), // 去除 Padding
                    BorderThickness = new Thickness(0), // 去除边框厚度
                                                        //Content = new TextBlock
                                                        //{
                                                        //    Text = "占位符", // 可以显示一些文本内容作为占位符
                                                        //    Foreground = Brushes.White
                                                        //}
                };

                label.Content = null;
                //label.Content = new Image
                //{
                //    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/chi_peng_gang_hu/chi.png")),

                //};

                //discarded[2].Add(label);
                Image image = new Image
                {
                    Width = 22,
                    Height = 29,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    //Margin = new Thickness(0,1,0,1),
                    Margin = new Thickness(0),
                    Source = null,
                    Stretch = Stretch.Uniform

                };
                discarded[2].Add(image);

                // 在 Grid 中添加控件，假设 `totalCardBox` 是一个 Grid 控件
                //totalCardBox.Children.Add(label);
                //Grid.SetRow(label, i);   // 设置行
                //Grid.SetColumn(label, j); // 设置列
                totalCardBox.Children.Add(image);
                Grid.SetRow(image, i);   // 设置行
                Grid.SetColumn(image, j); // 设置列

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
                    Width = 30,
                    Height = 22,
                    //Background = Brushes.Transparent,  // 使用背景色作为占位符
                    Background = Brushes.Black,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0), // 去除 Padding
                    BorderThickness = new Thickness(0), // 去除边框厚度
                                                        //Content = new TextBlock
                                                        //{
                                                        //    Text = "占位符", // 可以显示一些文本内容作为占位符
                                                        //    Foreground = Brushes.White
                                                        //}
                };

                //discarded[3].Add(label);
                Image image = new Image
                {
                    Width = 30,
                    Height = 22,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Source = null,
                    Stretch = Stretch.Uniform
                };
                discarded[3].Add(image);

                //totalCardBox.Children.Add(label);
                //Grid.SetRow(label, i);    // 设置行
                //Grid.SetColumn(label, j); // 设置列
                totalCardBox.Children.Add(image);
                Grid.SetRow(image, i);   // 设置行
                Grid.SetColumn(image, j); // 设置列

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
                    Width = 30,
                    Height = 22,
                    //Background = Brushes.Transparent,  // 使用背景色作为占位符
                    Background = Brushes.Black,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0), // 去除 Padding
                    BorderThickness = new Thickness(0), // 去除边框厚度
                                                        //Content = new TextBlock
                                                        //{
                                                        //    Text = "占位符", // 可以显示一些文本内容作为占位符
                                                        //    Foreground = Brushes.Red
                                                        //}
                };

                //discarded[1].Add(label);
                Image image = new Image
                {
                    Width = 30,
                    Height = 22,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Source = null,
                    Stretch = Stretch.Uniform
                };
                discarded[1].Add(image);

                //totalCardBox.Children.Add(label);
                //Grid.SetRow(label, i);    // 设置行
                //Grid.SetColumn(label, j); // 设置列
                totalCardBox.Children.Add(image);
                Grid.SetRow(image, i);   // 设置行
                Grid.SetColumn(image, j); // 设置列

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
                    Width = 22,
                    Height = 30,
                    //Background = Brushes.Transparent,  // 使用背景色作为占位符
                    Background = Brushes.Black,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0), // 去除 Padding
                    BorderThickness = new Thickness(0), // 去除边框厚度
                                                        //Content = new TextBlock
                                                        //{
                                                        //    Text = "占位符", // 可以显示一些文本内容作为占位符
                                                        //    Foreground = Brushes.White
                                                        //}
                };

                //discarded[0].Add(label);
                Image image = new Image
                {
                    Width = 22,
                    Height = 29,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    //Margin = new Thickness(0,1,0,1),
                    Margin = new Thickness(0),
                    Source = null,
                    Stretch = Stretch.Uniform,
                };
                discarded[0].Add(image);

                //totalCardBox.Children.Add(label);
                //Grid.SetRow(label, i);    // 设置行
                //Grid.SetColumn(label, j); // 设置列
                totalCardBox.Children.Add(image);
                Grid.SetRow(image, i);   // 设置行
                Grid.SetColumn(image, j); // 设置列

                if (j == 23 && i < 18)
                {
                    j = 12;
                    i++;
                }
            }

            // 设置 Grid 的间距（你可以根据需要设置）
            totalCardBox.Margin = new Thickness(0);
        }


        // 初始化结算界面
        private void InitCheckoutUI()
        {
            // 创建 Grid 作为容器
            checkout = new Grid
            {
                //Width = 700,
                //Height = 400,
                Visibility = Visibility.Collapsed
            };

            for (int i = 0; i < 10; i++)
            {
                checkout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < 10; i++)
            {
                checkout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // 设置 Grid 背景为图像
            var pix = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/settlement/Checkout.png"));
            var brush = new ImageBrush(pix)
            {
                Stretch = Stretch.Uniform
            };
            checkout.Background = brush;

            // 创建并设置结算状态的 Label
            settlementPic = new Label
            {
                //Width = 535,
                //Height = 160,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                                                    //Content =new Image 
                                                    //{
                                                    //    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/settlement/win.png")),
                                                    //    Stretch=Stretch.Uniform
                                                    //} 
            };

            Grid.SetRow(settlementPic, 1); // 设置它在 Grid 中的行
            Grid.SetColumn(settlementPic, 1); // 设置它在 Grid 中的列
            Grid.SetRowSpan(settlementPic, 4);
            Grid.SetColumnSpan(settlementPic, 8);
            checkout.Children.Add(settlementPic);


            // 设置结算状态文本
            settlementState = new Label
            {
                FontFamily = new System.Windows.Media.FontFamily("KaiTi"),
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
            };
            Grid.SetRow(settlementState, 5); // 设置它在 Grid 中的行
            Grid.SetColumn(settlementState, 1); // 设置它在 Grid 中的列
            Grid.SetRowSpan(settlementState, 2);
            Grid.SetColumnSpan(settlementState, 8);
            checkout.Children.Add(settlementState);

            // 创建按钮布局
            //checkoutBtn = new StackPanel
            //{
            //    Orientation = Orientation.Horizontal,
            //    HorizontalAlignment = HorizontalAlignment.Center
            //};
            //Grid.SetRow(checkoutBtn, 2); // 设置它在 Grid 中的行
            //Grid.SetColumn(checkoutBtn, 0); // 设置它在 Grid 中的列
            //Grid.SetRowSpan(settlementPic, 3);
            //Grid.SetColumnSpan(settlementPic, 8);

            continueGame = new Button
            {
                Content = "继续游戏",
                //Width = 240,
                //Height = 50,
                FontFamily = new System.Windows.Media.FontFamily("KaiTi"),
                FontSize = 10,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/btn.png")),
                    Stretch = Stretch.Uniform
                }
            };
            //continueGame.Style = (Style)Application.Current.Resources["ButtonStyle"]; // 设置按钮样式
            checkout.Children.Add(continueGame);
            Grid.SetRow(continueGame, 8);
            Grid.SetColumn(continueGame, 1);
            Grid.SetRowSpan(continueGame, 1);
            Grid.SetColumnSpan(continueGame, 3);
            //checkout.Children.Add(continueGame);

            backToMainStage = new Button
            {
                Content = "返回菜单",
                //Width = 240,
                //Height = 50,
                FontFamily = new System.Windows.Media.FontFamily("KaiTi"),
                FontSize = 10,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/btn.png")),
                    Stretch = Stretch.Uniform
                }
            };
            //backToMainStage.Style = (Style)Application.Current.Resources["ButtonStyle"];
            checkout.Children.Add(backToMainStage);
            Grid.SetRow(backToMainStage, 8);
            Grid.SetColumn(backToMainStage, 6);
            Grid.SetRowSpan(backToMainStage, 1);
            Grid.SetColumnSpan(backToMainStage, 3);
            //checkout.Children.Add(backToMainStage);

            // 创建隐藏按钮
            hideCheckOut = new Button
            {
                Width = 20,
                Height = 20,
                Background = Brushes.Transparent,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
                                                    //Content = new Image
                                                    //{
                                                    //    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/eye.png")),
                                                    //    Stretch = Stretch.Uniform
                                                    //}
            };
            hideCheckOut.Click += HideCheckOut_Click;
            Grid.SetRow(hideCheckOut, 26);
            Grid.SetColumn(hideCheckOut, 37);
            Grid.SetRowSpan(hideCheckOut, 1);
            Grid.SetColumnSpan(hideCheckOut, 1);
            totalCardBox.Children.Add(hideCheckOut);

            // 隐藏按钮放置位置2
            Grid.SetRow(checkout, 7);
            Grid.SetColumn(checkout, 12);
            Grid.SetRowSpan(checkout, 10);
            Grid.SetColumnSpan(checkout, 14);
            totalCardBox.Children.Add(checkout);  // 将按钮添加到容器中

            // 注册按钮点击事件
            continueGame.Click += ContinueGame_Click;
            backToMainStage.Click += BackToMainStage_Click;

            //// 添加 checkout 到当前页面
            //this.Content = checkout;
        }

        private void ResetCheckoutUI()
        {
            statu = false;
            hideCheckOut.Content = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/eye.png"))
            };

            settlementState.Content = statement;

            if (choiceBtn != -1)
            {
                selfCardLabel[choiceBtn].Content = null;
                choiceBtn = -1;
            }

            for (int i = 0; i < 3; i++)
            {
                List<int> cardTemp = new List<int>();
                foreach (var card in game.player[i + 1].OwnCard)
                {
                    cardTemp.Add(Transition(card));
                }
                cardTemp.Sort();
                int j = 0;
                foreach (var card in cardTemp)
                {
                    //othersCard[i][j].Content = new Image
                    //{
                    //    Source = totalDiscardedCard[card],
                    //    RenderTransform = new RotateTransform((i + 1) * (-90)),
                    //    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                    //    Stretch = Stretch.Uniform
                    //};
                    othersCard[i][j].Source = totalDiscardedCard[i % 2][card];
                    othersCard[i][j].RenderTransform = new RotateTransform((i + 1) / 2 * (-180));
                    othersCard[i][j].RenderTransformOrigin = new Point(0.5, 0.5);
                    j++;
                }
            }
            checkout.Visibility = Visibility.Visible;
        }


        // 继续游戏按钮点击事件
        private void ContinueGame_Click(object sender, RoutedEventArgs e)
        {
            //// 清除状态和重置游戏
            //statement = "";
            //game = new Game("谭杰");
            //game.DistributeCards();
            //ResetCardUI();
            //ResetDirectionUI();
            //ResetDiscardedUI();
            //hideCheckOut.Visibility = Visibility.Collapsed; // 隐藏按钮
            //checkout.Visibility = Visibility.Collapsed; // 隐藏结算界面
            //StartGame();
            DestroyPageResources();
            GC.Collect();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new GamePage(true, -1));
        }

        // 返回菜单按钮点击事件
        private void BackToMainStage_Click(object sender, RoutedEventArgs e)
        {
            // 清除状态和重置游戏
            //statement = "";
            //game = new Game("谭杰");
            //game.DistributeCards();
            //ResetCardUI();
            //ResetDirectionUI();
            //ResetDiscardedUI();
            //hideCheckOut.Visibility = Visibility.Collapsed; // 隐藏按钮
            //                                                // 跳转到主菜单
            //NavigateToMainPage();
            //checkout.Visibility = Visibility.Collapsed; // 隐藏结算界面
            DestroyPageResources();
            GC.Collect();
            // 获取父窗口中的 Frame 控件
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.mainPage);
        }

        // 隐藏或显示结算界面
        private void HideCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (!statu)
            {
                if (checkout.Visibility == Visibility.Hidden)
                    checkout.Visibility = Visibility.Visible;
                else
                    checkout.Visibility = Visibility.Hidden;
            }
        }

        private void ResetDirectionUI() { }
        private void ResetDiscardedUI() { }

        private void NavigateToMainPage()
        {
            // 这里写主菜单跳转逻辑
        }

        public async void StartGame()
        {
            try
            {
                //Task.Run(() =>
                //    {
                //        Dispatcher.Invoke(() =>
                //        {
                //ResetCheckoutUI();
                isFinish = false;
                if (game.cur == 0)
                    waitUserChoice = true;
                //cout<<"游戏开始,先手为"<<game->player[game->cur].get_name();
                bool isGang = false;//是否杠了
                bool isChiPeng = false;//是否吃碰
                while (!game.card.IsEmpty())
                {//循环直到牌被摸完
                    if (game.card.IsEmpty())
                        break;//判断是否还有牌，没牌则退出
                    int ans = 0;//当前玩家出牌，ans为打出的牌


                    //await Task.Run(() =>
                    //{
                    //    direction[cur].Dispatcher.Invoke(() =>
                    //    {
                    //(direction[cur].Content as Image).Source = directionPic[cur];
                    //cur = game.cur;
                    //(direction[cur].Content as Image).Source = directionPic[cur+4];
                    direction[cur].Content = new Image
                    {
                        Source = directionPic[cur]
                    };
                    cur = game.cur;
                    direction[cur].Content = new Image
                    {
                        Source = directionPic[cur + 4]
                    };
                    //    });
                    //});



                    selfCardTransition();
                    for (int i = 0; i < handCard.Count; ++i)
                        selfCardButton[i].Content = new Image
                        {
                            Source = totalCardSelf[handCard[i].Key]
                        };

                    if (game.cur == 0)
                    {
                        waitUserChoice = true;
                        int getCard = 0;

                        if (!isChiPeng)
                        {
                            if (isGang)
                            {
                                getCard = game.player[game.cur].GetBackCard();//摸牌
                                isGang = false;
                            }
                            else
                                getCard = game.player[game.cur].GetCard();//摸牌

                            remained.Content = remainedText + game.card.dq.Count;

                            handCard.Add(new KeyValuePair<int, int>(Transition(getCard), getCard));
                            //BitmapSource bitmapSource2 = AdaptImageSize(totalCardSelf[Transition(getCard)], new Size(selfCardButton[selfCardButton.Count - 1].ActualWidth, selfCardButton[selfCardButton.Count - 1].ActualHeight), 0);
                            BitmapSource bitmapSource2 = ImageMethod.RotateBitmapImage(totalCardSelf[Transition(getCard)], 0);
                            selfCardButton[selfCardButton.Count - 1].Content = new Image
                            {
                                Source = totalCardSelf[Transition(getCard)]
                                //Source = bitmapSource2
                            };
                            selfCardButton[selfCardButton.Count - 1].IsEnabled = true;

                            guoChiPengGangHu.Clear();
                            guoChiPengGangHu.Add(0);
                            bool gangType = true;
                            if (game.player[game.cur].CheckGang() != -1)
                            {
                                guoChiPengGangHu.Add(3);
                                gangType = true;
                                waitUserOtherChoice = true;
                            }
                            if (game.player[game.cur].CheckAddGang(getCard))
                            {
                                guoChiPengGangHu.Add(3);
                                gangType = false;
                                waitUserOtherChoice = true;
                            }
                            if (game.player[game.cur].CheckWin())
                            {
                                guoChiPengGangHu.Add(4);
                                waitUserOtherChoice = true;
                            }

                            if (waitUserOtherChoice)
                            {
                                for (int i = 0; i < guoChiPengGangHu.Count; i++)
                                {
                                    guoChiPengGangHuBtn[i].Content = new Image
                                    {
                                        Source = guoChiPengGangHuPic[guoChiPengGangHu[i]]
                                    };
                                    guoChiPengGangHuBtn[i].Visibility = Visibility.Visible;
                                }
                            }

                            //todo
                            // 暂停程序
                            if (waitUserOtherChoice)
                            {
                                await WaitForPlayer();
                                //    connect(this, &GameScene::resumeProgram, &loop, &QEventLoop::quit);
                                //    loop.exec();
                            }



                            if (guoChiPengGangHu.Count > 1)
                            {
                                if (guoChiPengGangHuChoice == 4)
                                {
                                    isFinish = true;
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    typeHu = 1;
                                    statement = statement + game.player[game.cur].GetName() + "自摸";
                                    break;
                                }
                                else if (guoChiPengGangHuChoice == 3)
                                {
                                    if (gangType)
                                    {
                                        game.player[game.cur].Gang(getCard, 0);
                                        //todo
                                        //emit pengGangSelfSignals(false,Transition(getCard));
                                        //QCoreApplication::processEvents();
                                        PengGangSelf(false, Transition(getCard));
                                    }
                                    else
                                    {
                                        game.player[game.cur].AddGang(getCard);
                                        AddGangSelf(Transition(getCard));
                                    }
                                    isGang = true;
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    continue;
                                }
                                foreach (var btn in guoChiPengGangHuBtn)
                                    btn.Visibility = Visibility.Hidden;
                            }

                        }
                        else
                        {
                            selfCardButton[selfCardButton.Count - 1].IsEnabled = true;
                            isChiPeng = false;
                        }

                        if (waitUserChoice)
                        {
                            await WaitForPlayer();
                            //    connect(this, &GameScene::resumeProgram, &loop, &QEventLoop::quit);
                            //    loop.exec();
                        }

                        KeyValuePair<int, int> discardThisRound = new KeyValuePair<int, int>();
                        if (discardIndex >= 0 && discardIndex < game.player[0].OwnCard.Count)
                            discardThisRound = new KeyValuePair<int, int>(handCard[discardIndex].Key, handCard[discardIndex].Value);
                        game.player[game.cur].Discard(discardThisRound.Value);//出牌

                        //double width = discarded[game.cur][discardedNowIndex[game.cur]].ActualWidth;
                        //double height = discarded[game.cur][discardedNowIndex[game.cur]].ActualHeight;
                        //BitmapSource bitmapSource = AdaptImageSize(totalDiscardedCard[discardThisRound.Key], new Size(width, height) , 0);
                        //BitmapSource bitmapSource = ImageMethod.RotateBitmapImage(totalDiscardedCard[discardThisRound.Key], 0);
                        //discarded[game.cur][discardedNowIndex[game.cur]].Content = new Image
                        //{
                        //    //Source = bitmapSource
                        //    //Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/settlement/Checkout.png"))
                        //    Source = totalDiscardedCard[discardThisRound.Key],
                        //    Stretch = Stretch.Uniform
                        //};
                        discarded[game.cur][discardedNowIndex[game.cur]].Source = totalDiscardedCard[1][discardThisRound.Key];

                        //Task.Run(() =>
                        //{
                        //    Dispatcher.Invoke(() =>
                        //    {
                        //        //discarded[game.cur][discardedNowIndex[game.cur]].Content = "123";
                        //    });
                        //});



                        selfCardButton[selfCardButton.Count - 1].Content = null;//更新按钮
                        selfCardButton[selfCardButton.Count - 1].IsEnabled = false;

                        //todo
                        //QCoreApplication::processEvents();

                        discardedNowIndex[game.cur]++;

                        selfCardTransition();
                        for (int i = 0; i < handCard.Count; ++i)
                            selfCardButton[i].Content = new Image
                            {
                                Source = totalCardSelf[handCard[i].Key]
                            };

                        deal = true;
                        if (deal)
                        {
                            //检查胡
                            for (int i = (game.cur + 1) % 4; i != 0; i = (i + 1) % 4)
                            {
                                if (game.player[i].CheckWin(discardThisRound.Value))
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    isFinish = true;
                                    deal = false;//结束游戏
                                    typeHu = -1;
                                    statement = statement + game.player[game.cur].GetName() + "点炮" + game.player[i].GetName();
                                    break;
                                }
                            }
                            if (isFinish)
                                break;
                        }
                        if (deal)
                        {
                            //检查杠
                            for (int i = (game.cur + 1) % 4; i != 0; i = (i + 1) % 4)
                            {
                                if (game.player[i].CheckGang(discardThisRound.Value))
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    game.player[i].Gang(discardThisRound.Value, 1);
                                    //discarded[i][--discardedNowIndex[i]]->setPixmap(QPixmap());
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    //todo
                                    //emit pengGangOthersSignals(false,i - 1,discardThisRound.first);
                                    //QCoreApplication::processEvents();
                                    PengGangOthers(false, i - 1, discardThisRound.Key);
                                    isGang = true;
                                    game.cur = i;
                                    deal = false;
                                    break;
                                }
                            }
                        }
                        if (deal)
                        {
                            //检查碰
                            for (int i = (game.cur + 1) % 4; i != 0; i = (i + 1) % 4)
                            {
                                if (game.player[i].CheckPeng(discardThisRound.Value))
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    game.player[i].Peng(discardThisRound.Value);
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    //todo
                                    //emit pengGangOthersSignals(true,i - 1,discardThisRound.first);
                                    //QCoreApplication::processEvents();
                                    PengGangOthers(true, i - 1, discardThisRound.Key);
                                    isChiPeng = true;
                                    game.cur = i;
                                    deal = false;
                                    break;
                                }
                            }
                        }
                        if (deal)
                        {
                            //检查吃
                            List<int> temp = game.player[game.cur + 1].CheckChiRot(discardThisRound.Value);
                            if (temp.Count > 0)
                            {
                                //todo
                                //timer.setSingleShot(true);
                                //timer.start(1000);
                                //loop.exec();
                                await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                game.player[game.cur + 1].Chi(temp);
                                temp.Add(discardThisRound.Value);
                                for (int i = 0; i < 3; i++)
                                    temp[i] = Transition(temp[i]);
                                temp.Sort();
                                //Task.Run(() =>
                                //{
                                //    Dispatcher.Invoke(() =>
                                //    {
                                //        //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                //    });
                                //});
                                //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;

                                ChiOthers(0, temp);
                                //todo
                                //QCoreApplication::processEvents();
                                isChiPeng = true;
                                game.cur = game.cur + 1;
                                deal = false;
                            }
                        }
                        if (deal)
                            game.cur = (game.cur + 1) % 4;
                    }
                    else
                    {
                        int getCard = 0;
                        if (!isChiPeng)
                        {
                            if (isGang)
                            {
                                getCard = game.player[game.cur].GetBackCard();//摸牌
                                isGang = false;
                            }
                            else
                                getCard = game.player[game.cur].GetCard();//摸牌
                                                                          //Task.Run(() =>
                                                                          //{
                                                                          //    Dispatcher.Invoke(() =>
                                                                          //    {
                            remained.Content = remainedText + game.card.dq.Count;
                            //othersCard[game.cur - 1][othersCard[game.cur - 1].Count - 1].Content = new Image
                            //{
                            //    Source = othersCardImages[game.cur - 1]
                            //};
                            othersCard[game.cur - 1][othersCard[game.cur - 1].Count - 1].Source = othersCardImages[game.cur - 1];
                            //});

                            //});



                            if (game.player[game.cur].CheckWin())
                            {
                                //todo
                                //timer.setSingleShot(true);
                                //timer.start(1000);
                                //loop.exec();
                                await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                isFinish = true;
                                typeHu = -1;
                                statement = statement + game.player[game.cur].GetName() + "自摸";
                                break;//结束游戏
                            }
                            if (game.player[game.cur].CheckGang() != -1)
                            {
                                //todo
                                //timer.setSingleShot(true);
                                //timer.start(1000);
                                //loop.exec();
                                await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                game.player[game.cur].Gang(getCard, 0);
                                //todo
                                //emit pengGangOthersSignals(false,game->cur - 1,Transition(getCard));
                                //QCoreApplication::processEvents();
                                PengGangOthers(false, game.cur - 1, Transition(getCard));
                                isGang = true;
                                continue;
                            }
                            if (game.player[game.cur].CheckAddGang(getCard))
                            {
                                //todo
                                //timer.setSingleShot(true);
                                //timer.start(1000);
                                //loop.exec();
                                await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                game.player[game.cur].AddGang(getCard);
                                AddGangOthers(game.cur - 1, Transition(getCard));
                                isGang = true;
                                continue;
                            }

                        }
                        else
                        {
                            isChiPeng = false;
                        }
                        // 启动定时器，设置1秒后触发一次定时器超时事件
                        //todo
                        //timer.setSingleShot(true);
                        //timer.start(1000); // 1000 毫秒 = 1 秒
                        //loop.exec();
                        await Task.Delay(1000);  // 延迟1秒（1000毫秒）

                        int discardThisRound = game.player[game.cur].DiscardRobot();
                        game.player[game.cur].Discard(discardThisRound);//出牌,默认出第一张牌

                        //BitmapSource bitmapSource = AdaptImageSize(totalDiscardedCard[Transition(discardThisRound)], new Size(discarded[game.cur][discardedNowIndex[game.cur]].ActualWidth, discarded[game.cur][discardedNowIndex[game.cur]].ActualHeight), game.cur * (-90));
                        //BitmapSource bitmapSource = ImageMethod.RotateBitmapImage(totalDiscardedCard[Transition(discardThisRound)], game.cur * (-90));
                        //discarded[game.cur][discardedNowIndex[game.cur]].Content = new Image
                        //{
                        //    //Source = bitmapSource
                        //    Source = totalDiscardedCard[Transition(discardThisRound)],
                        //    RenderTransform = new RotateTransform(game.cur * (-90)),
                        //    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                        //    Stretch = Stretch.Uniform
                        //};//更新已出牌区
                        //discarded[game.cur][discardedNowIndex[game.cur]].Source = totalDiscardedCard[Transition(discardThisRound)];
                        //discarded[game.cur][discardedNowIndex[game.cur]].RenderTransform = new RotateTransform(game.cur * (-90));
                        discarded[game.cur][discardedNowIndex[game.cur]].Source = totalDiscardedCard[(game.cur - 1) % 2][Transition(discardThisRound)];
                        discarded[game.cur][discardedNowIndex[game.cur]].RenderTransform = new RotateTransform(game.cur / 2 * (-180));
                        discarded[game.cur][discardedNowIndex[game.cur]].RenderTransformOrigin = new Point(0.5, 0.5);


                        //othersCard[game.cur - 1][othersCard[game.cur - 1].Count - 1].Content = null;//更新按钮
                        othersCard[game.cur - 1][othersCard[game.cur - 1].Count - 1].Source = null;
                        discardedNowIndex[game.cur]++;

                        guoChiPengGangHu.Clear();
                        guoChiPengGangHu.Add(0);
                        deal = true;
                        if (deal)
                        {
                            multiChi = false;//当前是否有多种吃法
                            List<List<int>> chiTemp = new List<List<int>>();//将多种吃法存入该数组中
                            List<List<int>> transitionChi = new List<List<int>>();//将多种吃法的牌所对应的图片的下标存入该数组中
                            if (game.cur == 3)//轮至自己
                            {
                                chiTemp = game.player[0].CheckChi(discardThisRound);//检查当前牌是否能吃
                                chiTemp.Sort((a, b) => Transition(a[0]).CompareTo(Transition(b[0])));
                                if (chiTemp.Count > 0)//能吃牌
                                {
                                    guoChiPengGangHu.Add(1);
                                    waitUserOtherChoice = true;
                                }
                                if (chiTemp.Count > 1)//能吃牌且有多种吃法
                                {
                                    multiChi = true;
                                    for (int i = 0; i < chiTemp.Count; i++)
                                    {
                                        List<int> t = new List<int>();
                                        transitionChi.Add(t);
                                        transitionChi[i].AddRange(chiTemp[i]);
                                        transitionChi[i].Add(discardThisRound);
                                        for (int j = 0; j < 3; j++)
                                        {
                                            chiChoice[i][j].IsEnabled = true;//设置按钮可用
                                            transitionChi[i][j] = Transition(transitionChi[i][j]);
                                        }
                                        transitionChi[i].Sort();
                                        for (int j = 0; j < 3; j++)
                                        {
                                            //设置按钮牌图片
                                            chiChoice[i][j].Content = new Image
                                            {
                                                //Source = AdaptImageSize(totalDiscardedCard[transitionChi[i][j]], new Size(chiChoice[i][j].ActualWidth, chiChoice[i][j].ActualHeight), 0)
                                                //Source = RotateBitmapImage(totalDiscardedCard[transitionChi[i][j]], 0)
                                                Source = totalDiscardedCard[1][transitionChi[i][j]]
                                            };
                                            chiChoice[i][j].Visibility = Visibility.Visible;
                                        }
                                    }
                                }
                            }
                            if (game.player[0].CheckPeng(discardThisRound))//检查是否能碰
                            {
                                guoChiPengGangHu.Add(2);
                                waitUserOtherChoice = true;
                            }
                            if (game.player[0].CheckGang(discardThisRound))//检查是否能杠
                            {
                                guoChiPengGangHu.Add(3);
                                waitUserOtherChoice = true;
                            }
                            if (game.player[0].CheckWin(discardThisRound))//检查是否能胡
                            {
                                guoChiPengGangHu.Add(4);
                                waitUserOtherChoice = true;
                            }

                            if (guoChiPengGangHu.Count > 1)//大于1说明能吃碰杠胡中的一种或多种
                            {
                                guoChiPengGangHu.Sort();
                                waitUserOtherChoice = true;

                                for (int i = 0; i < guoChiPengGangHu.Count; i++)
                                {
                                    //设置吃碰杠胡区按钮图片
                                    guoChiPengGangHuBtn[i].Content = new Image
                                    {
                                        Source = guoChiPengGangHuPic[guoChiPengGangHu[i]]
                                    };
                                    guoChiPengGangHuBtn[i].Visibility = Visibility.Visible;
                                }
                                //等待玩家选择
                                //todo
                                if (waitUserOtherChoice)
                                {
                                    await WaitForPlayer();
                                    //    connect(this, &GameScene::resumeProgram, &loop, &QEventLoop::quit);
                                    //    loop.exec();
                                }

                                if (guoChiPengGangHuChoice == 4)
                                {
                                    isFinish = true;
                                    deal = false;
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    typeHu = 1;
                                    statement = statement + game.player[game.cur].GetName() + "点炮" + game.player[0].GetName();
                                    break;
                                }
                                else if (guoChiPengGangHuChoice == 3)
                                {
                                    game.player[0].Gang(discardThisRound, 1);
                                    //todo
                                    //emit pengGangSelfSignals(false,Transition(discardThisRound));
                                    //QCoreApplication::processEvents();
                                    PengGangSelf(false, Transition(discardThisRound));
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    deal = false;
                                    isGang = true;
                                    game.cur = 0;
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    continue;
                                }
                                else if (guoChiPengGangHuChoice == 2)
                                {
                                    game.player[0].Peng(discardThisRound);
                                    //todo
                                    //emit pengGangSelfSignals(true,Transition(discardThisRound));
                                    //QCoreApplication::processEvents();
                                    PengGangSelf(true, Transition(discardThisRound));
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    deal = false;
                                    isChiPeng = true;
                                    game.cur = 0;
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    continue;
                                }
                                else if (guoChiPengGangHuChoice == 1)//吃
                                {
                                    if (multiChi)//有多种情况
                                    {
                                        game.player[0].Chi(chiTemp[multiChiChoice]);//更新手牌
                                        ChiSelf(transitionChi[multiChiChoice]);//更新UI
                                        for (int i = 0; i < chiTemp.Count; i++)
                                        {
                                            for (int j = 0; j < 3; j++)
                                            {
                                                chiChoice[i][j].Content = null;//设置图片为空
                                                chiChoice[i][j].IsEnabled = false;//设置按钮不可用
                                                chiChoice[i][j].Visibility = Visibility.Hidden;
                                            }
                                        }
                                        //todo
                                        //QCoreApplication::processEvents();
                                    }
                                    else//无
                                    {
                                        game.player[0].Chi(chiTemp[0]);//更新手牌
                                        chiTemp[0].Add(discardThisRound);
                                        for (int i = 0; i < 3; i++)
                                            chiTemp[0][i] = Transition(chiTemp[0][i]);
                                        chiTemp[0].Sort();
                                        ChiSelf(chiTemp[0]);//更新UI
                                                            //todo
                                                            //QCoreApplication::processEvents();
                                    }
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    deal = false;
                                    isChiPeng = true;
                                    game.cur = 0;
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                    continue;
                                }
                                else if (guoChiPengGangHuChoice == 0)
                                {
                                    //当用户选择过时，也需要将多吃部分进行修改
                                    for (int i = 0; i < chiTemp.Count; i++)
                                    {
                                        for (int j = 0; j < 3; j++)
                                        {
                                            chiChoice[i][j].Content = null;//设置图片为空
                                            chiChoice[i][j].IsEnabled = false;//设置按钮不可用
                                            chiChoice[i][j].Visibility = Visibility.Hidden;
                                        }
                                    }
                                    foreach (var btn in guoChiPengGangHuBtn)
                                        btn.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                        if (deal)
                        {
                            //检查胡
                            for (int i = (game.cur + 1) % 4; i != game.cur; i = (i + 1) % 4)
                            {
                                if (i != 0 && game.player[i].CheckWin(discardThisRound))
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    isFinish = true;//结束游戏
                                    deal = false;
                                    typeHu = -1;
                                    statement = statement + game.player[game.cur].GetName() + "点炮" + game.player[i].GetName();
                                    break;
                                }
                            }
                            if (isFinish)
                                break;
                        }
                        if (deal)
                        {
                            //检查杠
                            for (int i = (game.cur + 1) % 4; i != game.cur; i = (i + 1) % 4)
                            {
                                if (i != 0 && game.player[i].CheckGang(discardThisRound))
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    game.player[i].Gang(discardThisRound, 1);
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    //todo
                                    //emit pengGangOthersSignals(false,i - 1,Transition(discardThisRound));
                                    //QCoreApplication::processEvents();
                                    PengGangOthers(false, i - 1, Transition(discardThisRound));
                                    isGang = true;
                                    game.cur = i;
                                    deal = false;
                                    break;
                                }
                            }
                        }
                        if (deal)
                        {
                            //检查碰
                            for (int i = (game.cur + 1) % 4; i != game.cur; i = (i + 1) % 4)
                            {
                                if (i != 0 && game.player[i].CheckPeng(discardThisRound))
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    game.player[i].Peng(discardThisRound);
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    //todo
                                    //emit pengGangOthersSignals(true,i - 1,Transition(discardThisRound));
                                    //QCoreApplication::processEvents();
                                    PengGangOthers(true, i - 1, Transition(discardThisRound));
                                    isChiPeng = true;
                                    game.cur = i;
                                    deal = false;
                                    break;
                                }
                            }
                        }
                        if (deal)
                        {
                            //检查吃
                            if (game.cur != 3)
                            {
                                List<int> temp = game.player[game.cur + 1].CheckChiRot(discardThisRound);
                                if (temp.Count > 0)
                                {
                                    //todo
                                    //timer.setSingleShot(true);
                                    //timer.start(1000);
                                    //loop.exec();
                                    await Task.Delay(1000);  // 延迟1秒（1000毫秒）
                                    game.player[game.cur + 1].Chi(temp);
                                    temp.Add(discardThisRound);
                                    for (int i = 0; i < 3; i++)
                                        temp[i] = Transition(temp[i]);
                                    temp.Sort();
                                    //discarded[game.cur][--discardedNowIndex[game.cur]].Content = null;
                                    discarded[game.cur][--discardedNowIndex[game.cur]].Source = null;
                                    ChiOthers(game.cur, temp);
                                    //todo
                                    //QCoreApplication::processEvents();
                                    isChiPeng = true;
                                    game.cur = game.cur + 1;
                                    deal = false;
                                }
                            }
                        }
                        if (deal)
                            game.cur = (game.cur + 1) % 4;
                    }
                    //todo
                    //QCoreApplication::processEvents();
                    //loop.processEvents(QEventLoop::AllEvents);
                }
                if (isFinish)
                {
                    if (typeHu == 1)
                        settlementPic.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/win.png")),
                            Stretch = Stretch.Uniform
                        };
                    else if (typeHu == -1)
                        settlementPic.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/lose.png")),
                            Stretch = Stretch.Uniform
                        };
                    ResetCheckoutUI();
                    //todo
                    //checkout->show();
                }
                else
                {
                    settlementPic.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/others/dogfall.png")),
                        Stretch = Stretch.Uniform
                    };
                    statement += "牌堆已空";
                    ResetCheckoutUI();
                    //todo
                    //checkout->show();
                }
                //    });
                //});
            }
            catch (Exception e)
            {

            }
        }

        //异步等待玩家点击出牌按钮
        private async Task WaitForPlayer()
        {
            // 使用 TaskCompletionSource 来暂停异步操作，直到玩家点击按钮
            tcs = new TaskCompletionSource<int>();

            // 等待玩家点击按钮
            await tcs.Task;
        }

        private void BackToMainPageButtonClick(object sender, RoutedEventArgs e)
        {
            DestroyPageResources();
            GC.Collect();
            // 获取父窗口中的 Frame 控件
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(mainWindow.mainPage);
        }
        private void DestroyPageResources()
        {
            // 1. 清理绑定
            BindingOperations.ClearAllBindings(this);

            // 2. 解除事件绑定
            // 如果控件绑定了事件处理程序，应该在销毁时解除绑定。
            foreach (var button in selfCardButton)
            {
                button.Click -= SelfCardButtonClick; // 示例: 解除按钮的点击事件
            }

            foreach (var button in guoChiPengGangHuBtn)
            {
                button.Click -= ChiPengGangButtonClick; // 示例: 解除按钮的点击事件
            }

            foreach (var chiButtons in chiChoice)
            {
                foreach (var button in chiButtons)
                    button.Click -= MultiChiButtonClick; // 示例: 解除按钮的点击事件
            }

            continueGame.Click -= ContinueGame_Click;
            backToMainStage.Click -= BackToMainStage_Click;
            hideCheckOut.Click -= HideCheckOut_Click;
            backToMainPage.Click -= BackToMainPageButtonClick;

            // 3. 清空控件内容（并释放 UI 元素）
            if (selfCardBox != null)
            {
                selfCardBox.Children.Clear(); // 清空控件内容
            }

            if (totalCardBox != null)
            {
                totalCardBox.Children.Clear();
            }

            if (othersCardBox != null)
            {
                foreach (var box in othersCardBox)
                {
                    box.Children.Clear();
                }
            }

            if (chiPengGangBox != null)
            {
                chiPengGangBox.Children.Clear();
            }

            if (stackWidget != null)
            {
                stackWidget.Children.Clear();
            }

            if (checkout != null)
            {
                checkout.Children.Clear();
            }

            if (checkoutLayout != null)
            {
                checkoutLayout.Children.Clear();
            }

            if (checkoutBtn != null)
            {
                checkoutBtn.Children.Clear();
            }

            // 4. 清理方向牌、牌的显示等
            if (direction != null)
            {
                foreach (var label in direction)
                {
                    label.Content = null; // 清空方向标签
                }
            }

            if (discarded != null)
            {
                foreach (var discardedList in discarded)
                {
                    foreach (var label in discardedList)
                    {
                        //label.Content = null; // 清空弃牌标签
                        label.Source = null;
                    }
                }
            }

            if (guoChiPengGangHuPic != null)
            {
                guoChiPengGangHuPic.Clear();
            }

            if (continueGame != null)
            {
                continueGame.Content = null;
            }

            if (backToMainStage != null)
            {
                backToMainStage.Content = null;
            }

            if (hideCheckOut != null)
            {
                hideCheckOut.Content = null;
            }

            if (backToMainPage != null)
            {
                backToMainPage.Content = null;
            }

            // 5. 清理图像资源
            //totalCardSelf?.Clear();
            //totalDiscardedCard?.Clear();
            othersCardImages?.Clear();
            biaoZhi = null; // 清除图像资源
            directionPic?.Clear();

            // 6. 清理任务相关数据
            tcs?.TrySetResult(-1); // 取消任务等待

            // 7. 清理控件和游戏相关的成员变量
            selfCard.Clear();
            selfCardButton.Clear();
            selfCardLabel.Clear();
            othersCard.Clear();
            chiChoice.Clear();
            pengAlready.Clear();
            discardedNowIndex.Clear();

            // 8. 清理游戏状态
            game = null;
            gameName = null;

            // 9. 停止定时器（如果有定时器）
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            // 10. 清空其他状态变量
            choiceBtn = -1;
            guoChiPengGangHuChoice = 0;
            multiChi = false;
            multiChiChoice = -1;
            statu = true;
            isFinish = false;
            typeHu = 0;

            // 11. 清除控件的任何其他附加状态
            remained.Content = null; // 清空剩余牌数标签
            settlementPic.Content = null; // 清空结算图片
            settlementState.Content = null; // 清空结算状态标签

            // 12. 清理字符串和其他成员变量
            statement = null;
            remainedText = null;

            if (!type)
            {
                Connect.ws.MessageReceived += OnMessageReceived;
            }
        }

    }
}
