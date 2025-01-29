using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : IAudio
{

#region Properties

    public AudioSource AudioSource { get; set; }
    public AudioMixerGroup MixerGroup { get; set; }
    public float Volume { get; set; }

#endregion

#region Fields

    public SFXManager(AudioSource prefab)
	{
		AudioSource = prefab;
		MixerGroup = AudioSource.outputAudioMixerGroup;
	}

    public void Play(AudioType type, bool loop = false)
	{
		if (AudioLoader.Instance.TryGetClip(type, out var clip))
		{
			AudioSource.clip = clip;
			AudioSource.loop = loop;
			AudioSource.volume = Volume;
			AudioSource.Play();
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
		AudioSource.volume = startVolume; // Restaurar el volumen original
	}

    public void Stop()
    {
        throw new System.NotImplementedException();
    }

    public void SetVolume(float volume)
    {
		;
    }

    #endregion

}