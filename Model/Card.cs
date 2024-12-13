using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card
{
    private List<int> card;
    private readonly string[] f = { "tong", "tiao", "wan" };
    private readonly string[] za = { "hongZhong", "faCai", "ban" };
    private readonly string[] nums = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    private Dictionary<int, string> card_name;
    private int tot;

    public Queue<int> dq;

    public Card()
    {
        card = new List<int>(new int[137]);
        card_name = new Dictionary<int, string>();
        dq = new Queue<int>();
        tot = 136;

        for (int i = 1; i <= 136; i++)
            card[i] = i;

        for (int i = 1; i <= 108; i++)
        {
            int p = (i - 1) / 36;
            int n = (i - 1) % 9;
            card_name[i] = nums[n] + f[p];
        }

        for (int i = 0; i < 4; i++) card_name[i + 109] = "dongFeng";
        for (int i = 0; i < 4; i++) card_name[i + 113] = "nanFeng";
        for (int i = 0; i < 4; i++) card_name[i + 117] = "xiFeng";
        for (int i = 0; i < 4; i++) card_name[i + 121] = "beiFeng";
        for (int i = 125; i <= 136; i++)
        {
            card_name[i] = za[(i - 125) / 4];
        }
    }

    public int GenerateRandomNumber(int n)
    {
        Random rand = new Random();
        return rand.Next(1, n + 1);
    }

    //洗牌
    public void Start()
    {
        for (int i = 1; i <= tot; i++)
        {
            int tar = GenerateRandomNumber(tot);
            int temp = card[i];
            card[i] = card[tar];
            card[tar] = temp;
        }

        for (int i = 1; i <= tot; i++)
        {
            dq.Enqueue(card[i]);
        }
    }

    public int GetCard()
    {
        if (dq.Count == 0)
            return 0;
        return dq.Dequeue();
    }

    public int GetBackCard()
    {
        if (dq.Count == 0)
            return 0;

        int[] arr = dq.ToArray();
        int backCard = arr[arr.Length - 1];
        dq.Dequeue();  // Removing the last card manually
        return backCard;
    }

    public string GetName(int n)
    {
        if (n > 0) return card_name[n];
        else return "";
    }

    public bool IsEmpty()
    {
        return dq.Count == 0;
    }
}