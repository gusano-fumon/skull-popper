using System;
using UnityEngine;


public class OptionsPanel : MonoBehaviour
{
	public static Action OnResetToDefault;

	public void ResetToDefault()
	{
		PlayerSettings.SetDefault();
		OnResetToDefault?.Invoke();
	}
}