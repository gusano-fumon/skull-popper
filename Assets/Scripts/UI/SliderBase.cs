using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all sliders this class is responsible for handling the slider value and invoking the events
/// </summary>
[RequireComponent(typeof(Slider))]
public abstract class SliderBase : MonoBehaviour, ISlider
{
	[SerializeField] private Slider _slider;
	[SerializeField] private Slider _minValue;
	[SerializeField] private Slider _maxValue;
	[SerializeField] private TMP_Text _nameText, _valueText;
	[SerializeField] private SliderType _sliderType;
	[SerializeField] private float _defaultValue = 50;
	public static event Action<SliderType, float> OnSliderChanged;

	public void Apply()
	{
		_valueText.text = _slider.value.ToString("000");
		OnSliderChanged?.Invoke(_sliderType, _slider.value);
	}

	public void Load()
	{
		_slider.value = PlayerPrefs.GetFloat(_sliderType.ToString(), 50);
		_valueText.text = _slider.value.ToString();
	}

	public void ResetToDefault()
	{
		_slider.value = _defaultValue;
		PlayerPrefs.SetFloat(_sliderType.ToString(), _defaultValue);
		PlayerPrefs.Save();	
	}
}