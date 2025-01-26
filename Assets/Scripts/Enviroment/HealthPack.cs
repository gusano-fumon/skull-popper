
using UnityEngine;


public class HealthPack : MonoBehaviour
{
	[SerializeField] private int _health = 10; 

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<PlayerController>(out var player))
		{
			if (player.Health == player.TotalHealth) return;

			player.RestoreHealth(_health);
			Destroy(gameObject);
		}
	}
}