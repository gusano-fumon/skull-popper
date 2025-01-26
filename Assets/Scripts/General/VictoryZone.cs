using UnityEngine;


public class VictoryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       GameMenu.OnVictory?.Invoke();
    }
}