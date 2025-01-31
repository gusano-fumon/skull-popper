using System;
using UnityEngine;


public class SensitivitySlider : SliderBase
{
	[SerializeField] private float _defaultValue;
	public static event Action<float> OnValueChanged;

	private void Awake()
	{
		DefaultValue = _defaultValue;
		stringType = "0.000";
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
		OnValueChanged?.Invoke(CurrentValue);
	}

	public override void ResetToDefault()
	{
		base.ResetToDefault();
	}
}