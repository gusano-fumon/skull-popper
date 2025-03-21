
public class SensitivitySlider : SliderBase
{
	private void Awake()
	{
		StringFormat = "000";
		SliderType = SliderType.Sensitivity;
		OptionsPanel.OnResetToDefault += () => Load(PlayerSettings.Sensitivity);
	}

	private void OnDestroy()
	{
		OptionsPanel.OnResetToDefault -= () => Load(PlayerSettings.Sensitivity);
	}

	private void OnEnable()
	{
		Load(PlayerSettings.Sensitivity);
	}

	private void OnDisable()
	{
		SaveChanges();
	}

	public override void SaveChanges()
	{
		PlayerSettings.Sensitivity = slider.value;
	}
}