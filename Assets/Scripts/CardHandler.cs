using UnityEngine;
using TMPro;

public class CardHandler : MonoBehaviour
{
    // use this to show our cards to the other players
    public GameObject cardOnTable;

    private int currentCardNumber = 0;
    private TextMeshPro cardOnTableText;
    private TextMeshPro cardNumberText;
    GameManager GM;

    private void Awake()
    {
        GM = GameManager.Instance;
        GM.StartNewRound += pullNewCard;
        if (transform.parent.name != "Player1")
        {
            // hardcoded to one player mode
            GM.BotMove += BotMove;
        }
        cardOnTableText = cardOnTable.transform.Find("Number").gameObject.GetComponent<TextMeshPro>();
        cardNumberText = transform.Find("Number").gameObject.GetComponent<TextMeshPro>();
        currentCardNumber = GM.pullNextCardFromDeck();
        setCardNumberText(cardNumberText, currentCardNumber);
        setCardNumberText(cardOnTableText, 0);
    }
    public void BotMove(string playerName, string cardName)
    {
        // only one card per botPlayer per round will call this function
        if (transform.parent.name == playerName && transform.name == cardName)
        {
            cardTouched();
        }
    }
    public void cardTouched()
    {
        // "disable" touch other cards until round is finished
        if (GM.playerDict[transform.parent.name].placedCardNumber == -1)
        {
            setCardNumberText(cardOnTableText, currentCardNumber);
            GM.playerDict[transform.parent.name].placedCardNumber = currentCardNumber;
            currentCardNumber = -1;
            setCardNumberText(cardNumberText, currentCardNumber);
            GM.checkRoundEnd();
        }
    }

    public void pullNewCard()
    {
        if (currentCardNumber == -1)
        {
            currentCardNumber = GM.pullNextCardFromDeck();
            setCardNumberText(cardNumberText, currentCardNumber);
            setCardNumberText(cardOnTableText, -1);
        }
    }

    private void setCardNumberText(TextMeshPro tmPro, int number)
    {
        if (number == -1)
        {
            tmPro.text = " ";
            return;
        }
        tmPro.text = number.ToString();
    }

    private void OnDestroy()
    {
        GM.StartNewRound -= pullNewCard;
        if (transform.parent.name != "Player1")
        {
            GM.BotMove -= BotMove;
        }
    }
}
