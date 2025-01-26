
using System;

using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour, ILife
{
	private static Transform _cameraTransform;
	public static Transform CameraTransform { get { return _cameraTransform; } } 
	public static Action<int> OnHit;
	public static Action<int> OnPlayerHeal;
	public static Action OnPlayerDeath;
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
	[SerializeField] private PlayerUI _playerUI;
	public AudioController audioController;

	public Image defaultState, reloadingState;
	public UnityEngine.Sprite upReload, downReload;

	public int Health { get; set; }

	private void Awake()
	{
		GameEnd = false;
		_cameraTransform = Camera.main.transform;
		OnHit += TakeDamage;
		OnPlayerHeal += RestoreHealth;
		VictoryZone.OnVictory += () => GameEnd = true;
	}

	private void OnDestroy()
	{
		OnHit -= TakeDamage;
		OnPlayerHeal -= RestoreHealth;
		VictoryZone.OnVictory -= () => GameEnd = true;
	}

	public void ReloadDirection(bool up)
	{
		reloadingState.sprite = up ? upReload : downReload;
	}


	private void Start()
	{
		Debug.Log("PlayerController Start");
		Health = TotalHealth;
		_playerUI.UpdateHealth(Health);
	}

	private void Update()
	{
		if (GameEnd) return;

		Movement();
		TiltCamera();
	}

	private void Movement()
	{
		_isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.9f, groundMask);
		// Aplicar gravedad
		if (!_isGrounded)
		{
			velocity.y += _gravity * Time.deltaTime;
		}

		if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			velocity.y = Mathf.Sqrt(jumpForce * -2f * _gravity);
		}

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		var move = transform.right * horizontal + transform.forward * vertical;
		_character.Move(_movementSpeed * Time.deltaTime * move);

		velocity.y += _gravity * Time.deltaTime;
		_character.Move(velocity * Time.deltaTime);
	}

	private void TiltCamera()
	{
		float z = Input.GetAxis("Horizontal") * -2f;
		Vector3 euler = transform.localEulerAngles;
		euler.z = z;
		transform.localEulerAngles = euler;
	}

	public void Die()
	{
		Health = 0;
		_playerUI.UpdateHealth(Health);
		GameEnd = true;
		OnPlayerDeath?.Invoke();
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		_playerUI.UpdateHealth(Health);
		if (Health > 0)
		{
			audioController.PlayHitClip(transform);
			return;
		}

		if (Health <= 0)
		{
			Die();
		}
	}

	public void RestoreHealth(int health)
	{
		Health += health;

		if (Health > TotalHealth)
		{
			Health = TotalHealth;
		}

		_playerUI.UpdateHealth(Health);
	}
}
