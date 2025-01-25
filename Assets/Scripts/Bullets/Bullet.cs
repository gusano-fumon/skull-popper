
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class Bullet : MonoBehaviour
{
	private CancellationTokenSource  _cts;
	private readonly int _timeToDelete = 5000;
	protected string _targetTag;
	
	private void Awake()
	{
		DeleteBullet().Forget();
	}

	private void OnCollisionEnter(Collision target)
	{
		if (target.collider.CompareTag(_targetTag))
		{
			_cts.Cancel();

			ExecuteCollision();
			Destroy(gameObject);
		}
	}

	public virtual void ExecuteCollision()
	{
		// Player receives damage.
		Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
	}

	public async UniTaskVoid DeleteBullet()
	{
		_cts = new CancellationTokenSource();
		await UniTask.Delay(_timeToDelete, cancellationToken: _cts.Token);
		Destroy(gameObject);
	}
}