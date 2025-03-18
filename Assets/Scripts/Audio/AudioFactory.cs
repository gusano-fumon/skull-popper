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
		SetVolume();
		PlayerSettings.OnMusicChanged += SetMusicVolume;
		PlayerSettings.OnSfxChanged += SetSFXVolume;
	}

	public void OnDestroy()
	{
		PlayerSettings.OnMusicChanged -= SetMusicVolume;
		PlayerSettings.OnSfxChanged -= SetSFXVolume;
	}

#endregion

#region Methods

	public MusicManager musicManager;
	public SFXManager sfxManager;

	public void PlayMusic(AudioType clipName, bool loop = true)
	{
		var music = Instantiate(_musicPrefab);
		musicManager = new MusicManager(music)
			.Play(clipName, loop);
	}

	public void StopMusic()
	{
		musicManager.FadeOutMusic(.3f);
	}

	public void StopSFX()
	{
		sfxManager.Stop();
	}

	public void PlaySFX(AudioType clipName)
	{
        var sfx = Instantiate(_sfxPrefab);
		sfxManager = new SFXManager(sfx, transform)
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

	private void SetVolume()
	{
		SetMusicVolume(PlayerSettings.MusicVolume);
		SetSFXVolume(PlayerSettings.SFXVolume);
	}

	public void SetMusicVolume(float value)
	{
		_audioMixer
			.SetFloat("Music", Mathf.Log10(value) * 20);
	}

	public void SetSFXVolume(float value)
	{
		_audioMixer
			.SetFloat("SFX", Mathf.Log10(value) * 20);
	}

#endregion

}