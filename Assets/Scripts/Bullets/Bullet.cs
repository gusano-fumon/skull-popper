
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class Bullet : Sprite
{
	private CancellationTokenSource _cts;
	private readonly int _timeToDelete = 5000;
	protected string _targetTag;

	private void Awake()
	{
		DeleteBullet().Forget();
	}

	private void OnTriggerEnter(Collider target)
	{
		if (target.CompareTag(_targetTag))
		{
			_cts.Cancel();
			ExecuteCollision();
		}
	}

	public virtual void ExecuteCollision()
	{
		// Player receives damage.
		Destroy(gameObject);
	}

	public async UniTaskVoid DeleteBullet()
	{
		_cts = new CancellationTokenSource();
		await UniTask.Delay(_timeToDelete, cancellationToken: _cts.Token);
		Destroy(gameObject);
	}
}