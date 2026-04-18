using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float smoothSpeed = 10f;
	[SerializeField] private Transform cameraTransform;

	private Rigidbody rb;
	private InputSystem_Actions inputActions;
	private Vector2 moveInput;

	void Awake()
	{
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

	void FixedUpdate()
	{
		moveInput = inputActions.Player.Move.ReadValue<Vector2>();

		// Positioning logic
		Quaternion cameraYaw = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
		Vector3 direction = cameraYaw * new Vector3(moveInput.x, 0f, moveInput.y).normalized;
		rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

		// Rotation logic
		Quaternion targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
		rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, smoothSpeed * Time.fixedDeltaTime));
	}
}