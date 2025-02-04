public class FOVSlider : SliderBase
{
	private void Awake()
	{
		StringFormat = "000";
		SliderType = SliderType.FOV;
		OptionsPanel.OnResetToDefault += () => Load(PlayerSettings.FieldOfView);
	}

	private void OnDestroy()
	{
		OptionsPanel.OnResetToDefault -= () => Load(PlayerSettings.FieldOfView);
	}

	private void OnEnable()
	{
		Load(PlayerSettings.FieldOfView);
	}

	private void OnDisable()
	{
		SaveChanges();
	}

	public override void SaveChanges()
	{
		PlayerSettings.FieldOfView = slider.value;
	}
}