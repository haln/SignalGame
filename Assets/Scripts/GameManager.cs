using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	[SerializeField] private string gameSceneName = "TitleScene";
	[SerializeField] private GameObject winnerText;

	[Header("Menu Stuff")]
	[SerializeField] private GameObject settingsPanel;

	private InputSystem_Actions inputActions;
	private float menuCooldown = 1f;
	private float currentMenuCooldown = 0f;

	private bool _gameWon = false;
	public bool IsGameWon() => _gameWon;

	private void Awake()
	{
		Instance = this;
		inputActions = new InputSystem_Actions();
	}

	void OnEnable()
	{
		inputActions.Global.Enable();
	}

	private void OnDisable()
	{
		inputActions.Global.Disable();
	}

	private void Update()
	{
		if (PlayerPrefs.GetInt("TutorialSeen", 0) == 0)
		{
			return;
		}
		HandleMenu();
	}

	private void HandleMenu()
	{
		if (currentMenuCooldown > 0f)
		{
			currentMenuCooldown -= Time.deltaTime;
			return;
		}

		bool pressedMenuButton = inputActions.Global.Menu.IsPressed();
		if (!pressedMenuButton)
		{
			return;
		}

		currentMenuCooldown = menuCooldown;
		settingsPanel.SetActive(!settingsPanel.activeSelf);
		Cursor.lockState = settingsPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
	}

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
