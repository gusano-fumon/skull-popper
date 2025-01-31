using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFactory : Singleton<AudioFactory>
{

#region Fields

	[SerializeField] private AudioSource _sfxPrefab, _musicPrefab;
	[SerializeField] private AudioMixer _audioMixer;

#endregion

#region Unity Methods

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

#endregion

#region Methods

	public void PlayMusic(AudioType clipName, bool loop = true)
	{
		var music = Instantiate(_musicPrefab);
		new MusicManager(music)
			.Play(clipName, loop)
			.Destroy(5f);
	}

	// public void StopMusic()
	// {
	// 	_musicManager.Stop();
	// }

	public void PlaySFX(AudioType clipName)
	{
        var sfx = Instantiate(_sfxPrefab);
		new SFXManager(sfx, transform)
			.SetRandomPitch()
			.Play(clipName)
			.Destroy(5f);
	}

	public void PlaySFX(AudioType clipName, Transform point)
	{
		var sfx = Instantiate(_sfxPrefab, point);
		new SFXManager(sfx, point)
			.SetRandomPitch()
			.Play(clipName)
			.Destroy(5f);
	}

	public void SetMusicVolume(float value)
	{
		_musicPrefab.outputAudioMixerGroup.audioMixer
			.SetFloat("Music", Mathf.Log10(value) * 20);

		PlayerPrefs.SetFloat("Music", value);
		PlayerPrefs.Save();
	}

	public void SetSFXVolume(float value)
	{
		_sfxPrefab.outputAudioMixerGroup.audioMixer
			.SetFloat("SFX", Mathf.Log10(value) * 20);

		PlayerPrefs.SetFloat("SFX", value);
		PlayerPrefs.Save();
	}

	public void SetMasterVolume(float value)
	{
		PlayerPrefs.SetFloat("Master", value);
		_audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
		PlayerPrefs.Save();
	}

#endregion

}