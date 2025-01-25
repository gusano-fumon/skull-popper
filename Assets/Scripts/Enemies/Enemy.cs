
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Sprite
{
	[SerializeField] protected NavMeshAgent _aiAgent;
	[SerializeField] protected float _distanceToTarget;
	[SerializeField] protected float _deadZone;

	protected Transform _target;

	private void Start()
	{
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_aiAgent.destination = _target.position;
	}

	protected override void Update()
	{
		base.Update();

		if (Time.frameCount % 3 == 0) Move();

		if (Time.frameCount % 200 == 0)
		{
			Attack();
		}
	}

	protected virtual void Move() { }
	protected virtual void Attack() { }
}