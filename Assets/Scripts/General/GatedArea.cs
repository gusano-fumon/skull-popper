using System;
using System.Collections.Generic;

using UnityEngine;


public class GatedArea : MonoBehaviour
{
	[SerializeField] private List<Animator> _openGates;
	[SerializeField] private List<Animator> _closeGates;
	[SerializeField] private int _enemiesToKill = 5;
	[SerializeField] private GatedAreaEnum _gatedAreaId;

	public static Action OnTutorialComplete;
	
	private void OnEnable()
	{
		Enemy.OnDeath += CheckEnemies;
		if (_gatedAreaId.AreaId == GatedAreaEnum.Tutorial) MouseLook.OnReload += CheckTutorial;
	}

	private void OnDisable()
	{
		Enemy.OnDeath -= CheckEnemies;
		if (_gatedAreaId.AreaId == GatedAreaEnum.Tutorial) MouseLook.OnReload += CheckTutorial;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerController>() != null)
		{
			CloseArea();
			if (_gatedAreaId.AreaId == GatedAreaEnum.None) OpenArea();
			gameObject.GetComponent<Collider>().enabled = false;
		}
	}

	private void OpenArea()
	{
		AudioFactory.Instance.PlaySFX(AudioType.Gate, transform);
		ScoreController.AddScore(100);

		foreach (var gate in _openGates)
			gate.Play("Open");
	}

	private void CloseArea()
	{
		AudioFactory.Instance.PlaySFX(AudioType.Gate, transform);
		foreach (var gate in _closeGates)
			gate.Play("Close");
	}

	private void CheckTutorial(int magazine)
	{
		if (_enemiesToKill <= 0 && _gatedAreaId.AreaId == GatedAreaEnum.Tutorial && magazine == 12)
		{
			OpenArea();
			OnTutorialComplete?.Invoke();
		}
	}   

	private void CheckEnemies(GatedAreaEnum id)
	{
		if (id.AreaId != _gatedAreaId.AreaId) return;

		_enemiesToKill--;

		if (_enemiesToKill <= 0 && _gatedAreaId.AreaId != GatedAreaEnum.Tutorial)
		{
			OpenArea();
		}
	}
}