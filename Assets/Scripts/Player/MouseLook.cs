using System;
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;


public class MouseLook : MonoBehaviour
{
	public Transform playerBody;
	public float sensitivity = 100f;
	[SerializeField] private Bubble _bubble;
	private float _rotationX;

	[SerializeField] private int rechargeTime = 3000;
	private bool _recharging = false;
	public bool Recharging { 
		get { return _recharging; }
		set {
			_recharging = value;
			if (_recharging)
			{
				StartRecharging().Forget();
			}
			else
			{
				Debug.Log("Stopped Recharging");
				_cts.Cancel();
			}
		}
	}

	private CancellationTokenSource _cts;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (!Recharging)
		{
			var scaledSesitivity = sensitivity * Time.deltaTime;
			var mouseX = Input.GetAxis("Mouse X") * scaledSesitivity;
			var mouseY = Input.GetAxis("Mouse Y") * scaledSesitivity;

			playerBody.Rotate(mouseX * Vector3.up);

			_rotationX -= mouseY;
			_rotationX = Math.Clamp(_rotationX, -90, 90);

			transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
		}

		if (Input.GetMouseButtonDown(1)) Recharging = true;
		if (Input.GetMouseButtonUp(1)) Recharging = false;

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	private async UniTaskVoid StartRecharging()
	{
		_cts = new CancellationTokenSource();
		Debug.Log("StartRecharging");
		
		await UniTask.Delay(rechargeTime, cancellationToken: _cts.Token);

		Debug.Log("Finish Recharging");
	}

	private void Shoot()
	{
		var bubble = Instantiate(_bubble, transform.position, transform.rotation);
		bubble.Init(transform.forward);
	}
}