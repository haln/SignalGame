using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

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
	}

	public void PlayerDied()
	{
		Debug.Log("Player Died!");
	}
}
