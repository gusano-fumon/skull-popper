using System;

using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    public static Action OnVictory;

    private void OnTriggerEnter(Collider other)
    {
        OnVictory?.Invoke();
    }
}