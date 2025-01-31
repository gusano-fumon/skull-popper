using UnityEngine;


public class SFXSlider : SliderBase
{
	[SerializeField] private int _defaultValue;

	private void Awake()
	{
		DefaultValue = _defaultValue;
		stringType = "000";
		SliderType = SliderType.SFX;
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
		AudioFactory.Instance.SetSFXVolume(CurrentValue);
	}

	public override void ResetToDefault()
	{
		base.ResetToDefault();
	}
}