using UnityEngine;


public class SFXSlider : SliderBase
{
	private void Awake()
	{
		StringFormat = "000";
		SliderType = SliderType.SFX;
		OptionsPanel.OnResetToDefault += Load;
	}

	private void OnDestroy()
	{
		OptionsPanel.OnResetToDefault -= Load;
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
		PlayerSettings.SFXVolume = slider.value;
	}
}