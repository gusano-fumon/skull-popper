using UnityEngine;


public class SFXSlider : SliderBase
{
	private void Awake()
	{
		StringFormat = "000";
		SliderType = SliderType.SFX;
		OptionsPanel.OnResetToDefault += () => Load(PlayerSettings.SFXVolume);
	}

	private void OnDestroy()
	{
		OptionsPanel.OnResetToDefault -= () => Load(PlayerSettings.SFXVolume);
	}

	private void OnEnable()
	{
		Load(PlayerSettings.SFXVolume);
	}

	private void OnDisable()
	{
		SaveChanges();
	}

	public override void Load(float value)
	{
		slider.value = value * 100;
		UpdateValue(slider.value);
	}

    public override void UpdateValue(float value)
    {
		if (value == 1)
		{
			valueText.SetText("OFF");
			return;
		}

		valueText.SetText(value.ToString(StringFormat));
    }


	public override void SaveChanges()
	{
		Debug.Log("SFXSlider: SaveChanges: slider.value = " + slider.value);
		PlayerSettings.SFXVolume = slider.value / 100;
	}
}