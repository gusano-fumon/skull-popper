
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class Bullet : SpriteAnimation
{
	protected string _targetTag;

	private CancellationTokenSource _cts;
	private readonly int _timeToDelete = 5000;

	protected virtual void Awake()
	{
		GameMenu.OnPlayerDeath += InstantDestroy;
		DeleteBullet().Forget();
	}

	private void OnDestroy()
	{
		GameMenu.OnPlayerDeath -= InstantDestroy;
	}

	private void OnTriggerEnter(Collider target)
	{
		_cts.Cancel();
		ExecuteCollision(target);
	}

	public virtual void ExecuteCollision(Collider target) { }

	public async UniTaskVoid DeleteBullet()
	{
		_cts = new CancellationTokenSource();
		await UniTask.Delay(_timeToDelete, cancellationToken: _cts.Token);
		Destroy(gameObject, .5f);
	}
	private void InstantDestroy()
	{
		_cts.Cancel();
		Destroy(gameObject);
	}
}