
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class Bullet : MonoBehaviour
{
	[SerializeField] protected MeshRenderer _meshRenderer;
	[SerializeField] private Texture2D[] _sprites;
	protected string _targetTag;

	private CancellationTokenSource _cts;
	private readonly int _timeToDelete = 5000;
	private int _spriteCounter = 0;

	protected virtual void Awake()
	{
		PlayerController.OnPlayerDeath += InstantDestroy;
		DeleteBullet().Forget();
	}

	protected virtual void Update()
	{
		if (Time.frameCount % 40 == 0) CheckSprite();
	}

	private void OnDestroy()
	{
		PlayerController.OnPlayerDeath -= InstantDestroy;
	}

	private void OnTriggerEnter(Collider target)
	{
		_cts.Cancel();

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

	private void CheckSprite()
	{
		_spriteCounter++;
		_meshRenderer.sharedMaterial.mainTexture = _sprites[_spriteCounter % 2];
	}

	private void InstantDestroy()
	{
		_cts.Cancel();
		Destroy(gameObject);
	}
}