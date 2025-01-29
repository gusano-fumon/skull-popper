
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	private static bool _applicationIsQuitting = false;

	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting)
			{
				Debug.LogWarning($"[Singleton] Instancia de {nameof(T)} ya no está disponible porque el juego está cerrándose.");
				return null;
			}

			return GetInstance();
		}
	}

	protected virtual void Awake()
	{


		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		
		_instance = GetComponent<T>();
	}

	protected virtual void OnApplicationQuit()
	{
		_applicationIsQuitting = true;
	}


	private static T GetInstance()
	{
		if (_instance != null) return _instance;

		_instance = FindObjectOfType<T>();

		if (_instance == null)
		{
			GameObject singletonObject = new ($"{nameof(T)} (Singleton)");
			_instance = singletonObject.AddComponent<T>();
		}

		return _instance;
	}
}