using Majiang;
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

public static class GameUI
{
    //public static void ChiSelf(List<int> chiCard, List<Label> selfCardLabel,ref int choiceBtn,List<StackPanel> selfCard,List<Button> selfCardButton, StackPanel selfCardBox, List<BitmapImage> totalDiscardedCard,Action SelfCardButtonClick)
    //{
    //    if (choiceBtn != -1)
    //    {
    //        selfCardLabel[choiceBtn].Content = null; // 清空选中的Label
    //        choiceBtn = -1;
    //    }

    //    // 移除按钮和控件
    //    for (int i = 0; i < 3; ++i)
    //    {
    //        var lastIndex = selfCardButton.Count - 2;

    //        // 移除自定义的控件和按钮
    //        selfCardBox.Children.Remove(selfCard[selfCard.Count - 2]);
    //        selfCardButton[selfCardButton.Count - 2].Content = null;
    //        selfCardButton[selfCardButton.Count - 2].Click -= SelfCardButtonClick; // 如果有事件绑定需要解除
    //        selfCardButton.RemoveAt(selfCardButton.Count - 2);

    //        selfCardLabel[selfCardLabel.Count - 2].Content = null;
    //        selfCardLabel.RemoveAt(selfCardLabel.Count - 2);

    //        selfCard.RemoveAt(selfCard.Count - 2);
    //    }

    //    // 插入间距
    //    selfCardBox.Children.Add(new Label { Margin = new Thickness(5) });

    //    // 插入新的图片标签
    //    for (int i = 0; i < 3; ++i)
    //    {
    //        var label = new Label
    //        {
    //            Width = 27,
    //            Height = 39,
    //            HorizontalContentAlignment = HorizontalAlignment.Center,
    //            VerticalContentAlignment = VerticalAlignment.Center,
    //            Margin = new Thickness(0),
    //            Padding = new Thickness(0), // 去除 Padding
    //            BorderThickness = new Thickness(0), // 去除边框厚度
    //            Content = new Image
    //            {
    //                Source = totalDiscardedCard[chiCard[i]],
    //                Stretch = Stretch.Uniform,
    //            }
    //        };
    //        selfCardBox.Children.Add(label);
    //    }
    //    selfCardButton[selfCardButton.Count - 1].Tag = selfCardButton.Count - 1;

    //    // 更新 UI
    //    //Dispatcher.Invoke(() =>
    //    //{
    //    //    //强制刷新 UI 界面
    //    //    selfCardBox.UpdateLayout();
    //    //});
    //}

    /*
     *     public static void ChiOthers(int type, List<int> chiCard)
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
            var label = new Label
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
            };
            if (type == 1)
            {
                label.Width = 27;
                label.Height = 39;
                label.Content = new Image
                {
                    Source = totalDiscardedCard[chiCard[i]],
                    Stretch = Stretch.Uniform,
                    RenderTransform = new RotateTransform((type + 1) * (-90)),
                    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                };
            }
            else
            {
                label.Width = 30;
                label.Height = 22;
                label.Content = new Image
                {
                    Source = totalDiscardedCard[chiCard[i]],
                    Stretch = Stretch.Uniform,
                    RenderTransform = new RotateTransform((type + 1) * (-90)),
                    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                };
            }
            othersCardBox[type].Children.Add(label);
        }

        // 强制刷新 UI
        Dispatcher.Invoke(() =>
        {
            othersCardBox[type].UpdateLayout();
        });
    }

    public static void PengGangSelf(bool choice, int pengGangCard)
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
                    Source = totalDiscardedCard[pengGangCard],
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

    public static void PengGangOthers(bool choice, int type, int pengGangCard)
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
            var label = new Label
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0),
                Padding = new Thickness(0), // 去除 Padding
                BorderThickness = new Thickness(0), // 去除边框厚度
            };
            if (type == 1)
            {
                label.Width = 27;
                label.Height = 39;
                label.Content = new Image
                {
                    Source = totalDiscardedCard[pengGangCard],
                    Stretch = Stretch.Uniform,
                    RenderTransform = new RotateTransform((type + 1) * (-90)),
                    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                };
            }
            else
            {
                label.Width = 30;
                label.Height = 22;
                label.Content = new Image
                {
                    Source = totalDiscardedCard[pengGangCard],
                    Stretch = Stretch.Uniform,
                    RenderTransform = new RotateTransform((type + 1) * (-90)),
                    RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
                };
            }
            othersCardBox[type].Children.Add(label);
            if (choice && i == 1)
            {
                pengAlready[type][pengGangCard] = othersCardBox[type].Children.IndexOf(label) - othersCard[type].Count;
            }
        }

        // 强制刷新界面更新
        Dispatcher.Invoke(() =>
        {
            selfCardBox.UpdateLayout();
        });
    }

    public static void AddGangSelf(int pengGangCard)
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
                Source = totalDiscardedCard[pengGangCard],
                Stretch = Stretch.Uniform,
            }
        };

        selfCardBox.Children.Insert(index, label);

        Dispatcher.Invoke(() =>
        {
            selfCardBox.UpdateLayout();
        });
    }

    public static void AddGangOthers(int type, int pengGangCard)
    {
        if (!pengAlready[type].ContainsKey(pengGangCard))
        {
            pengAlready[type][pengGangCard] = 0;
        }
        // 计算要插入的位置
        int index = pengAlready[type][pengGangCard] + othersCard[type].Count;

        // 创建 Image 控件，并设置其大小
        Label label = new Label
        {
            Width = (type == 1) ? 27 : 30,
            Height = (type == 1) ? 39 : 22,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0),
            Padding = new Thickness(0), // 去除 Padding
            BorderThickness = new Thickness(0), // 去除边框厚度
            Content = new Image
            {
                Source = totalDiscardedCard[pengGangCard],
                Stretch = Stretch.Uniform,
                RenderTransform = new RotateTransform((type + 1) * (-90)),
                RenderTransformOrigin = new Point(0.5, 0.5),  // 设置旋转的中心点
            }
        };
        othersCardBox[type].Children.Insert(index, label);
        Dispatcher.Invoke(() =>
        {
            selfCardBox.UpdateLayout();
        });
    }

     */
}
