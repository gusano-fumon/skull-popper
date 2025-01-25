
using UnityEngine;


public class BaseEnemy : Enemy, ILife
{
	public int totalHealth = 5;
	public EnergyBall bulletPrefab;
	public int Health { get; set; }
	[SerializeField] private Texture2D[] _walkingSprites;
	[SerializeField] private Texture2D _shootSprite;
	[SerializeField] private Texture2D _hurtSprite;
	[SerializeField] private Texture2D _idleSprite;

	public void Awake()
	{
		_distanceToTarget = 10;
		Health = totalHealth;
		_deadZone = .2f;
	}

	protected override void Move()
	{
		if (_distanceToTarget + _deadZone > _remainingDistance && _remainingDistance > _distanceToTarget - _deadZone)
		{
			_state = EnemyState.Idle;
			_aiAgent.destination = transform.position;
			return;
		}

		_state = EnemyState.Walking;

		if (_remainingDistance > _distanceToTarget)
			_aiAgent.destination = _target.position;
		else
			_aiAgent.destination = transform.position + (transform.position - _target.position).normalized * _distanceToTarget;
	}

	protected override void Attack()
	{
		_meshRenderer.sharedMaterial.mainTexture = _shootSprite;
		_state = EnemyState.Attacking;

		var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
		bullet.Init(_target.position);
	}

	protected override void CheckSprite()
	{
		if (_state == EnemyState.Idle)
		{
			_meshRenderer.sharedMaterial.mainTexture = _idleSprite;
			return;
		}

		_spriteCounter++;
		_meshRenderer.sharedMaterial.mainTexture = _walkingSprites[_spriteCounter % 2];
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			_state = EnemyState.ReceivingDamage;
			_meshRenderer.sharedMaterial.mainTexture = _hurtSprite;
			Destroy(gameObject);
		}
	}

	public void RestoreHealth(int health)
	{
		Health += health;
	}
}