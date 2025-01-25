using System;

using UnityEngine;


public class MouseLook : MonoBehaviour
{
	public PlayerController player;
	public float sensitivity = 100f;
	[SerializeField] private Bubble _bubble;
	private float _rotationX;

	public int rechargeTimeWindow = 3000;
	public int requiredMovements = 3;
	public float movementThreshold = 50f;

	private int currentMovementCount = 0;
	private float lastMovementTime = 0f;
	private bool expectingUpMovement = true;
	private bool _recharging = false;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (PlayerController.IsDead) return;
		
		var scaledSesitivity = sensitivity * Time.deltaTime;
		var mouseY = Input.GetAxis("Mouse Y") * scaledSesitivity;

		if (!_recharging)
		{
			var mouseX = Input.GetAxis("Mouse X") * scaledSesitivity;

			player.transform.Rotate(mouseX * Vector3.up);

			_rotationX -= mouseY;
			_rotationX = Math.Clamp(_rotationX, -90, 90);

			transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
		}
		else
		{
			if (Time.time - lastMovementTime > rechargeTimeWindow)
			{
				ResetRecharge();
			}

			// Detect significant mouse movement
			if (Mathf.Abs(mouseY) > movementThreshold / 100f)
			{
				// Check for up movement when expecting up
				if (expectingUpMovement && mouseY > 0)
				{
					UpdateMovementCount(true);
				}
				// Check for down movement when not expecting up
				else if (!expectingUpMovement && mouseY < 0)
				{
					UpdateMovementCount(false);
				}
			}
		}

		if (Input.GetMouseButtonDown(1)) _recharging = true;
		if (Input.GetMouseButtonUp(1)) _recharging = false;

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	private void UpdateMovementCount(bool isUpMovement)
	{
		// Update last movement time
		lastMovementTime = Time.time;

		// Toggle expected movement direction
		expectingUpMovement = !isUpMovement;

		// Increment movement count
		currentMovementCount++;

		// Check if recharge is complete
		if (currentMovementCount >= requiredMovements * 2)
		{
			PerformRecharge();
		}
	}

	private void ResetRecharge()
	{
		Debug.Log("Reset Recharge!");
		currentMovementCount = 0;
		expectingUpMovement = true;
	}

	private void PerformRecharge()
	{
		Debug.Log("Recharge Complete!");
		// Add your recharge logic here
		// For example:
		// playerHealth.Recharge();
		// energySystem.FullyRecharge();

		// Reset for next recharge attempt
		ResetRecharge();
	}

	private void Shoot()
	{
		if (_recharging) return;
		var bubble = Instantiate(_bubble, transform.position, transform.rotation);
		bubble.Init(transform.forward);

		player.audioController.PlayShotClip(bubble.transform);
	}
}