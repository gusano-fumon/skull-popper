using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatedArea : MonoBehaviour
{
    [SerializeField] private List<Animator> _openGates;
    [SerializeField] private List<Animator> _closeGates;
    [SerializeField] private int _enemiesToKill = 5;
    [SerializeField] private int _gatedAreaId = 0;
    private Image _image;
    private Sprite sprite;
    
    private void OnEnable()
    {
        Enemy.OnDeath += CheckEnemies;
    }

    private void OnDisable()
    {
        Enemy.OnDeath -= CheckEnemies;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            CloseArea();
            if (_gatedAreaId == -1) OpenArea();
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void OpenArea()
    {
        AudioController.OnOpenGate?.Invoke(transform);
        foreach (var gate in _openGates)
            gate.Play("Open");
    }

    private void CloseArea()
    {
        AudioController.OnOpenGate?.Invoke(transform);
        foreach (var gate in _closeGates)
            gate.Play("Close");
    }   

    private void CheckEnemies(int id)
    {
        if (id != _gatedAreaId) return;

        _enemiesToKill--;

        if (_enemiesToKill <= 0)
        {
            OpenArea();
        }
    }
}
