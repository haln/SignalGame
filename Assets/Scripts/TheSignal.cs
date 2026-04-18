using System;
using System.Collections;
using UnityEngine;

public enum SignalState { Green, Yellow2ToRed, Yellow1ToRed, Red, Yellow1ToGreen, Yellow2ToGreen }

public class TheSignal : MonoBehaviour
{
    [Header("Light Renderers")]
    public Renderer redLight;
    public Renderer yellowLight1;
    public Renderer yellowLight2;
    public Renderer greenLight;

    [Header("Durations (seconds)")]
    public float greenDuration = 5f;
    public float yellowDuration = 1f;
    public float redDuration = 5f;

    [Header("Audio")]
    public AudioClip sfxRed;
	public AudioClip sfxYellow;
	public AudioClip sfxGreen;

    private AudioSource _audio;

	public SignalState CurrentState { get; private set; }
    public event Action<SignalState> OnStateChanged;

    private const float Brightness = 3f;
    private static readonly int EmissionID = Shader.PropertyToID("_EmissionColor");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            yield return Transition(SignalState.Green, greenDuration);
			yield return Transition(SignalState.Yellow2ToRed, yellowDuration);
			yield return Transition(SignalState.Yellow1ToRed, yellowDuration);
			yield return Transition(SignalState.Red, redDuration);
			yield return Transition(SignalState.Yellow1ToGreen, yellowDuration);
			yield return Transition(SignalState.Yellow2ToGreen, yellowDuration);
		}
    }

    IEnumerator Transition(SignalState state, float duration)
    {
        SetState(state);
        yield return new WaitForSeconds(duration);
    }

    void SetState(SignalState state)
    {
        CurrentState = state;

        // Dim everything, light up active bulb
        SetEmission(redLight, Color.black);
		SetEmission(yellowLight1, Color.black);
		SetEmission(yellowLight2, Color.black);
		SetEmission(greenLight, Color.black);

        switch (state)
        {
            case SignalState.Red: 
                SetEmission(redLight, Color.red);
                break;
            case SignalState.Yellow1ToRed:
            case SignalState.Yellow1ToGreen:
                SetEmission(yellowLight1, Color.yellow);
                break;
            case SignalState.Yellow2ToGreen:
            case SignalState.Yellow2ToRed:
                SetEmission(yellowLight2, Color.yellow);
                break;
            case SignalState.Green:
                SetEmission(greenLight, Color.green);
                break;
        }

        AudioClip clip = state switch
        {
            SignalState.Red => sfxRed,
            SignalState.Green => sfxGreen,
            SignalState.Yellow1ToGreen or
            SignalState.Yellow2ToGreen or
            SignalState.Yellow1ToRed or
            SignalState.Yellow2ToRed => sfxYellow,
            _ => null
        };

        if (clip != null) _audio.PlayOneShot(clip);
        OnStateChanged?.Invoke(state);
	}

    void SetEmission(Renderer r, Color color)
    {
        r.material.SetColor(EmissionID, color * Brightness);
    }
}
