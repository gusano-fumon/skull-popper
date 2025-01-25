
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class EnergyBall : MonoBehaviour
{
	[SerializeField] private float _speed = 5;
	private CancellationTokenSource  _cts;
	private Vector3 _direction;

	private void FixedUpdate()
	{
		Move();
	}

	private void OnCollisionEnter(Collision target)
	{
		if (target.collider.CompareTag("Player"))
		{
			_cts.Cancel();

			// Player receives damage.
			Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Destroy(gameObject);
		}
	}

	public void Init(Vector3 target)
	{
		_cts = new CancellationTokenSource();
		DeleteBullet().Forget();
		transform.LookAt(target);
		_direction = (target - transform.position).normalized;
	}

	private void Move()
	{
		transform.localPosition += _speed * Time.deltaTime * _direction;
	}

	private async UniTaskVoid DeleteBullet()
	{
		await UniTask.Delay(3000, cancellationToken: _cts.Token);
		Destroy(gameObject);
	}
}