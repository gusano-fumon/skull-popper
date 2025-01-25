using System;

using UnityEngine;


public class MouseLook : MonoBehaviour
{
	public Transform playerBody;
	public float sensitivity = 100f;
	[SerializeField] private Bubble _bubble;
	private float _rotationX;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		var scaledSesitivity = sensitivity * Time.deltaTime;
		var mouseX = Input.GetAxis("Mouse X") * scaledSesitivity;
		var mouseY = Input.GetAxis("Mouse Y") * scaledSesitivity;

		playerBody.Rotate(mouseX * Vector3.up);

		_rotationX -= mouseY;
		_rotationX = Math.Clamp(_rotationX, -90, 90);

		transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		Debug.Log("Pium");
		var bubble = Instantiate(_bubble, transform.position, transform.rotation);
		bubble.Init(transform.forward);
	}
}