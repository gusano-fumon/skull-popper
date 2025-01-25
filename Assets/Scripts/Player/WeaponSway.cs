
using UnityEngine;


public class WeaponSway : MonoBehaviour
{
	public float swayAmount = 2f;
	public float swaySpeed = 5f;

	private Vector3 initialPosition;
	private float time;

	private void Start()
	{
		initialPosition = transform.localPosition;
	}

	private void Update()
	{
		time += Time.deltaTime * swaySpeed;

		// Horizontal movement sway
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		// Create 2D sine wave sway effect
		float swayX = Mathf.Sin(time) * swayAmount * horizontalInput;
		float swayY = Mathf.Cos(time) * swayAmount * .5f * verticalInput;

		// Apply sway
		transform.localPosition = initialPosition + new Vector3(swayX, swayY, 0);
	}
}