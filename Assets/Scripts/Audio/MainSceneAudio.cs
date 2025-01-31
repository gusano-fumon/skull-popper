using UnityEngine;
using UnityEngine.UI;


public class MainSceneAudio : MonoBehaviour
{
	[SerializeField] private Button[] _buttons;

	private void Awake()
	{
		foreach (var button in _buttons)
		{
			button.onClick.AddListener(() => AudioFactory.Instance.PlaySFX(AudioType.PopBubble));
		}
	}
}