using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
	public void ProcessChange(SliderType type, float value)
	{
		PlayerPrefs.SetFloat(type.ToString(), value);
		PlayerPrefs.Save();

		switch (type)
		{
			case SliderType.Music:
				AudioFactory.Instance.SetMusicVolume(value);
				break;
			case SliderType.SFX:
				AudioFactory.Instance.SetSFXVolume(value);
				break;
			case SliderType.FOV:
				Debug.Log("FOV: " + value);
				Camera.main.fieldOfView = value;
				break;
			case SliderType.Sensitivity:
				// MouseLook.SetSensitivity(value);// AAAAAAAAAAAAAAAAAAAAAA
				break;
		}
	}
}