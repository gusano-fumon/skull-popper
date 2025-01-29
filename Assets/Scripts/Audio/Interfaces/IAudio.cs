
using UnityEngine;
using UnityEngine.Audio;


public interface IAudio
{

#region Properties

	public AudioSource AudioSource { get; set; }
	public AudioMixerGroup MixerGroup { get; set; }
	public float Volume { get; set; }

#endregion

#region Methods

	public void Play(AudioType type, bool loop = false);
	public void Stop();
	public void SetVolume(float volume);

#endregion

}