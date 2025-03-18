using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : IAudio<MusicManager>
{
	private AudioClip audioClip;

	[Range(0f, 1f)] public float volume = 1f;

	public AudioSource AudioSource { get; set; }
	public AudioMixerGroup MixerGroup { get; set; }

	public MusicManager(AudioSource prefab)
	{
		AudioSource = prefab;
		MixerGroup = AudioSource.outputAudioMixerGroup;
	}

	public MusicManager Play(AudioType type, bool loop = true)
	{
		if (AudioLoader.Instance.TryGetClip(type, out var clip))
		{
			audioClip = clip;
			AudioSource.clip = audioClip;
			AudioSource.loop = loop;
			FadeInMusic(.5f);
		}

		return this;
	}

	public MusicManager Destroy(float time)
	{
		MonoBehaviour.Destroy(AudioSource.gameObject, time);
		return this;
	}

	public void Stop()
	{
		AudioSource.Stop();
	}

	public void Stop(float duration)
	{
		AudioSource
			.DOFade(0, duration)
			.From(AudioSource.volume)
			.OnComplete(() => {
				AudioSource.Stop();
				Destroy(0f);
			});
	}

	public void FadeInMusic(float duration)
	{
		AudioSource.volume = 0;
		AudioSource.Play();
		AudioSource.DOFade(volume, duration);
	}

	public void FadeOutMusic(float duration)
	{
		AudioSource.DOFade(0f, duration).SetUpdate(true).OnComplete(() => {
			AudioSource.Stop();
			Destroy(0f);
		});
	}
}