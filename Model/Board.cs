using System;
using System.Collections.Generic;

namespace Majiang
{
    public class Card
    {
        // 定义卡片相关的成员变量
        private List<int> card;
        private Dictionary<int, string> card_name;
        private Queue<int> dq;
        private readonly string[] f = { "tong", "tiao", "wan" };
        private readonly string[] za = { "hongZhong", "faCai", "ban" };
        private readonly string[] nums = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        private int tot;

        // 构造函数
        public Card()
        {
            card = new List<int>(new int[137]);
            card_name = new Dictionary<int, string>();
            dq = new Queue<int>();
            tot = 136;

            // 初始化卡片
            for (int i = 1; i <= 136; i++)
                card[i] = i;

            // 初始化牌名
            for (int i = 1; i <= 108; i++)
            {
                int p = (i - 1) / 36;
                int n = (i - 1) % 9;
                card_name[i] = nums[n] + f[p];
            }

            // 初始化风牌
            for (int i = 0; i < 4; i++) card_name[i + 109] = "dongFeng";
            for (int i = 0; i < 4; i++) card_name[i + 113] = "nanFeng";
            for (int i = 0; i < 4; i++) card_name[i + 117] = "xiFeng";
            for (int i = 0; i < 4; i++) card_name[i + 121] = "beiFeng";

            // 初始化字牌
            for (int i = 125; i <= 136; i++)
            {
                card_name[i] = za[(i - 125) / 4];
            }
        }

        // 生成一个随机数
        private int GenerateRandomNumber(int n)
        {
            Random random = new Random();
            return random.Next(1, n + 1);
        }

        // 打乱卡牌
        public void Start()
        {
            for (int i = 1; i <= tot; i++)
            {
                int target = GenerateRandomNumber(tot);
                // 交换卡片
                Swap(card, i, target);
            }

            // 将打乱的卡牌加入队列
            for (int i = 1; i <= tot; i++)
            {
                dq.Enqueue(card[i]);
            }
        }

        // 获取一张牌
        public int GetCard()
        {
            if (dq.Count == 0)
                return 0;
            return dq.Dequeue();
        }

        // 获取最后一张牌
        public int GetBackCard()
        {
            if (dq.Count == 0)
                return 0;
            return dq.Dequeue();
        }

        // 获取卡牌的名称
        public string GetName(int n)
        {
            return card_name.ContainsKey(n) ? card_name[n] : "Unknown";
        }

        // 判断卡牌队列是否为空
        public bool IsEmpty()
        {
            return dq.Count == 0;
        }

        // 交换两个卡片的值
        /*
        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        */
         private static List<T> Swap<T>(List<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }
    }
}
