using UnityEngine;
using DG.Tweening;


public class Bubble : Bullet
{
	private Vector3 _forwardDirection, _startPos;
	private float _forwardProgress, _velocity;
	[SerializeField] private float _forwardSpeed, _sineSpeed, _sineMultiplier;
	[SerializeField] private Texture2D[] _sprites;

	private const float RANDOM = 0.4f;

	private float RandomY => Random.Range(-RANDOM, RANDOM);
	private float RandomX => Random.Range(-RANDOM, RANDOM);

	protected override void Awake()
	{
		base.Awake();
		_meshRenderer.transform.DOScale(0.2f, 5f).From(0);
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
		Vector3 position = _startPos + _forwardProgress * _forwardDirection * _velocity;

		transform.position = position;
	}

    public override void ExecuteCollision(Collider target)
    {
        base.ExecuteCollision(target);
		if (target.TryGetComponent<Enemy>(out var baseEnemy))
		{
			baseEnemy.TakeDamage(1);
		}
		Destroy(gameObject);
    }

	protected override void CheckSprite()
	{
		_spriteCounter++;
		_meshRenderer.sharedMaterial.mainTexture = _sprites[_spriteCounter % 2];
	}
}