using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : SliderBase
{
	[SerializeField] private float _defaultValue;

	private void Awake()
	{
		DefaultValue = _defaultValue;
		stringType = "000";
		SliderType = SliderType.Music;
		Load();
	}

	private void OnEnable()
	{
		Load();
	}

	private void OnDisable()
	{
		SaveChanges();
	}

	public override void SaveChanges()
	{
		base.SaveChanges();
		AudioFactory.Instance.SetMusicVolume(CurrentValue);
	}

	public override void ResetToDefault()
	{
		base.ResetToDefault();
	}
}