using UnityEngine;


public class FOVSlider : SliderBase
{
	[SerializeField] private float _defaultValue;

	private void Awake()
	{
		DefaultValue = _defaultValue;
		stringType = "000";
		SliderType = SliderType.Sensitivity;
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
		Camera.main.fieldOfView = CurrentValue;
	}

	public override void ResetToDefault()
	{
		base.ResetToDefault();
	}
}