
using UnityEngine;
using UnityEngine.AI;


public class BaseEnemy : MonoBehaviour
{
	[SerializeField] private NavMeshAgent _aiAgent;
	[SerializeField] private Transform _target;
	[SerializeField] private float _distanceToTarget = 5;
	[SerializeField] private float _deadZone = .2f;
	[SerializeField] private EnergyBall _energyBall;

	private void Start()
	{
		_aiAgent.destination = _target.position;
	}

	private void Update()
	{
		transform.LookAt(_target);

		if (Time.frameCount % 3 == 0)
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

		if (Time.frameCount % 200 == 0)
		{
			var bullet = Instantiate(_energyBall, transform.position, transform.rotation);
			bullet.Init(_target.position);
		}
	}
}