using UnityEngine;
using DG.Tweening;


public class Bubble : Bullet
{
	private Vector3 _forwardDirection, _startPos;
	private float _forwardProgress;
	[SerializeField] private float _forwardSpeed, _sineSpeed, _sineMultiplier;

	private const float RANDOM = 0.4f;

	private float RandomY => Random.Range(-RANDOM, RANDOM);
	private float RandomX => Random.Range(-RANDOM, RANDOM);

	protected override void Awake()
	{
		base.Awake();
		_meshRenderer.transform.DOScale(.2f, 6f).From(0);
		_meshRenderer.transform.DOLocalMoveY(RandomY, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		_meshRenderer.transform.DOLocalMoveX(RandomX, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
	}

	private void OnDestroy()
	{
		_meshRenderer.transform.DOKill();
	}

	public void Init(Vector3 direction)
	{
		_startPos = transform.position;
		_targetTag = "Enemy";
		_forwardDirection = direction;
	}

	protected override void Update()
	{
		base.Update();

		_forwardProgress += _forwardSpeed * Time.deltaTime;
		Vector3 position = _startPos + _forwardProgress * _forwardDirection;

		transform.position = position;
	}

    public override void ExecuteCollision(Collider target)
    {
        base.ExecuteCollision(target);
		if (target.TryGetComponent<Enemy>(out var baseEnemy))
		{
			baseEnemy.TakeDamage(1);
		}
    }
}