using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float distance = 7f;
	[SerializeField] private float sensitivity = 0.15f;
	[SerializeField] private float smoothSpeed = 8f;
	[SerializeField] private float minPitch = -20f;
	[SerializeField] private float maxPitch = 60f;

	private InputSystem_Actions inputActions;
	private float yaw;
	private float pitch = 20f;

	void Awake()
	{
		inputActions = new InputSystem_Actions();
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnEnable()
	{
		inputActions.Player.Enable();
	}

	private void OnDisable()
	{
		inputActions.Player.Disable();
	}

	void LateUpdate()
	{
		Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
		yaw += lookInput.x * sensitivity;
		pitch -= lookInput.y * sensitivity;
		pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

		Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
		Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;
		transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
		transform.LookAt(target.position);
	}
}
