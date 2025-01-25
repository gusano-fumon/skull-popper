
public class MeleeEnemy : Enemy
{
	private void Awake()
	{
		_distanceToTarget = 1;
		_deadZone = .1f;
	}

	protected override void Move()
	{
		var remainingDistance = (_target.position - transform.position).magnitude;
		if (_distanceToTarget + _deadZone > remainingDistance && remainingDistance > _distanceToTarget - _deadZone)
		{
			_aiAgent.destination = transform.position;
			return;
		}

		if (remainingDistance > _distanceToTarget)
			_aiAgent.destination = _target.position;
		else
			_aiAgent.destination = transform.position + (transform.position - _target.position).normalized * _distanceToTarget;
	}

	protected override void Attack()
	{
		// Melee Attack
	}
}