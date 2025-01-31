using UnityEngine;


public class MasterSlider : SliderBase
{
	[SerializeField] private float _defaultValue;

	private void Awake()
	{
		DefaultValue = _defaultValue;
		stringType = "000";
		SliderType = SliderType.Master;
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
		AudioFactory.Instance.SetMasterVolume(CurrentValue);
	}

	public override void ResetToDefault()
	{
		base.ResetToDefault();
	}
}