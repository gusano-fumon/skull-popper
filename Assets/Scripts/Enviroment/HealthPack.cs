using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private int _health = 5; 

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            var player = other.GetComponent<PlayerController>();
            if (player.Health == player.TotalHealth) return;

            player.RestoreHealth(_health);
            Destroy(gameObject);
        }
    }
}
