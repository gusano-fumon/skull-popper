
using System;

using UnityEngine;


public class PlayerController : MonoBehaviour, ILife
{
	[SerializeField] private Camera _playerCamera;
	public static Camera PlayerCamera;
	public static Action<int> OnHit;
	public static Action<int> OnPlayerHeal;
	public int TotalHealth = 100;

	[SerializeField] private float _movementSpeed;
	[SerializeField] private float _gravity;

	private Vector3 velocity;
	public float jumpForce = 1.5f;
	public LayerMask groundMask;
	private bool _isGrounded;

	[Header("References")]
	[SerializeField] private CharacterController _character;

#region Properties

	public int Health { get; set; }
	public static bool GameEnd { get; private set; }

#endregion

	private void Awake()
	{
		GameEnd = false;
		PlayerCamera = _playerCamera;
		PlayerCamera.fieldOfView = PlayerSettings.FieldOfView;
		AudioFactory.Instance.PlayMusic(AudioType.BackgroundMusic);
		OnHit += TakeDamage;
		OnPlayerHeal += RestoreHealth;
		GameMenu.OnVictory += () => GameEnd = true;
		PlayerSettings.OnFovChanged += SetFOV;
	}

	private void OnDestroy()
	{
		OnHit -= TakeDamage;
		OnPlayerHeal -= RestoreHealth;
		PlayerSettings.OnFovChanged -= SetFOV;
		GameMenu.OnVictory -= () => GameEnd = true;
	}

	private void Start()
	{
		Health = TotalHealth;
	}

	private void Update()
	{
		if (GameEnd) return;
		if (GameMenu.IsPaused) return;

		Movement();
	}

	private void SetFOV(float fov)
	{
		PlayerCamera.fieldOfView = fov;
	}

	private void Movement()
	{
		_isGrounded = Physics.Raycast(transform.position, Vector3.down, .9f, groundMask);
		// Aplicar gravedad
		if (!_isGrounded)
		{
			velocity.y += _gravity * Time.deltaTime;
		}
		else 
		{
			velocity.y = 0;
		}

		if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			_isGrounded = false;
			AudioFactory.Instance.PlaySFX(AudioType.Jump);
			velocity.y = Mathf.Sqrt(jumpForce * -2f * _gravity);
		}

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		TiltCamera(horizontal * -2f);

		var move = transform.right * horizontal + transform.forward * vertical;
		_character.Move(_movementSpeed * Time.deltaTime * move);

		velocity.y += _gravity * Time.deltaTime;
		_character.Move(velocity * Time.deltaTime);
	}

	private void TiltCamera(float pos)
	{
		Vector3 euler = PlayerCamera.transform.localEulerAngles;
		euler.z = pos;
		PlayerCamera.transform.localEulerAngles = euler;
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		AudioFactory.Instance.PlaySFX(AudioType.PlayerHit);
		ScoreController.RemoveScore(damage);

		if (Health <= 0)
		{
			Health = 0;
			GameEnd = true;
			GameMenu.OnPlayerDeath?.Invoke();
			AudioFactory.Instance.PlaySFX(AudioType.PlayerDeath);
			AudioFactory.Instance.StopMusic();
		}

		PlayerUI.OnHit?.Invoke(Health, true);
	}

	public void RestoreHealth(int health)
	{
		Health += health;

		AudioFactory.Instance.PlaySFX(AudioType.Potion);

		if (Health > TotalHealth)
		{
			Health = TotalHealth;
		}

		PlayerUI.OnHit?.Invoke(Health, false);
	}
}