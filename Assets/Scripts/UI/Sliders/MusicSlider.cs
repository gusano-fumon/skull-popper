
public class MusicSlider : SliderBase
{
	private void Awake()
	{
		StringFormat = "000";
		SliderType = SliderType.Music;
		OptionsPanel.OnResetToDefault += () => Load(PlayerSettings.MusicVolume);
	}

	private void OnDestroy()
	{
		OptionsPanel.OnResetToDefault -= () => Load(PlayerSettings.MusicVolume);
	}

	private void OnEnable()
	{
		Load(PlayerSettings.MusicVolume);
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
		PlayerSettings.MusicVolume = slider.value / 100;
	}
}