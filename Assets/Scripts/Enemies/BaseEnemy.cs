
using Cysharp.Threading.Tasks;
using UnityEngine;


public class BaseEnemy : Enemy, ILife
{
	public int totalHealth = 5;
	public EnergyBall bulletPrefab;
	public int Health { get; set; }
	[SerializeField] private MeshRenderer _meshRenderer;
	[SerializeField] private Texture2D[] _walkingSprites;
	[SerializeField] private Texture2D _shootSprite;
	[SerializeField] private Texture2D _idleSprite;

	public void Awake()
	{
		_distanceToTarget = 5;
		Health = totalHealth;
		_deadZone = .2f;
	}

	protected override void Move()
	{
		if (_state == EnemyState.Attacking)
		{
			UniTask.Void(async () => {
				await UniTask.Delay(200);
			});
		}

		var remainingDistance = (_target.position - transform.position).magnitude;
		if (_distanceToTarget + _deadZone > remainingDistance && remainingDistance > _distanceToTarget - _deadZone)
		{
			_state = EnemyState.Idle;
			_aiAgent.destination = transform.position;
			return;
		}

		_state = EnemyState.Walking;

		if (remainingDistance > _distanceToTarget)
			_aiAgent.destination = _target.position;
		else
			_aiAgent.destination = transform.position + (transform.position - _target.position).normalized * _distanceToTarget;
	}

    protected override void Attack()
    {
		_meshRenderer.sharedMaterial.SetTexture("_MainTex", _shootSprite);
		_state = EnemyState.Attacking;

        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
		bullet.Init(_target.position);
    }

	protected override void CheckSprite()
	{
		if (_state == EnemyState.Idle)
		{
			_meshRenderer.sharedMaterial.SetTexture("_MainTex", _idleSprite);
			return;
		}

		_spriteCounter++;
		_meshRenderer.sharedMaterial.SetTexture(
			"_MainTex",
			_spriteCounter % 2 == 0 ? _walkingSprites[0] : _walkingSprites[1]
		);
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void RestoreHealth(int health)
	{
		Health += health;
	}
}