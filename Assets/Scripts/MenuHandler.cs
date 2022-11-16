using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuHandler : MonoBehaviour
{
	GameManager GM;
    private void Awake()
    {
		GM = GameManager.Instance;
		GM.WinnerIsHere += WinnerMenu;
		GM.AnnounceRoundWinner += AnnounceRoundWinner;
	}
	public void StartGameButton()
	{
		SceneManager.LoadScene("MainMap");
	}
	public void AnnounceRoundWinner(string player, int score)
    {
		GameObject rWGo = transform.Find("RoundWinnerButton").gameObject;
		rWGo.transform.Find("RoundWinnerButtonText").gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("ROUND winner is {0} with {1} - TAP to continue!", player, score.ToString());
		rWGo.SetActive(true);
	}
	public void ContinueGame()
	{
		transform.Find("RoundWinnerButton").gameObject.SetActive(false);
		GM.startNewRound();
	}
	public void WinnerMenu(string text)
    {
		transform.Find("RoundWinnerButton").gameObject.SetActive(false);
		transform.Find("QuitToMainMenuButton").gameObject.SetActive(false);
		GameObject victoryGo = transform.Find("VictoryQuitButton").gameObject;
		victoryGo.transform.Find("VictoryQuitButtonTextPlayer").gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0} is the winner!", text);
		victoryGo.transform.Find("FinalPoints").gameObject.GetComponent<TextMeshProUGUI>().text = GM.generateFinalScore();
		victoryGo.SetActive(true);
		GM.ResetGM();
	}
	public void QuitToMainMenu()
    {
		GM.ResetGM();
		SceneManager.LoadScene("MainMenu");
	}
	public void Quit()
	{
		Debug.Log("Quit!");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
    private void OnDestroy()
    {
		GM.WinnerIsHere -= WinnerMenu;
		GM.AnnounceRoundWinner -= AnnounceRoundWinner;
    }
}
