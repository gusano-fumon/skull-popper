
using UnityEngine;

using Cysharp.Threading.Tasks;


public class SpriteLookAt : MonoBehaviour
{
	[SerializeField] private bool _isBullet;
	private void Start()
	{
		if (_isBullet) return;

		UniTask.Void(async () =>
		{
			await UniTask.Delay(200);
			enabled = false;
			await UniTask.Delay(200);
			enabled = true;
		});
	}

	private void Update()
	{
		if (Time.frameCount < 50) return;
		if (GameMenu.IsPaused) return;
		if (!enabled) return;

		transform.LookAt(PlayerController.PlayerCamera.transform);
	}
}