
using UnityEngine;


public class Sprite : MonoBehaviour
{
	private void Update()
	{
		transform.LookAt(PlayerController.Transform);
	}
}