
using DG.Tweening;

public class TutorialDummy : Enemy
{
    protected override void Move()
	{
        _state = EnemyState.Idle;
    }

    protected override void Die()
        {
            ScoreController.AddScore(10);

            _deadPS.Play();
            _aiAgent.baseOffset = .5f;
            _aiAgent.destination = transform.position;

            _meshRenderer.transform.DOKill();
            _meshRenderer.transform.position = new (transform.position.x, .5f, transform.position.z);
            base.Die();
        }
}
