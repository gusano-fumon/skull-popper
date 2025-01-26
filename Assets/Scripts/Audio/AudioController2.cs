using UnityEngine;


public class AudioController2 : MonoBehaviour
{
	public AudioClip clickClip;
	public AudioSource _prefab;

	public void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void PlayClick()
	{
		var audio = Instantiate(_prefab, transform);
		audio.clip = clickClip;
		audio.Play();
		Destroy(audio.gameObject, 5f);
	}
}