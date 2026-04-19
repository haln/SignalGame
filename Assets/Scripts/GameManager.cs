using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	[SerializeField] private string gameSceneName = "TitleScene";
	[SerializeField] private GameObject winnerText;

	private bool _gameWon = false;

	private void Awake()
	{
		Instance = this;
	}

	public bool IsGameWon() => _gameWon;

	public void PlayerWon()
	{
		Debug.Log("Player Won!");
		_gameWon = true;
		winnerText.SetActive(true);
	}

	public void PlayerDied()
	{
		Debug.Log("Player Died!");
	}

	public void BackToTitle()
	{
		SceneManager.LoadScene(gameSceneName);
	}
}
