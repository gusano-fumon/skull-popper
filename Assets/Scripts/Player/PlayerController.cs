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
	[SerializeField] private float _gravity = -9.81f;

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
		Health = TotalHealth;
		_playerUI.UpdateHealth(Health);
	}

	private void Update()
	{
		if (PlayerController.IsDead) return;
		
		MoveXZ();
		TiltCamera();
	}

	private void MoveXZ()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		var move = transform.right * horizontal + transform.forward * vertical;
		_character.Move(_movementSpeed * Time.deltaTime * move);
		_character.Move(new Vector3(0, _gravity, 0) * Time.deltaTime);
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
		IsDead = true;
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
