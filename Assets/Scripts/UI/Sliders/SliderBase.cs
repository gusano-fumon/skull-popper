using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all sliders this class is responsible for handling the slider value and invoking the events
/// </summary>
public abstract class SliderBase : MonoBehaviour, ISlider
{
	public Slider slider;
	public TMP_Text valueText;
	[HideInInspector] public string stringType;
	public float DefaultValue { get; set; }
	public float CurrentValue { get; set; }
    public SliderType SliderType { get; set; }

	private float SavedValue 
	{ 
		get
		{
			return PlayerPrefs.GetFloat($"{SliderType}", DefaultValue);
		}
		set
		{
			PlayerPrefs.SetFloat($"{SliderType}", value);
			PlayerPrefs.Save();
		}
	}

	public virtual void SaveChanges()
	{
		SavedValue = CurrentValue;
	}

	public void Load()
	{
		CurrentValue = SavedValue;
		SetValues();
	}

	public void UpdateValue(float value)
	{
		CurrentValue = value;
		valueText.text = value.ToString(stringType);
	}

	public virtual void ResetToDefault()
	{
		CurrentValue = DefaultValue;
		SetValues();
	}

	private void SetValues()
	{
		slider.value = CurrentValue;
		valueText.text = CurrentValue.ToString(stringType);
	}
}