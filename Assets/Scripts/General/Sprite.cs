
using UnityEngine;


public class Sprite : MonoBehaviour
{
	protected int _spriteCounter = 0;

	protected virtual void Update()
	{
		transform.LookAt(PlayerController.Transform);
		if (Time.frameCount % 40 == 0) CheckSprite();
	}

	protected virtual void CheckSprite() { }
}