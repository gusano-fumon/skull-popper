using System;
using UnityEngine;


public class PlayerController : MonoBehaviour, ILife
{
	private static Transform _transform;
	public static Transform Transform { get { return _transform; } } 
	public static Action<int> OnHit;
	public static Action<int> OnPlayerHeal;
	public static Action OnPlayerDeath;
	public static bool IsDead { get; private set; }
	public int TotalHealth = 100;

	[Header("Properties")]
	[SerializeField] private float _movementSpeed;
	[SerializeField] private float _gravity = -15f;

	private Vector3 velocity;
	public float jumpForce = 1.5f;
	public LayerMask groundMask;
	private bool _isGrounded;


	[Header("References")]
	[SerializeField] private CharacterController _character;
	[SerializeField] private PlayerUI _playerUI;
	public AudioController audioController;

	public int Health { get; set; }

	private void Awake()
	{
		IsDead = false;
		_transform = transform;
		OnHit += TakeDamage;
		OnPlayerHeal += RestoreHealth;
	}

	private void OnDestroy()
	{
		OnHit -= TakeDamage;
		OnPlayerHeal -= RestoreHealth;
	}


	private void Start()
	{
		Debug.Log("PlayerController Start");
		Health = TotalHealth;
		_playerUI.UpdateHealth(Health);
	}

	private void Update()
	{
<<<<<<< Updated upstream
		if (PlayerController.IsDead) return;
		
		MoveXZ();
=======
		Movement();
>>>>>>> Stashed changes
		TiltCamera();
	}

	private void Movement()
	{
		_isGrounded = Physics.Raycast(transform.position, Vector3.down, 1, groundMask);

		if (_isGrounded && velocity.y < -10)
		{
			velocity.y = -10;
		}

		if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
		{
			Debug.Log("Jump");
			velocity.y = Mathf.Sqrt(jumpForce * -2 *_gravity);
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

<<<<<<< Updated upstream
	public void Die()
	{
		Health = 0;
		_playerUI.UpdateHealth(Health);
		IsDead = true;
		OnPlayerDeath?.Invoke();
	}

=======
>>>>>>> Stashed changes
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
