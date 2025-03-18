
using System;

using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;


public enum EnemyState {
	Idle,
	Walking,
	Attacking,
	ReceivingDamage
}

public enum EnemyType {
	Melee,
	Shooter,
}

public class Enemy : MonoBehaviour, ILife
{
	[SerializeField] protected NavMeshAgent _aiAgent;
	[SerializeField] protected float _distanceToTarget;
	[SerializeField] protected float _deadZone;

	public static Action<GatedAreaEnum> OnDeath;
	[SerializeField] private GatedAreaEnum _gatedAreaId;
	

	protected EnemyType enemyType;
	private AudioType HitEnemyType => enemyType == EnemyType.Melee ? AudioType.MeleeEnemyDie : AudioType.ShooterEnemyDie;
	private AudioType DieEnemyType => enemyType == EnemyType.Melee ? AudioType.MeleeEnemyDie : AudioType.ShooterEnemyDie;

	public int totalHealth = 5;
	public int Health { get; set; }

	[SerializeField] protected MeshRenderer _meshRenderer;
	[SerializeField] protected Texture2D[] _walkingSprites;
	[SerializeField] protected Texture2D _attackSprite;
	[SerializeField] protected ParticleSystem _deadPS;
	[SerializeField] private Texture2D _deadSprite;
	[SerializeField] private Texture2D _hurtSprite;
	[SerializeField] private Texture2D _idleSprite;

	protected Transform _target;
	protected EnemyState _state;
	protected float _remainingDistance;
	public bool isDead = false;
	private bool _alredySeen = false;

	private void Start()
	{
		Health = totalHealth;
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_aiAgent.destination = transform.position;
	}

	protected void Update()
	{
		if (PlayerController.GameEnd || isDead) return;

		if (Time.frameCount % 40 == 0) CheckSprite();

		var horizontalDistance = new Vector3(_target.position.x - transform.position.x, 0, _target.position.z - transform.position.z);
		_remainingDistance = horizontalDistance.magnitude;

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
				await UniTask.Delay(UnityEngine.Random.Range(300, 600));
				if (isDead) return;
			}

			if (Time.frameCount % 3 == 0) Move();

			if (Time.frameCount % 50 == 0)
			{
				var ray = new Ray(transform.position, _target.position - transform.position);
				if (Physics.Raycast(ray, out var hit))
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
		isDead = true;
		OnDeath?.Invoke(_gatedAreaId);
		_meshRenderer.material.mainTexture = _deadSprite;

		_meshRenderer.material.color = new (0.6f, 0.6f, 0.6f, 1); // Gray color for die enemy sprite
		Destroy(GetComponent<CapsuleCollider>());
		Destroy(_aiAgent);
		Destroy(this);
	}

	public void TakeDamage(int damage)
	{
		if (isDead) return;

		Health -= damage;
		if (Health <= 0)
		{
			Die();
			AudioFactory.Instance.PlaySFX(DieEnemyType);
			return;
		}

		AudioFactory.Instance.PlaySFX(HitEnemyType);
		_state = EnemyState.ReceivingDamage;
		_meshRenderer.material.mainTexture = _hurtSprite;
	}

	public void RestoreHealth(int health)
	{
		Health += health;
	}

	protected int _spriteCounter = 0;
	private void CheckSprite()
	{
		if (isDead) return;

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