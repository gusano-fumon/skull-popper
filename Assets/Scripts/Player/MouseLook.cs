using System;

using UnityEngine;

using DG.Tweening;


public class MouseLook : MonoBehaviour
{
	public PlayerController player;

    public float sensitivity;

    [SerializeField] private Bubble _bubble;
	[SerializeField] private Transform _initialPos;
	[SerializeField] private Transform _bubbleWandArm;
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

	private void Awake()
	{
		_currentAmmo = _maxAmmo;
		sensitivity = PlayerSettings.Sensitivity;
		PlayerSettings.OnSensitiveChanged += SetSensitivity;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDestroy()
	{
		PlayerSettings.OnSensitiveChanged -= SetSensitivity;
	}

	private void Update()
	{
		if (PlayerController.GameEnd) return;
		if (GameMenu.IsPaused) return;

		if (Input.GetKeyDown(KeyCode.Escape)) // Pause Game
		{
			GameMenu.Instance.Pause();
			player.DOTogglePause();
			return;
		}
		
		var scaledSesitivity = sensitivity * Time.deltaTime;
		var mouseY = Input.GetAxis("Mouse Y") * scaledSesitivity;

		if (!_recharging)
		{
			var mouseX = Input.GetAxis("Mouse X") * scaledSesitivity;

			player.transform.Rotate(Vector3.up * mouseX);

			_rotationX -= mouseY;
			_rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

			transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
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
					AudioFactory.Instance.PlaySFX(AudioType.Recharge);
				}
				// Check for down movement when not expecting up
				else if (!expectingUpMovement && mouseY < 0)
				{
					UpdateMovementCount(false);
					GameMenu.Instance.playerUI.ReloadDirection(false);
					AudioFactory.Instance.PlaySFX(AudioType.Recharge);
				}
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			_recharging = true;
			GameMenu.Instance.playerUI.defaultState.SetActive(false);
			GameMenu.Instance.playerUI.reloadingStateUp.SetActive(true);
 		}

		if (Input.GetMouseButtonUp(1))
		{
			_recharging = false;
			ResetRecharge();
			GameMenu.Instance.playerUI.defaultState.SetActive(true);
			GameMenu.Instance.playerUI.reloadingStateUp.SetActive(false);
			GameMenu.Instance.playerUI.reloadingStateDown.SetActive(false);
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (_recharging) return;
			if (_currentAmmo == 0)
			{
				AudioFactory.Instance.PlaySFX(AudioType.NoAmmo);
				return;
			}

			// Efecto delay para el disparo de la burbuja

			GameMenu.Instance.playerUI.ShootAnimation().Forget();
			Invoke(nameof(Shoot), .2f);
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
			_currentAmmo = _maxAmmo;
			OnReload?.Invoke(_currentAmmo);
			ResetRecharge();
		}
	}

	private void ResetRecharge()
	{
		currentMovementCount = 0;
		expectingUpMovement = true;
	}

	private void Shoot()
	{
		if (_currentAmmo == 0) return;

		_currentAmmo--;
		ResetRecharge();

		OnReload?.Invoke(_currentAmmo);

		var bubble = Instantiate(_bubble, _initialPos.position, transform.rotation);
		bubble.Init(transform.forward);

		AudioFactory.Instance.PlaySFX(AudioType.Bubble);
	}

	private void SetSensitivity(float value)
	{
		sensitivity = value;
	}
}