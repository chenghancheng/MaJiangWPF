using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public HashSet<int> OwnCard { get; set; }
    public string Name { get; set; }
    public int CurNum { get; set; }
    public Card Card { get; set; }
    public HashSet<string> MyPeng { get; set; }

    public Player()
    {
        OwnCard = new HashSet<int>();
    }

    public Player(string name, Card card) : this()
    {
        Name = name;
        CurNum = 0;
        Card = card;
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

    public int Round()
    {
        int p = Card.GetCard();
        OwnCard.Add(p);
        CurNum++;
        if (CheckWin())
            return 666;

        int choice = (int)OwnCard.First();
        Discard(choice);
        return choice;
    }

    public int Discard(int n)
    {
        // 播放弃牌音效
        //var music = new MusicE();
        //music.PlayDiscardSound();
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
        var curCard2 = new List<int>(CurNum);
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
        var curCard = new List<int>(CurNum);
        int ii = 0;
        foreach (var it in OwnCard)
            curCard[ii++] = it;

        var dp = new List<int>(1 << CurNum);
        for (int i = 0; i < (1 << CurNum); i++) dp.Add(-1);
        return Check((1 << CurNum) - 1, curCard, dp);
    }

    public bool CheckWin(int n)
    {
        var curCard = new List<int>(CurNum);
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
        var curCard = new List<int>(CurNum);
        int idx = 0;
        foreach (var it in OwnCard)
            curCard[idx++] = it;

        int left = CountOne(n);
        if (left == 3)
        {
            var st = new List<int>();
            for (int j = 0; j < CurNum; j++)
            {
                if ((1 << j & n) == 0) st.Add(curCard[j]);
            }

            return st;
        }

        return null;
    }
}
