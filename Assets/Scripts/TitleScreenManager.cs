using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
	[SerializeField] private string gameSceneName = "GameScene";

	public void StartGame()
	{
		SceneManager.LoadScene(gameSceneName);
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}