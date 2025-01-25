using System.Collections;

using UnityEngine;

public class AcidZone : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _damageDelayFrames = 5;

    public Coroutine _cr;

    private void OnTriggerStay(Collider other) 
    {
        if (other.GetComponent<ILife>() != null)
        {
            if (Time.frameCount % _damageDelayFrames == 0)
            {
                other.GetComponent<ILife>().TakeDamage(_damage);
            }
        }
    }
}
