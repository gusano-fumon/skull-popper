using Cysharp.Threading.Tasks;
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
			FadeInMusic(1f);
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

	public void Stop(float time)
	{
		FadeOut(time).Forget();
	}

	public void FadeInMusic(float duration)
	{
		AudioSource.volume = 0;
		AudioSource.Play();
		FadeIn(duration).Forget();
	}

	private async UniTaskVoid FadeIn(float duration)
	{
		float startVolume = AudioSource.volume;
		float elapsedTime = 0f;

		while (AudioSource.volume < volume)
		{
			elapsedTime += Time.deltaTime;
			AudioSource.volume = Mathf.Lerp(startVolume, volume, elapsedTime / duration);
			await UniTask.Yield();
		}
	}

	public void FadeOutMusic(float duration)
	{
		FadeOut(duration).Forget();
	}

	private async UniTaskVoid FadeOut(float duration)
	{
		float startVolume = AudioSource.volume;
		float elapsedTime = 0f;

		while (AudioSource.volume > 0)
		{
			elapsedTime += Time.deltaTime;
			AudioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / duration);
			await UniTask.Yield();
		}

		AudioSource.Stop();
		Destroy(0f);
	}
}