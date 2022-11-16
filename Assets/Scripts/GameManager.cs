using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

// delegates for the different events
public delegate void Notify();
public delegate void NotifyString(string _string);
public delegate void NotifyDoubleString(string _string1, string _string2);
public delegate void NotifyStringInt(string _string, int _int);

public class GameManager
{
    // custom dataStructure to store the needed data for the players
    public class playerData
    {
        public int sumPoints { get; set; }
        public int placedCardNumber { get; set; }
        public playerData(int sum, int placed)
        {
            this.sumPoints = sum;
            this.placedCardNumber = placed;
        }
    }

    protected GameManager() {
        InitDataBase();
        createShuffledDeck();
    }
    static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (GameManager._instance == null)
            {
                GameManager._instance = new GameManager();
            }
            return GameManager._instance;
        }
    }
    // function for resetting the game, like restarting from menu
    public void ResetGM()
    {
        ResetDataBase();
        createShuffledDeck();
    }
    public void InitDataBase()
    {
        playerDict.Add("Player1", new playerData(0, -1));
        playerDict.Add("Player2", new playerData(0, -1));
        playerDict.Add("Player3", new playerData(0, -1));
        playerDict.Add("Player4", new playerData(0, -1));
    }
    public void ResetDataBase()
    {
        playerDict["Player1"] = new playerData(0, -1);
        playerDict["Player2"] = new playerData(0, -1);
        playerDict["Player3"] = new playerData(0, -1);
        playerDict["Player4"] = new playerData(0, -1);
    }

    // function to shuffle a deck of cards from 1-99; this shuffle should be good enough for this game
    public void createShuffledDeck()
    {
        // we need a temporary array to shuffle the deck then we will "convert" it to a queue
        var rand = new Random();
        int[] tempDeck = Enumerable.Range(1, deckSize).ToArray();
        for (int i = tempDeck.Length - 1; i > 0; i--)
        {
            var nextRand = rand.Next(0, i);
            var temp = tempDeck[nextRand];
            tempDeck[nextRand] = tempDeck[i];
            tempDeck[i] = temp;
        }
        cardDeck.Clear();
        foreach(int number in tempDeck)
        {
            cardDeck.Enqueue(number);
        }
    }

    public int pullNextCardFromDeck()
    {
        int res;
        cardDeck.TryDequeue(out res);
        return res;
    }

    // events that we need for this game
    public event Notify UpdateScore;
    public event Notify StartNewRound;
    public event NotifyDoubleString BotMove;
    public event NotifyString WinnerIsHere;
    public event NotifyStringInt AnnounceRoundWinner;

    // player data is stored in a dictionary
    public IDictionary<string, playerData> playerDict = new Dictionary<string, playerData>();
    // need this queue to manage the race condition during deck placement beacuse of the multiple card instantiation at the same time
    public ConcurrentQueue<int> cardDeck = new ConcurrentQueue<int>();

    private int targetPoint = 150;
    private int deckSize = 99;

    public void OnUpdateScore()
    {
        UpdateScore?.Invoke();
        foreach (KeyValuePair<string, playerData> kvp in playerDict)
        {
            // check if there is a winner after each round
            if (kvp.Value.sumPoints > targetPoint)
            {
                WinnerIsHere?.Invoke(kvp.Key);
            }
        }
    }
    public void checkRoundEnd()
    {
        foreach (KeyValuePair<string, playerData> kvp in playerDict)
        {
            if (kvp.Value.placedCardNumber == -1)
            {
                OnBotMove(kvp.Key);
                return;
            }
        }
        calculateRoundWinner();
    }
    public void OnBotMove(string player)
    {
        // choose a random card to place for the bots
        string name = "Card" + UnityEngine.Random.Range(1, 6).ToString();
        BotMove?.Invoke(player, name);
    }
    public void calculateRoundWinner()
    {
        int min = deckSize;
        string roundWinner = "";
        foreach (KeyValuePair<string, playerData> kvp in playerDict)
        {
            if (kvp.Value.placedCardNumber < min)
            {
                min = kvp.Value.placedCardNumber;
                roundWinner = kvp.Key;
            }
        }
        playerDict[roundWinner].sumPoints += min;
        OnUpdateScore();
        AnnounceRoundWinner?.Invoke(roundWinner, min);
    }
    public void startNewRound()
    {
        StartNewRound?.Invoke();
        foreach (KeyValuePair<string, playerData> kvp in playerDict)
        {
            kvp.Value.placedCardNumber = -1;
        }
    }
    public string generateFinalScore()
    {
        // this function will collect all of the players' points and put them into a nice string to display on the final screen
        string ret = "";
        foreach (KeyValuePair<string, playerData> kvp in playerDict)
        {
            ret += kvp.Key + " - " + kvp.Value.sumPoints.ToString() + "\n";
        }
        return ret;
    }
}
