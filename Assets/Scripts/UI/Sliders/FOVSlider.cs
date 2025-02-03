using UnityEngine;


public class FOVSlider : SliderBase
{
	private void Awake()
	{
		StringFormat = "000";
		SliderType = SliderType.FOV;
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
		PlayerSettings.FieldOfView = slider.value;
	}
}