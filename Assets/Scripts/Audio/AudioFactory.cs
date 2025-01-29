using UnityEngine;
using UnityEngine.Audio;

public class AudioFactory : Singleton<AudioFactory>
{

#region Fields

	private MusicManager _musicManager;
	private SFXManager _sfxManager;

	[SerializeField] private AudioSource _sfxPrefab, _musicPrefab;
	[SerializeField] private AudioMixer _audioMixer;

#endregion

#region Properties

	public MusicManager Music
	{
		get
		{
			_musicManager ??= new MusicManager(_musicPrefab);
			return _musicManager;
		}
		set => _musicManager = value;
	}

	public SFXManager SFX
	{
		get
		{
			_sfxManager ??= new SFXManager(_sfxPrefab);
			return _sfxManager;
		}
		set => _sfxManager = value;
	}

#endregion

#region Unity Methods

	protected override void Awake()
	{
		base.Awake();
		_musicManager = new MusicManager(_musicPrefab);
		_sfxManager = new SFXManager(_sfxPrefab);
	}

#endregion

#region Methods

	public void PlayMusic(AudioType clipName, bool loop = true)
	{
		Music.Play(clipName, loop);
	}

	public void StopMusic()
	{
		Music.Stop();
	}

	public void PlaySFX(AudioType clipName)
	{
		SFX.Play(clipName);
	}

	public void SetMusicVolume(float volume)
	{
		Music.SetVolume(volume);
	}

	public void SetSFXVolume(float volume)
	{
		SFX.SetVolume(volume);
	}

#endregion

}