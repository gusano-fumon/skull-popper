
using UnityEngine;

using DG.Tweening;


public class MeleeEnemy : Enemy
{
	[SerializeField] int _damage;

	private void Awake()
	{
		_distanceToTarget = 1;
		_deadZone = .1f;
		LevitateAnimation();
		enemyType = EnemyType.Melee;
	}

	protected override void Move()
	{
		if (_distanceToTarget + _deadZone > _remainingDistance && _remainingDistance > _distanceToTarget - _deadZone)
		{
			_state = EnemyState.Idle;
			_aiAgent.destination = transform.position;
			return;
		}

		_state = EnemyState.Walking;

		if (_remainingDistance > _distanceToTarget)
			_aiAgent.destination = _target.position;
		else
			_aiAgent.destination = transform.position + (transform.position - _target.position).normalized * (_distanceToTarget + 1);
	}

	private void LevitateAnimation()
	{
		_meshRenderer.transform.DOLocalMoveY(.5f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
	}

	protected override void Attack()
	{
		if (_remainingDistance > _distanceToTarget + 1.5f) return;

		// Melee Attack
		_meshRenderer.material.mainTexture = _attackSprite;
		_state = EnemyState.Attacking;

		PlayerController.OnHit(_damage);
	}

	protected override void Die()
	{
		_deadPS.Play();
		_aiAgent.baseOffset = .5f;
		_aiAgent.destination = transform.position;

		_meshRenderer.transform.DOKill();
		_meshRenderer.transform.position = new (transform.position.x, .5f, transform.position.z);
		base.Die();
	}
	
	protected override void SetWalkingSprite()
	{
		_meshRenderer.material.mainTexture = _walkingSprites[0];
	}
}