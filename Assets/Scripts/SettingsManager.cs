using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private Volume globalVolume;

	private Bloom _bloom;
	private Vignette _vignette;
	private ColorAdjustments _colorAdjustments;

	void Awake()
	{
		if (globalVolume != null)
		{
			globalVolume.profile.TryGet(out _bloom);
			globalVolume.profile.TryGet(out _vignette);
			globalVolume.profile.TryGet(out _colorAdjustments);
		}

		// Load saved settings
		AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
		if (_bloom != null)
			_bloom.active = PlayerPrefs.GetInt("Bloom", 1) == 1;
		if (_vignette != null)
			_vignette.active = PlayerPrefs.GetInt("Vignette", 1) == 1;
	}

	public void SetMasterVolume(float value)
	{
		AudioListener.volume = value;
		PlayerPrefs.SetFloat("MasterVolume", value);
	}

	public float GetMasterVolume() => PlayerPrefs.GetFloat("MasterVolume", 1f);

	public void SetBloom(bool value)
	{
		if (_bloom != null) _bloom.active = value;
		PlayerPrefs.SetInt("Bloom", value ? 1 : 0);
	}

	public bool GetBloom() => PlayerPrefs.GetInt("Bloom", 1) == 1;

	public void SetVignette(bool value)
	{
		if (_vignette != null) _vignette.active = value;
		PlayerPrefs.SetInt("Vignette", value ? 1 : 0);
	}

	public bool GetVignette() => PlayerPrefs.GetInt("Vignette", 1) == 1;
}