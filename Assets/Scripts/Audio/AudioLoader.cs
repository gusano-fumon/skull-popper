using System.Collections.Generic;

using UnityEngine;


public class AudioLoader : Singleton<AudioLoader>
{

#region Fields
	
	private readonly List<AudioObject> _audioClips = new ();
	[SerializeField] private AudioSO _sfxSO;
	[SerializeField] private AudioSO _musicSO;

#endregion

#region Unity Methods

	protected override void Awake()
	{
		base.Awake();
		LoadAudio(_sfxSO);
		LoadAudio(_musicSO);
	}

#endregion

#region Methods

	private void LoadAudio(AudioSO data)
	{
		foreach (AudioObject audioObject in data.audioObjects)
		{
			if (_audioClips.Contains(audioObject))
			{
				Debug.LogWarning($"El audio {audioObject.type} ya se encuentra cargado.");
				continue;
			}

			_audioClips.Add(audioObject);
		}
	}

	public List<AudioObject> GetAudioClips()
	{
		return _audioClips;
	}

	public bool TryGetClip(AudioType clipName, out AudioClip clip)
	{
		foreach (AudioObject audioObject in _audioClips)
		{
			if (audioObject.type == clipName)
			{
				if (audioObject.multiple)
				{
					clip = audioObject.clips[UnityEngine.Random.Range(0, audioObject.clips.Count)];
				}
				else
				{
					clip = audioObject.Clip;
				}
				return true;
			}
		}

		Debug.LogWarning($"El clip {clipName} no se encuentra cargado.");
		clip = null;
		return false;
	}

#endregion

}