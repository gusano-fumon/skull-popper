using UnityEngine;


public class Bubble : Bullet
{
	private Vector3 _forwardDirection, _sineDirection, _startPos;
	private float _forwardProgress, _sineProgress;
	[SerializeField] private float _forwardSpeed, _sineSpeed, _sineMultiplier;

	public void Init(Vector3 direction)
	{
		_startPos = transform.position;
		_targetTag = "Enemy";
		_forwardDirection = direction;
		_sineProgress = Random.Range(0, 10);

		_sineDirection = Vector3.Cross(Vector3.left, _forwardDirection);
	}

	protected override void Update()
	{
		base.Update();
		_forwardProgress += _forwardSpeed * Time.deltaTime;
		Vector3 position = _startPos + _forwardProgress * _forwardDirection;

		_sineProgress += _sineSpeed * Time.deltaTime;
		var sine = Mathf.Sin(_sineProgress) * _sineMultiplier;
		position += _sineDirection * sine;

		transform.position = position;
	}
}