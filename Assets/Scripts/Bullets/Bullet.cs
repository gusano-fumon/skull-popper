
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class Bullet : Sprite
{
	private CancellationTokenSource _cts;
	private readonly int _timeToDelete = 5000;
	protected string _targetTag;

	protected virtual void Awake()
	{
		PlayerController.OnPlayerDeath += InstantDestroy;
		DeleteBullet().Forget();
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerDeath -= InstantDestroy;
	}

	private void OnTriggerEnter(Collider target)
	{
		_cts.Cancel();

		Debug.Log(target.name);
		if (target.CompareTag(_targetTag))
		{
			ExecuteCollision(target);
		}

		Destroy(gameObject);
	}

	public virtual void ExecuteCollision(Collider target) { }

	public async UniTaskVoid DeleteBullet()
	{
		_cts = new CancellationTokenSource();
		await UniTask.Delay(_timeToDelete, cancellationToken: _cts.Token);
		Destroy(gameObject);
	}

	private void InstantDestroy()
	{
		_cts.Cancel();
		Destroy(gameObject);
	}
}