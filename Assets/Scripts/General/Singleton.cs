
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance
	{
		get => GetInstance();
	}

	public virtual void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		
		_instance = GetComponent<T>();
	}

	private static T GetInstance()
	{
		if (_instance != null) return _instance;

		_instance = FindObjectOfType<T>();

		return _instance;
	}
}