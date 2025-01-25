
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;


public enum EnemyState {
	Idle,
	Walking,
	Attacking,
	ReceivingDamage
}

public class Enemy : Sprite
{
	[SerializeField] protected NavMeshAgent _aiAgent;
	[SerializeField] protected float _distanceToTarget;
	[SerializeField] protected float _deadZone;

	protected Transform _target;
	protected EnemyState _state;
	protected float _remainingDistance;
	private bool _alredySeen = false;

	private void Start()
	{
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_aiAgent.destination = transform.position;
	}

	protected override void Update()
	{
		base.Update();

		if (PlayerController.GameEnd) return;

		_remainingDistance = (_target.position - transform.position).magnitude;
		if (_remainingDistance > 50) return;

		if (!_alredySeen)
		{
			var ray = new Ray(transform.position, _target.position - transform.position);
			if (Physics.Raycast(ray, out var hit, _remainingDistance))
			{
				if (!hit.collider.CompareTag("Player")) return;
				_alredySeen = true;
			}
		}

		UniTask.Void(async () => {
			if (_state is EnemyState.Attacking or EnemyState.ReceivingDamage)
			{
				await UniTask.Delay(500);
			}

			if (Time.frameCount % 3 == 0) Move();

			if (Time.frameCount % 300 == 0)
			{
				var ray = new Ray(transform.position, _target.position - transform.position);
				if (Physics.Raycast(ray, out var hit, _remainingDistance))
				{
					if (!hit.collider.CompareTag("Player")) return;
					Attack();
				}
			}
		});
	}

	protected virtual void Move() { }
	protected virtual void Attack() { }
}