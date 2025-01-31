using System;
using UnityEngine;


public class MouseLook : MonoBehaviour
{
	public PlayerController player;
	public PlayerUI playerUI;
	public float sensitivity = 100f;
	[SerializeField] private Bubble _bubble;
	[SerializeField] private Transform _initialPos; 
	[SerializeField] private int _maxAmmo = 12;
	[SerializeField] private int _currentAmmo; 
	private float _rotationX;

	public int rechargeTimeWindow = 3000;
	public int requiredMovements = 3;
	public float movementThreshold = 50f;

	private int currentMovementCount = 0;
	private float lastMovementTime = 0f;
	private bool expectingUpMovement = true;
	private bool _recharging = false;

	public static Action<int> OnReload;

	private void Start()
	{
		_currentAmmo = _maxAmmo;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (PlayerController.GameEnd) return;
		
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
					GameMenu.Instance.playerUI.ReloadDirection(true);
					player.audioController.PlayRecarga(player.transform);
				}
				// Check for down movement when not expecting up
				else if (!expectingUpMovement && mouseY < 0)
				{
					UpdateMovementCount(false);
					GameMenu.Instance.playerUI.ReloadDirection(false);
					player.audioController.PlayRecarga(player.transform);
				}
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			_recharging = true;
			GameMenu.Instance.playerUI.defaultState.gameObject.SetActive(false);
			GameMenu.Instance.playerUI.reloadingState.gameObject.SetActive(true);
 		}

		if (Input.GetMouseButtonUp(1))
		{
			_recharging = false;
			ResetRecharge();
			GameMenu.Instance.playerUI.defaultState.gameObject.SetActive(true);
			GameMenu.Instance.playerUI.reloadingState.gameObject.SetActive(false);
		}

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
		currentMovementCount = 0;
		expectingUpMovement = true;
	}

	private void PerformRecharge()
	{
		_currentAmmo = _maxAmmo;
		OnReload?.Invoke(_currentAmmo);
		ResetRecharge();
	}

	private void Shoot()
	{
		if (_recharging || _currentAmmo == 0) return;

		_currentAmmo--;
		ResetRecharge();

		OnReload?.Invoke(_currentAmmo);

		var bubble = Instantiate(_bubble, _initialPos.position, transform.rotation);
		bubble.Init(transform.forward);

		player.audioController.PlayShotClip(bubble.transform);
	}
}