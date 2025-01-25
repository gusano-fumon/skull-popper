
using UnityEngine;


public class EnergyBall : Bullet
{
	[SerializeField] private float _speed = 5;
	private Vector3 _direction;

	private void FixedUpdate()
	{
		Move();
	}

	public void Init(Vector3 target)
	{
		_targetTag = "Player";
		_direction = (target - transform.position).normalized;

		transform.LookAt(target);
	}

	private void Move()
	{
		transform.localPosition += _speed * Time.deltaTime * _direction;
	}

	public override void ExecuteCollision()
	{
		base.ExecuteCollision();
		PlayerController.OnHit.Invoke(1);
	}
}