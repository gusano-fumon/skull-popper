
using UnityEngine;


public class AcidZone : MonoBehaviour
{
	[SerializeField] private int _damage = 1;
	[SerializeField] private float _damageDelayFrames = 5;

	private void OnTriggerStay(Collider other) 
	{
		if (other.TryGetComponent<ILife>(out var life))
		{
			if (Time.frameCount % _damageDelayFrames == 0)
			{
				if (!PlayerController.GameEnd)
					life.TakeDamage(_damage);
			}
		}
	}
}