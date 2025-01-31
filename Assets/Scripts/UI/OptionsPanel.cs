using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
	[SerializeField] private SliderBase[] sliders;

	public void RevertChanges()
	{
		foreach (var slider in sliders)
		{
			slider.ResetToDefault();
		}
	}
}