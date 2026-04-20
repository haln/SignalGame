using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] private GameObject tutorialPanel;
	[SerializeField] private TextMeshProUGUI slideText;
	[SerializeField] private TextMeshProUGUI slideCounterText;
	[SerializeField] private TextMeshProUGUI nextButtonText;

	[Header("Cameras")]
	[SerializeField] private CinemachineCamera vanCam;
	[SerializeField] private CinemachineCamera signalCam;
	[SerializeField] private CinemachineCamera bombCam;

	private List<CutsceneData> slides = new List<CutsceneData>();
	private int _currentSlide = 0;
	private InputSystem_Actions inputActions;
	private float skipCooldown = 0.5f;
	private float currentSkipCooldown = 0f;

	private void Awake()
	{
		inputActions = new InputSystem_Actions();
		slides.Clear();
		slides.Add(new CutsceneData
		{
			CutsceneText = "You have been kidnapped.\n There is a van you can escape with.\n\nGet to it alive.",
			Camera = vanCam,
		});
		slides.Add(new CutsceneData
		{
			CutsceneText = "There's a signal between you and freedom.\n\nMove when it's <color=green>GREEN</color>.\nFreeze when it's <color=red>RED</color>.",
			Camera = signalCam,
		});
		slides.Add(new CutsceneData
		{
			CutsceneText = "If you move on <color=red>RED</color> the bomb strapped to your chest will explode.\n\n\nPress <b>WASD</b> to move. <b>ESC</b> for settings.\nGood luck, victim.",
			Camera = bombCam,
		});
	}

	void OnEnable()
	{
		inputActions.Cinematics.Enable();
	}

	private void OnDisable()
	{
		inputActions.Cinematics.Disable();
	}

	void Start()
	{
#if !UNITY_EDITOR
		if (PlayerPrefs.GetInt("TutorialSeen", 0) == 1)
		{
			EndTutorial();
			return;
		}
#endif

		Time.timeScale = 0f;
		tutorialPanel.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		ShowSlide(0);
	}

	void Update()
	{
		if (currentSkipCooldown > 0f)
		{
			currentSkipCooldown -= Time.unscaledDeltaTime;
		}

		// Press button to advance
		if (inputActions.Cinematics.Skip.IsPressed() && currentSkipCooldown <= 0f)
		{
			currentSkipCooldown = skipCooldown;
			NextSlide();
		}
			
	}

	void ShowSlide(int index)
	{
		CutsceneData cutsceneData = slides[index];

		cutsceneData.Camera.gameObject.SetActive(true);
		slideText.text = cutsceneData.CutsceneText;
		slideCounterText.text = $"{index + 1} / {slides.Count}";
		nextButtonText.text = index == slides.Count - 1 ? "Play" : "Next";
	}

	public void NextSlide()
	{
		slides[_currentSlide].Camera.gameObject.SetActive(false);
		_currentSlide++;
		
		if (_currentSlide >= slides.Count)
			EndTutorial();
		else
			ShowSlide(_currentSlide);
	}

	public void Skip()
	{
		slides[_currentSlide].Camera.gameObject.SetActive(false);
		EndTutorial();
	}

	void EndTutorial()
	{
		PlayerPrefs.SetInt("TutorialSeen", 1);
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1f;
		tutorialPanel.SetActive(false);
	}
}