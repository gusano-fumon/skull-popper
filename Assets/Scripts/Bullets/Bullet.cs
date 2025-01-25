
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
		Debug.Log(gameObject.name);
	}

	void OnDestroy()
	{
		PlayerController.OnPlayerDeath -= InstantDestroy;
	}

	private void OnTriggerEnter(Collider target)
	{
		Debug.Log("tag: " + target.tag);
		Debug.Log("targettag: " + _targetTag);
		
		if (target.CompareTag(_targetTag))
		{
			_cts.Cancel();
			ExecuteCollision(target);
		}
	}

	public virtual void ExecuteCollision(Collider target)
	{
		// Player receives damage.
	}

	public async UniTaskVoid DeleteBullet()
	{
		_cts = new CancellationTokenSource();
		await UniTask.Delay(_timeToDelete, cancellationToken: _cts.Token);
		Destroy(gameObject);
	}

	private void InstantDestroy()
	{
		Debug.Log("Bullet destroyed");
		_cts.Cancel();
		Destroy(gameObject);
	}
}