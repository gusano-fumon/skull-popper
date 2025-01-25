using System;
using UnityEngine;


public class PlayerController : MonoBehaviour, ILife
{
	public static Action<int> OnHit;
	public static Action<int> OnPlayerHeal;
	public const int TotalHealth = 100;

	[Header("Properties")]
	[SerializeField] private float _movementSpeed;
	[SerializeField] private float _gravity = -9.81f;

	[Header("References")]
	[SerializeField] private CharacterController _character;
	[SerializeField] private PlayerUI _playerUI;

	[Header("Sounds")]
	public AudioClip playerHitSound;
	public AudioClip playerDieSound;
	public AudioClip playerHealSound;

	public int Health { get; set; }

	private void Awake()
	{
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
		MoveXZ();

		if (Health <= 0)
		{
			Die();
		}
	}

	private void MoveXZ()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		var move = transform.right * horizontal + transform.forward * vertical;
		_character.Move(_movementSpeed * Time.deltaTime * move);

		_character.Move(new Vector3(0, _gravity, 0) * Time.deltaTime);
	}

	public void Die()
	{
		Health = 0;
		_playerUI.UpdateHealth(Health);
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		Debug.Log("Player hit! Health: " + Health);
		_playerUI.UpdateHealth(Health);
	}

	public void RestoreHealth(int health)
	{
		Health += health;
		_playerUI.UpdateHealth(Health);
	}
}
