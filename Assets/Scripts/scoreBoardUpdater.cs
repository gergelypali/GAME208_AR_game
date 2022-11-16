using UnityEngine;
using TMPro;

public class scoreBoardUpdater : MonoBehaviour
{
    GameManager GM;
    private void Awake()
    {
        GM = GameManager.Instance;
        GM.UpdateScore += updateScoreBoard;
    }

    public void updateScoreBoard()
    {
        transform.Find("Number").gameObject.GetComponent<TextMeshPro>().text = GM.playerDict[transform.parent.name].sumPoints.ToString();
    }

    private void OnDestroy()
    {
        GM.UpdateScore -= updateScoreBoard;
    }
}
