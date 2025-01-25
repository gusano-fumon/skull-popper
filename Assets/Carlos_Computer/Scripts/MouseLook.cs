using System;

using UnityEngine;


public class MouseLook : MonoBehaviour
{
	public Transform playerBody;
	public float sensitivity = 100f;
	private float _rotationX;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		var mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		var mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		playerBody.Rotate(mouseX * Vector3.up);

		_rotationX -= mouseY;
		_rotationX = Math.Clamp(_rotationX, -90, 90);

		transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
	}
}