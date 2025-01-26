
using System;

using UnityEngine;
using UnityEngine.AI;
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
		Health = TotalHealth;
		_playerUI.UpdateHealth(Health);
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
		_playerUI.TakeDamage();
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

		audioController.PlayPocion(transform);

		if (Health > TotalHealth)
		{
			Health = TotalHealth;
		}

		_playerUI.UpdateHealth(Health);
	}
}
