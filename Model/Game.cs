using System.Collections.Generic;

public class Game
{
    public Card card;
    public List<Player> player;
    public int round;
    public string winner;
    public int cur;

    public Game(string name)
    {
        winner = "";
        card = new Card();
        player = new List<Player>(new Player[4]);

        for (int i = 1; i <= 3; i++)
        {
            string ss = "robot";
            ss += i.ToString();
            player[i] = new Player(ss, card);
        }

        player[0] = new Player(name, card);
        cur = card.GenerateRandomNumber(4) - 1;
        card.Start();
        DistributeCards();
    }

    public void DistributeCards()
    {
        for (int j = 0; j < 13; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                player[i].GetCard();
            }
        }
    }

}
