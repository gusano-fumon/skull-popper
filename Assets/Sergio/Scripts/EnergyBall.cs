
using UnityEngine;

using Cysharp.Threading.Tasks;


public class EnergyBall : MonoBehaviour
{
	[SerializeField] private Rigidbody _rb;

	private void OnCollisionEnter(Collision target)
	{
		if (target.collider.CompareTag("Player"))
		{
			// Player receives damage.
			Destroy(this);
		}
	}

	public void Init(Vector3 target)
	{
		DeleteBullet().Forget();
		_rb.MovePosition(target);
	}

	private async UniTaskVoid DeleteBullet()
	{
		await UniTask.Delay(3000);
		Destroy(this);
	}
}