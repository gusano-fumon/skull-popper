
using UnityEngine;


public class EnergyBall : Bullet
{
	[SerializeField] private float _speed = 5;
	[SerializeField] private Texture2D[] _sprites;
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
		base.ExecuteCollision(target);
		PlayerController.OnHit(1);
		Destroy(gameObject);
	}

	protected override void CheckSprite()
	{
		_spriteCounter++;
		_meshRenderer.sharedMaterial.mainTexture = _sprites[_spriteCounter % 2];
	}
}