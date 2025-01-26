using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;


public class AudioController : MonoBehaviour
{
	public List<AudioClip> hitClips;
	public AudioClip shotClip;
	public List<AudioClip> enemyHitClips;
	public List<AudioClip> enemyDieClips;
	public AudioClip recargaClip;
	public AudioClip jumpClip;
	public AudioClip pocionClip;
	public AudioClip popBubbleClip;
	public AudioClip musicClip;
	public AudioClip musicClip2;
	public AudioClip openGateClip;
	public AudioSource _prefab;

	private bool _onHitSound = false;

	public static Action<Transform> OnHitSound;
	public static Action<Transform> OnShotSound;
	public static Action<EnemyType, Transform> OnEnemyHitSound;
	public static Action<EnemyType, Transform> OnEnemyDieSound;
	public static Action<Transform> OnPopBubbleSound;
	public static Action<Transform> OnRecargaSound;
	public static Action<Transform> OnOpenGate;


	public void Awake()
	{
		OnShotSound += PlayShotClip;
		OnEnemyHitSound += PlayEnemyHitClip;
		OnPopBubbleSound += PlayPopBubbleClip;
		OnEnemyDieSound += PlayEnemyDie;
		OnOpenGate += PlayOpenGate;

		PlayMusic();
	}

	public void OnDestroy()
	{
		OnShotSound -= PlayShotClip;
		OnEnemyHitSound -= PlayEnemyHitClip;
		OnPopBubbleSound -= PlayPopBubbleClip;
		OnEnemyDieSound -= PlayEnemyDie;
		OnOpenGate -= PlayOpenGate;
	}

	public void PlayMusic()
	{
		// var audio = Instantiate(_prefab);
		// audio.clip = musicClip;
		// audio.Play();
		// audio.loop = true;

		var audio2 = Instantiate(_prefab);
		audio2.volume = 0.5f;
		audio2.clip = musicClip2;
		audio2.Play();
		audio2.loop = true;
	}
	
	public void PlayOpenGate(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.clip = openGateClip;
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	public void PlayPocion(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		audio.clip = pocionClip;
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	public void PlayHitClip(Transform transform)
	{
		if (_onHitSound) return;

		_onHitSound = true;
	
		var audio = Instantiate(_prefab, transform);
		audio.clip = hitClips[UnityEngine.Random.Range(0, hitClips.Count)];
		audio.Play();
		Destroy(audio.gameObject, 5f);

		UniTask.Delay(1000).ContinueWith(() => _onHitSound = false);
	}

	public void PlayRecarga(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(1f, 1.4f);
		audio.clip = recargaClip;
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	public void PlayShotClip(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		audio.clip = shotClip;
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	public void PlayEnemyHitClip(EnemyType type, Transform transform)
	{
		Debug.Log("PlayEnemyHitClip");
		if (type == EnemyType.Melee)
		{
			PlayMeleeEnemyHitClip(transform);
			return;
		}

		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(0.7f, 1f);
		audio.clip = enemyHitClips[1];
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	private void PlayMeleeEnemyHitClip(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(1f, 1.3f);
		audio.clip = enemyHitClips[1];
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	public void PlayEnemyDie(EnemyType type, Transform transform)
	{
		if (type == EnemyType.Melee)
		{
			PlayMeleeEnemyDie(transform);
			return;
		}

		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		audio.clip = enemyDieClips[0];
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	private void PlayMeleeEnemyDie(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		audio.clip = enemyDieClips[1];
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}

	public void PlayPopBubbleClip(Transform transform)
	{
		var audio = Instantiate(_prefab, transform);
		audio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		audio.clip = popBubbleClip;
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}
}