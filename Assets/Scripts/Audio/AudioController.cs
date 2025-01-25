using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class AudioController : MonoBehaviour
{
	public List<AudioClip> hitClips;
	public List<AudioClip> shotClips;
	public AudioSource _prefab;

	private bool _onHitSound = false;

	public void PlayHitClip(Transform transform)
	{
		if (_onHitSound) return;

		_onHitSound = true;
	
		var audio = Instantiate(_prefab, transform);
		audio.clip = hitClips[Random.Range(0, hitClips.Count)];
		audio.Play();
		Destroy(audio.gameObject, 5f);

		UniTask.Delay(1000).ContinueWith(() => _onHitSound = false);
	}

	public void PlayShotClip(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.clip = shotClips[Random.Range(0, shotClips.Count)];
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}
}