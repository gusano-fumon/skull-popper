
using UnityEngine;


public class Sprite : MonoBehaviour
{
	protected virtual void Update()
	{
		transform.LookAt(PlayerController.Transform);
	}
}