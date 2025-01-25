

public class BaseEnemy : Enemy, ILife
{
	public EnergyBall bulletPrefab;
	public int Health { get; set; }

	public void Awake()
	{
		_distanceToTarget = 5;
		_deadZone = .2f;
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
        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
		bullet.Init(_target.position);
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