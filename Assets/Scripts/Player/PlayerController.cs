
using System;

using UnityEngine;


public class PlayerController : MonoBehaviour, ILife
{
	private static Transform _cameraTransform;
	public static Transform CameraTransform { get { return _cameraTransform; } } 
	public static Action<int> OnHit;
	public static Action<int> OnPlayerHeal;
	public static bool GameEnd { get; private set; }
	public int TotalHealth = 100;

	[Header("Properties")]
	[SerializeField] private float _movementSpeed;
	[SerializeField] private float _gravity;

	private Vector3 velocity;
	public float jumpForce = 1.5f;
	public LayerMask groundMask;
	private bool _isGrounded;

	[Header("References")]
	[SerializeField] private CharacterController _character;

	public int Health { get; set; }

	private void Awake()
	{
		GameEnd = false;
		_cameraTransform = Camera.main.transform;
		OnHit += TakeDamage;
		OnPlayerHeal += RestoreHealth;
		GameMenu.OnVictory += () => GameEnd = true;
	}

	private void OnDestroy()
	{
		OnHit -= TakeDamage;
		OnPlayerHeal -= RestoreHealth;
		GameMenu.OnVictory -= () => GameEnd = true;
	}

	private void Start()
	{
		Health = TotalHealth;
	}

	private void Update()
	{
		if (GameEnd) return;

		Movement();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Vector3.down * 0.9f);
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
		Vector3 euler = CameraTransform.localEulerAngles;
		euler.z = pos;
		CameraTransform.localEulerAngles = euler;
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
