using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    public static Action OnVictory;

    void OnTriggerEnter(Collider other)
    {
        OnVictory?.Invoke();
    }
}
