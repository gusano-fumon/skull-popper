
using UnityEngine;


public class EnergyBall : Bullet
{
	[SerializeField] private int _damage = 5;
	[SerializeField] private float _speed = 5;
	[SerializeField] private Texture2D _collisionSprite;
	private SpriteLookAt _spriteScript;
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

	public override void ExecuteCollision(Collider target)
	{
		Destroy(_spriteScript);
		_meshRenderer.material.mainTexture = _collisionSprite;

		if (target.CompareTag(_targetTag))
		{
			PlayerController.OnHit(_damage);	
		}

		Destroy(gameObject, 5f);
	}
}