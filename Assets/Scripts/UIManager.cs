using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Header("Panels")]
	[SerializeField] private GameObject titlePanel;
	[SerializeField] private GameObject settingsPanel;

	[Header("Settings Controls")]
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private Toggle bloomToggle;
	[SerializeField] private Toggle vignetteToggle;

	[SerializeField] private SettingsManager settingsManager;

	void Start()
	{
		ShowTitle();

		// Initialize controls from saved settings
		volumeSlider.value = settingsManager.GetMasterVolume();
		bloomToggle.isOn = settingsManager.GetBloom();
		vignetteToggle.isOn = settingsManager.GetVignette();

		// Wire up listeners
		volumeSlider.onValueChanged.AddListener(settingsManager.SetMasterVolume);
		bloomToggle.onValueChanged.AddListener(settingsManager.SetBloom);
		vignetteToggle.onValueChanged.AddListener(settingsManager.SetVignette);
	}

	public void ShowTitle()
	{
		titlePanel.SetActive(true);
		settingsPanel.SetActive(false);
	}

	public void ShowSettings()
	{
		titlePanel.SetActive(false);
		settingsPanel.SetActive(true);
	}
}