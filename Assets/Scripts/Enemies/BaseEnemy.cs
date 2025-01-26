

public class BaseEnemy : Enemy
{
	public EnergyBall bulletPrefab;
	public void Awake()
	{
		_distanceToTarget = 10;
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
		_meshRenderer.material.mainTexture = _attackSprite;
		_state = EnemyState.Attacking;

		var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
		bullet.Init(_target.position);
	}

	protected override void Die()
	{
		_aiAgent.baseOffset = .8f;
		_aiAgent.destination = transform.position;
		base.Die();
	}

	protected override void SetWalkingSprite()
	{
		_meshRenderer.material.mainTexture = _walkingSprites[_spriteCounter % 2];
	}
}