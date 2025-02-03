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
	public string StringFormat { get; set; }
    public SliderType SliderType { get; set; }

	public abstract void SaveChanges();

	public void Load()
	{
		UpdateValue(PlayerSettings.FieldOfView);
	}

	public void UpdateValue(float value)
	{
		valueText.text = value.ToString(StringFormat);
	}
}