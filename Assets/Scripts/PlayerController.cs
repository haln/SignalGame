using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float smoothSpeed = 10f;
	[SerializeField] private Transform cameraTransform;

	[Header("Death Stuff")]
	[SerializeField] private TheSignal signal;
	[SerializeField] private float settleDelay = 2f;

	[Header("Audio")]
	[SerializeField] private AudioClip explosionSfx;
	[SerializeField] private List<AudioClip> stepSfx;
	[SerializeField] private float stepDelay = 0.3f;

	private AudioSource _audio;
	private Rigidbody rb;
	private InputSystem_Actions inputActions;
	private Vector2 moveInput;
	private float stepTimer = 0f;

	private bool dead = false;
	private float groundedTimer = 0f;
	private bool settling = false;

	void Awake()
	{
		_audio = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
		inputActions = new InputSystem_Actions();
	}

	void OnEnable()
	{
		inputActions.Player.Enable();
	}

	private void OnDisable()
	{
		inputActions.Player.Disable();
	}

	private void Update()
	{
		moveInput = inputActions.Player.Move.ReadValue<Vector2>();

		// Check if player moving
		if (moveInput.sqrMagnitude <= 0.01f)
		{
			return;
		}

		stepTimer -= Time.deltaTime;

		if (stepTimer <= 0f)
		{
			stepTimer = stepDelay;
			AudioClip stepSound = stepSfx[Random.Range(0, stepSfx.Count)];
			_audio.PlayOneShot(stepSound);
		}
	}

	void FixedUpdate()
	{
		if (dead)
		{
			if (settling)
			{
				groundedTimer += Time.fixedDeltaTime;
				if (groundedTimer >= settleDelay)
				{
					Freeze();
				}
			}
			return;
		}

		moveInput = inputActions.Player.Move.ReadValue<Vector2>();
		if (moveInput.sqrMagnitude > 0.01f && signal != null && signal.CurrentState == SignalState.Red)
		{
			Die();
			return;
		}

		// Positioning logic
		Quaternion cameraYaw = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
		Vector3 direction = cameraYaw * new Vector3(moveInput.x, 0f, moveInput.y).normalized;
		rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

		// Rotation logic
		Quaternion targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
		rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, smoothSpeed * Time.fixedDeltaTime));
	}

	void Die()
	{
		dead = true;

		rb.linearDamping = 0f;
		rb.constraints = RigidbodyConstraints.None;
		Vector3 force = (Vector3.up * 2f + Random.insideUnitSphere) * 5f;
		rb.AddForce(force, ForceMode.Impulse);
		rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);

		Debug.Log("Player exploded for moving on red!");
		_audio.PlayOneShot(explosionSfx);
	}

	private void OnCollisionStay(Collision collision)
	{
		if (!dead || settling) return;

		foreach (ContactPoint contact in collision.contacts)
		{
			if (contact.normal.y > 0.5f)
			{
				settling = true;
				groundedTimer = 0f;
				return;
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (dead && settling)
		{
			settling = false;
			groundedTimer = 0f;
		}
	}

	void Freeze()
	{
		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.constraints = RigidbodyConstraints.FreezeAll;
	}
}