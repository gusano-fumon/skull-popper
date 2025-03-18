using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;


public class SFXManager : IAudio<SFXManager>
{
    public AudioSource AudioSource { get; set; }
    public AudioMixerGroup MixerGroup { get; set; }

    public SFXManager(AudioSource audio, Transform parent)
	{
		AudioSource = audio;
		AudioSource.transform.SetParent(parent);
		MixerGroup = AudioSource.outputAudioMixerGroup;
	}

    public SFXManager Play(AudioType type, bool loop = false)
	{
		if (AudioLoader.Instance.TryGetClip(type, out var clip))
		{
			AudioSource.clip = clip;
			AudioSource.loop = loop;
			AudioSource.PlayOneShot(clip);
		}

		return this;
	}

	public SFXManager SetRandomPitch()
	{
		AudioSource.pitch = Random.Range(0.8f, 1.2f);
		return this;
	}

	public void PlayInPoint(AudioType type, Vector3 point)
	{
		if (AudioLoader.Instance.TryGetClip(type, out var clip))
		{
			AudioSource.PlayOneShot(clip);
		}
	}

	public SFXManager Destroy(float time)
	{
		MonoBehaviour.Destroy(AudioSource.gameObject, time);
		return this;
	}

	public void Stop()
	{
		AudioSource.DOFade(0f, .3f).SetUpdate(true).OnComplete(() => {
			AudioSource.Stop();
			Destroy(0f);
		});
	}
}