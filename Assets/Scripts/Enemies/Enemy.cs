
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;


public enum EnemyState {
	Idle,
	Walking,
	Attacking,
	ReceivingDamage
}

public class Enemy : Sprite, ILife
{
	[SerializeField] protected NavMeshAgent _aiAgent;
	[SerializeField] protected float _distanceToTarget;
	[SerializeField] protected float _deadZone;

	public int totalHealth = 5;
	public int Health { get; set; }
	[SerializeField] protected Texture2D[] _walkingSprites;
	[SerializeField] protected Texture2D _attackSprite;
	[SerializeField] private Texture2D _deadSprite;
	[SerializeField] private Texture2D _hurtSprite;
	[SerializeField] private Texture2D _idleSprite;

	protected Transform _target;
	protected EnemyState _state;
	protected float _remainingDistance;
	protected bool _isDead = false;
	private bool _alredySeen = false;

	private void Start()
	{
		Health = totalHealth;
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_aiAgent.destination = transform.position;
	}

	protected override void Update()
	{
		base.Update();

		if (PlayerController.GameEnd || _isDead) return;

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
				if (_isDead) return;
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
	protected virtual void Die()
	{
		_isDead = true;
		_meshRenderer.material.mainTexture = _deadSprite;
	}

	public void TakeDamage(int damage)
	{
		if (_isDead) return;

		Health -= damage;
		if (Health <= 0)
		{
			Die();
			return;
		}

		_state = EnemyState.ReceivingDamage;
		_meshRenderer.material.mainTexture = _hurtSprite;
	}

	public void RestoreHealth(int health)
	{
		Health += health;
	}

	protected override void CheckSprite()
	{
		if (_isDead) return;

		if (_state == EnemyState.Idle)
		{
			_meshRenderer.material.mainTexture = _idleSprite;
			return;
		}

		_spriteCounter++;
		SetWalkingSprite();
	}

	protected virtual void SetWalkingSprite() { }
}