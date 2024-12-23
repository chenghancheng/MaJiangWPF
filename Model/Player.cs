using Majiang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows.Resources;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

public class Player
{
    public HashSet<int> OwnCard { get; set; }
    public string? Name { get; set; }
    public int CurNum { get; set; }
    public Card? Card { get; set; }
    public HashSet<string> MyPeng { get; set; }

    //private SoundPlayer soundPlayer;

    public Player()
    {
        OwnCard = new HashSet<int>();
        MyPeng = new HashSet<string>();
    }

    public Player(string name, Card card) : this()
    {
        Name = name;
        CurNum = 0;
        this.Card = card;
        OwnCard = new HashSet<int>();
        MyPeng = new HashSet<string>();
        //soundPlayer = new SoundPlayer();
    }

    public int GetCard()
    {
        CurNum++;
        int p = Card.GetCard();
        OwnCard.Add(p);
        return p;
    }

    public int GetBackCard()
    {
        CurNum++;
        int p = Card.GetBackCard();
        OwnCard.Add(p);
        return p;
    }
   

    //查找1的个数
    public int CountOne(int n)
    {
        int count = 0;
        while (n != 0)
        {
            n &= (n - 1);
            count++;
        }
        return count;
    }

    public int Discard(int n)
    {
        //// 获取嵌入资源的 URI
        //Uri soundUri = new Uri("pack://application:,,,/Resources/Music/click.wav");
        //// 获取资源流
        //StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);
        //// 使用流创建 SoundPlayer
        //SoundPlayer soundPlayer = new SoundPlayer(soundStreamInfo.Stream);
        //soundPlayer.Play();

        Task.Run(() =>
        {
            // 获取嵌入资源的 URI
            Uri soundUri = new Uri("pack://application:,,,/Resources/Music/click.wav");

            // 获取资源流
            StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);

            // 使用 MemoryStream 复制资源流
            using (MemoryStream memoryStream = new MemoryStream())
            {
                soundStreamInfo.Stream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);  // 将流位置重置到开始

                // 使用独立的 MemoryStream 创建 SoundPlayer
                SoundPlayer soundPlayer = new SoundPlayer(memoryStream);
                soundPlayer.Play();
            }
        });
        

        CurNum--;
        int p = -1;
        foreach (var i in OwnCard)
        {
            if (Card.GetName(i) == Card.GetName(n))
            {
                p = i;
                break;
            }
        }
        OwnCard.Remove(p);
        return n;
    }

    public int DiscardChi(int n)
    {
        
        CurNum--;
        int p = -1;
        foreach (var i in OwnCard)
        {
            if (Card.GetName(i) == Card.GetName(n))
            {
                p = i;
                break;
            }
        }
        OwnCard.Remove(p);
        return n;
    }

    public int DiscardRobot()
    {
        //CurNum = OwnCard.Count;
        var curCard2 = new List<int>(CurNum);
        for (int i = 0; i < CurNum; i++)
        {
            curCard2.Add(0); // 你可以用需要的初始值来填充 List
        }
        int pos = 0;
        foreach (var it in OwnCard)
            curCard2[pos++] = it;

        int dis = 0;
        var mp = new Dictionary<string, int>();
        foreach (var i in curCard2)
        {
            string cardCurName = Card.GetName(i);
            if (!mp.ContainsKey(cardCurName))
                mp[cardCurName] = 0;
            mp[cardCurName]++;
        }

        foreach (var i in curCard2)
        {
            if (i > 108 && mp[Card.GetName(i)] == 1)
            {
                dis = i;
                return dis;
            }
        }

        int ans = 0;
        var curCard = new List<int>(CurNum);
        for (int i = 0; i < CurNum; i++)
        {
            curCard.Add(0); // 你可以用需要的初始值来填充 List
        }
        int idx = 0;
        foreach (var it in OwnCard)
            curCard[idx++] = it;

        var dp = new List<int>(1 << CurNum);
        for (int i = 0; i < (1 << CurNum); i++) dp.Add(-1);
        Check((1 << CurNum) - 1, curCard, dp);
        int pp = 0;
        for (int j = 0; j < (1 << CurNum); j++)
        {
            if (dp[j] == 1 && CountOne(j) > CountOne(pp))
                pp = j;
        }

        var st = new HashSet<int>();
        for (int j = 0; j < CurNum; j++)
        {
            if ((1 << j & pp) == 0)
            {
                ans = j;
                st.Add(j);
            }
        }

        int xx = st.Count;
        int r = GenerateRandomNumber(xx);
        foreach (var it in st)
        {
            if (r-- == 0)
            {
                ans = it;
                break;
            }
        }

        return curCard[ans];
    }

    private int GenerateRandomNumber(int n)
    {
        var random = new Random();
        return random.Next(1, n + 1);
    }

    public bool CheckWin()
    {
        CurNum = OwnCard.Count;
        var curCard = new List<int>(CurNum);
        for (int i = 0; i < CurNum; i++)
        {
            curCard.Add(0); // 你可以用需要的初始值来填充 List
        }
        int ii = 0;
        foreach (var it in OwnCard)
            curCard[ii++] = it;

        var dp = new List<int>(1 << CurNum);
        for (int i = 0; i < (1 << CurNum); i++) dp.Add(-1);
        return Check((1 << CurNum) - 1, curCard, dp);
    }

    public bool CheckWin(int n)
    {
        CurNum = OwnCard.Count;
        var curCard = new List<int>(CurNum + 1);
        for (int i = 0; i < CurNum; i++)
        {
            curCard.Add(0); // 你可以用需要的初始值来填充 List
        }
        int ii = 0;
        foreach (var it in OwnCard)
            curCard[ii++] = it;

        curCard.Add(n);
        var dp = new List<int>((1 << (CurNum + 1)));
        for (int i = 0; i < (1 << (CurNum + 1)); i++) dp.Add(-1);
        return Check((1 << (CurNum + 1)) - 1, curCard, dp);
    }

    private bool Check(int n, List<int> curCard, List<int> dp)
    {
        if (dp[n] != -1)
            return dp[n] == 1;

        int l = CountOne(n);
        if (l == 2)
        {
            int c1 = 0;
            for (int i = 0; i < 31; i++)
            {
                if ((n & (1 << i)) != 0)
                {
                    n ^= 1 << i;
                    break;
                }
                else c1++;
            }

            int c2 = 0;
            for (int i = 0; i < 31; i++)
            {
                if ((n & (1 << i)) != 0)
                {
                    n ^= 1 << i;
                    break;
                }
                else c2++;
            }

            dp[n] = Card.GetName(curCard[c1]) == Card.GetName(curCard[c2]) ? 1 : 0;
            return dp[n] == 1;
        }

        for (int j = n; j != 0; j = (j - 1) & n)
        {
            if (CountOne(j) != 3) continue;

            int c1 = 0, jj = j;
            for (int i = 0; i < 31; i++)
            {
                if ((jj & (1 << i)) != 0)
                {
                    jj ^= 1 << i;
                    break;
                }
                else c1++;
            }

            int c2 = 0;
            for (int i = 0; i < 31; i++)
            {
                if ((jj & (1 << i)) != 0)
                {
                    jj ^= 1 << i;
                    break;
                }
                else c2++;
            }

            int c3 = 0;
            for (int i = 0; i < 31; i++)
            {
                if ((jj & (1 << i)) != 0)
                {
                    jj ^= 1 << i;
                    break;
                }
                else c3++;
            }

            string s1 = Card.GetName(curCard[c1]);
            string s2 = Card.GetName(curCard[c2]);
            string s3 = Card.GetName(curCard[c3]);
            bool key = false;

            if (s1 == s2 && s2 == s3)
            {
                key = Check(n ^ j, curCard, dp);
                dp[j] = key ? 1 : 0;
                if (key) return true;
                continue;
            }

            int n1 = curCard[c1], n2 = curCard[c2], n3 = curCard[c3];
            if (n1 > 108 || n2 > 108 || n3 > 108) continue;

            int p1 = (n1 - 1) / 36, p2 = (n2 - 1) / 36, p3 = (n3 - 1) / 36;
            if (p1 != p2 || p1 != p3) continue;

            int num1 = (n1 - 1) % 9 + 1, num2 = (n2 - 1) % 9 + 1, num3 = (n3 - 1) % 9 + 1;
            if (num1 > num2) (num1, num2) = (num2, num1);
            if (num2 > num3) (num2, num3) = (num3, num2);
            if (num1 > num2) (num1, num2) = (num2, num1);
            if (num1 + 1 != num2 || num2 + 1 != num3) continue;

            key = Check(n ^ j, curCard, dp);
            dp[j] = key ? 1 : 0;
            if (key) return true;
        }

        dp[n] = 0;
        return false;
    }

    public List<int> CheckChiRot(int n)
    {
        List<int> curCard = new List<int>(CurNum);  // 初始化当前卡牌
        for (int i = 0; i < CurNum; i++)
        {
            curCard.Add(0);
        }
        int ii = 0;
        foreach (var card in OwnCard)  // 复制当前牌到 curCard
        {
            curCard[ii++] = card;
        }

        List<List<int>> ans = new List<List<int>>();  // 存储结果

        // 遍历所有可能的组合
        for (int j = (1 << CurNum) - 1; j > 0; j = (j - 1) & ((1 << CurNum) - 1))
        {
            int cnt = CountOne(j);  // 计算1的数量
            if (cnt != 2) continue;  // 只考虑有两个1的组合

            int jj = j;
            int c1 = 0;

            // 找到第一个位置
            for (int i = 0; i < 31; i++)
            {
                if ((jj & (1 << i)) != 0)
                {
                    jj ^= 1 << i;
                    break;
                }
                else
                {
                    c1++;
                }
            }

            int c2 = 0;

            // 找到第二个位置
            for (int i = 0; i < 31; i++)
            {
                if ((jj & (1 << i)) != 0)
                {
                    jj ^= 1 << i;
                    break;
                }
                else
                {
                    c2++;
                }
            }

            int n1 = curCard[c1], n2 = curCard[c2], n3 = n;

            if (n1 > 108 || n2 > 108 || n3 > 108) continue;

            int p1 = (n1 - 1) / 36, p2 = (n2 - 1) / 36, p3 = (n3 - 1) / 36;

            if (p1 != p2 || p1 != p3) continue;

            int num1 = (n1 - 1) % 9 + 1, num2 = (n2 - 1) % 9 + 1, num3 = (n3 - 1) % 9 + 1;

            // 排序并检查是否是连续的
            if (num1 > num2) Swap(ref num1, ref num2);
            if (num2 > num3) Swap(ref num2, ref num3);
            if (num1 > num2) Swap(ref num1, ref num2);

            if (num1 + 1 != num2 || num2 + 1 != num3) continue;

            // 如果满足条件，将该组合加入结果
            ans.Add(new List<int> { n1, n2 });
        }

        // 如果没有找到合适的组合，返回空列表
        if (ans.Count == 0)
        {
            return new List<int>();  // 返回空的列表
        }

        // 返回第一个符合条件的组合
        return ans[0];
    }


    public int Ctz(int x)
    {
        int cnt = 0;
        if (x == 0) return 0;
        while ((x & 1) == 0)
        {
            x >>= 1;
            cnt++;
        }
        return cnt;
    }

    public List<List<int>> CheckChi(int n)
    {
        List<int> curCard = new List<int>(OwnCard);
        for (int i = 0; i < OwnCard.Count + 1; i++)
        {
            curCard.Add(0);
        }
        List<List<int>> res = new List<List<int>>();
        int j = (1 << 2) - 1;
        List<List<int>> ans = new List<List<int>>();

        while (j < (1 << CurNum))
        {
            int jj = j;
            int lb = j & -j;
            int cur = lb + j;
            j = ((cur ^ j) >> (Ctz(j) + 2)) + cur;
            int cnt = CountOne(jj);
            if (cnt != 2) continue;

            int c1 = Ctz(jj);
            jj ^= jj & -jj;
            int c2 = Ctz(jj);

            int n1 = curCard[c1], n2 = curCard[c2], n3 = n;
            if (n1 > 108 || n2 > 108 || n3 > 108) continue;

            int p1 = (n1 - 1) / 36, p2 = (n2 - 1) / 36, p3 = (n3 - 1) / 36;
            if (p1 != p2 || p1 != p3) continue;

            int num1 = (n1 - 1) % 9 + 1, num2 = (n2 - 1) % 9 + 1, num3 = (n3 - 1) % 9 + 1;
            if (num1 > num2) Swap(ref num1, ref num2);
            if (num2 > num3) Swap(ref num2, ref num3);
            if (num1 > num2) Swap(ref num1, ref num2);

            if (num1 + 1 != num2 || num2 + 1 != num3) continue;

            res.Add(new List<int> { n1, n2 });
        }

        HashSet<Tuple<string, string>> st = new HashSet<Tuple<string, string>>();
        foreach (var p in res)
        {
            int x = p[0], y = p[1];
            string s1 = Card.GetName(x), s2 = Card.GetName(y);
            var pair1 = new Tuple<string, string>(s1, s2);
            var pair2 = new Tuple<string, string>(s2, s1);
            if (st.Contains(pair1) || st.Contains(pair2)) continue;
            ans.Add(p);
            st.Add(pair1);
        }

        return ans;
    }

    public void Chi(List<int> n)
    {
        //获取嵌入资源的 URI
        Uri soundUri = new Uri("pack://application:,,,/Resources/Music/chi.wav");
        // 获取资源流
        StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);
        //使用流创建 SoundPlayer
        SoundPlayer soundPlayer = new SoundPlayer(soundStreamInfo.Stream);
        soundPlayer.Play();
        //music.PlayChiSound();
        for (int i = 0; i < 2; ++i)
        {
            DiscardChi(n[i]);
        }
    }

    public bool CheckPeng(int n)
    {
        Dictionary<string, int> cnt = new Dictionary<string, int>();
        foreach (var card in OwnCard)
        {
            string cardName = this.Card.GetName(card);
            if (cnt.ContainsKey(cardName))
                cnt[cardName]++;
            else
                cnt.Add(cardName, 1);
        }
        return cnt.ContainsKey(this.Card.GetName(n)) && cnt[this.Card.GetName(n)] >= 2;
    }

    public void Peng(int n)
    {
        //获取嵌入资源的 URI
        Uri soundUri = new Uri("pack://application:,,,/Resources/Music/peng.wav");
        // 获取资源流
        StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);
        //使用流创建 SoundPlayer
        SoundPlayer soundPlayer = new SoundPlayer(soundStreamInfo.Stream);
        soundPlayer.Play();
        //music.PlayPengSound();
        int cnt = 2;
        CurNum -= 2;
        MyPeng.Add(Card.GetName(n));
        List<int> p = new List<int>();

        foreach (var card in OwnCard)
        {
            if (Card.GetName(card) == Card.GetName(n) && cnt > 0)
            {
                //OwnCard.Remove(card);
                p.Add(card);
                cnt--;
            }
        }

        foreach (var item in p)
        {
            OwnCard.Remove(item);
        }
    }

    public int CheckGang()
    {
        Dictionary<string, int> cnt = new Dictionary<string, int>();
        int p = -1;
        foreach (var card in OwnCard)
        {
            string cardName = this.Card.GetName(card);
            if (!cnt.ContainsKey(cardName))
            {
                cnt[cardName] = 0;
            }
            if (++cnt[cardName] >= 4)
                p = card;
        }
        return p;
    }

    public bool CheckGang(int n)
    {
        Dictionary<string, int> cnt = new Dictionary<string, int>();
        foreach (var card in OwnCard)
        {
            string cardName = this.Card.GetName(card);
            if (cnt.ContainsKey(cardName))
                cnt[cardName]++;
            else
                cnt.Add(cardName, 1);
        }
        return cnt.ContainsKey(this.Card.GetName(n)) && cnt[this.Card.GetName(n)] >= 3;
    }

    public bool CheckAddGang(int n)
    {
        return MyPeng.Contains(this.Card.GetName(n));
    }

    public int Gang(int n, int type)
    {
        //获取嵌入资源的 URI
        Uri soundUri = new Uri("pack://application:,,,/Resources/Music/gang.wav");
        // 获取资源流
        StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);
        //使用流创建 SoundPlayer
        SoundPlayer soundPlayer = new SoundPlayer(soundStreamInfo.Stream);
        soundPlayer.Play();
        //music.PlayGangSound();
        int cnt = type == 1 ? 3 : 4;
        CurNum -= cnt;
        List<int> p = new List<int>();

        foreach (var card in OwnCard)
        {
            if (Card.GetName(card) == Card.GetName(n) && cnt > 0)
            {
                p.Add(card);
                cnt--;
            }
        }

        foreach (var item in p)
        {
            OwnCard.Remove(item);
        }

        return 0;
    }

    // C++ 版的 add_gang 方法转为 C# 版本
    public void AddGang(int n)
    {
        //获取嵌入资源的 URI
        Uri soundUri = new Uri("pack://application:,,,/Resources/Music/gang.wav");
        // 获取资源流
        StreamResourceInfo soundStreamInfo = Application.GetResourceStream(soundUri);
        //使用流创建 SoundPlayer
        SoundPlayer soundPlayer = new SoundPlayer(soundStreamInfo.Stream);
        soundPlayer.Play();
        //music.PlayGangSound();
        DiscardChi(n);
    }

    // C++ 版的 get_name 方法转为 C# 版本
    public string GetName()
    {
        return this.Name;
    }


    // 辅助方法：交换两个值
    private void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
}
