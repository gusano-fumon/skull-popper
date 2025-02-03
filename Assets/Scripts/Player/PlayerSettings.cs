
using System;
using UnityEngine;


public static class PlayerSettings
{
	public const float Sensitivity_Default = 100f;
	public const float FOV_Default = 50f;
	public const float Music_Default = 100f;
	public const float SFX_Default = 100f; 
	public static Action<float> OnSensitiveChanged;
	public static Action<float> OnFovChanged;
	public static Action<float> OnMusicChanged;
	public static Action<float> OnSfxChanged;
	
	public static float Sensitivity
	{
		get => PlayerPrefs.GetFloat("Sensitivity", Sensitivity_Default);
		set
		{
			OnSensitiveChanged?.Invoke(value);
			PlayerPrefs.SetFloat("Sensitivity", value);
			PlayerPrefs.Save();
		} 
	}

	public static float FieldOfView 
	{
		get => PlayerPrefs.GetFloat("FOV", FOV_Default);
		set
		{
			OnFovChanged?.Invoke(value);
			PlayerPrefs.SetFloat("FOV", value);
			PlayerPrefs.Save();
		}
	}
	public static float MusicVolume
	{
		get => PlayerPrefs.GetFloat("Music", Music_Default);
		set
		{
			OnMusicChanged?.Invoke(value);
			PlayerPrefs.SetFloat("Music", value);
			PlayerPrefs.Save();
		}
	}

	public static float SFXVolume
	{
		get => PlayerPrefs.GetFloat("SFX", SFX_Default);
		set
		{
			OnSfxChanged?.Invoke(value);
			PlayerPrefs.SetFloat("SFX", value);
			PlayerPrefs.Save();
		}
	}

	public static void SetDefault()
	{
		PlayerPrefs.SetFloat("Sensitivity", Sensitivity_Default);
		PlayerPrefs.SetFloat("FOV", FOV_Default);
		PlayerPrefs.SetFloat("Music", Music_Default);
		PlayerPrefs.SetFloat("SFX", SFX_Default);
		PlayerPrefs.Save();

		OnSfxChanged?.Invoke(SFX_Default);
		OnMusicChanged?.Invoke(Music_Default);
		OnFovChanged?.Invoke(FOV_Default);
		OnSensitiveChanged?.Invoke(Sensitivity_Default);
	}
}
