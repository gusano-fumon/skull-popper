
using UnityEngine;


public class Sprite : MonoBehaviour
{
	[SerializeField] protected MeshRenderer _meshRenderer;
	protected int _spriteCounter = 0;

	protected virtual void Update()
	{
		transform.LookAt(PlayerController.CameraTransform);
		if (Time.frameCount % 40 == 0) CheckSprite();
	}

	protected virtual void CheckSprite() { }
}