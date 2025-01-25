

public class MeleeEnemy : Enemy
{
	private void Awake()
	{
		_distanceToTarget = 1;
		_deadZone = .1f;
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
		// Melee Attack
		_meshRenderer.material.mainTexture = _attackSprite;
		_state = EnemyState.Attacking;
	}
}