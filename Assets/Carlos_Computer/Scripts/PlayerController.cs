using UnityEngine;


public class PlayerController : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;
	[SerializeField] private CharacterController _character;

	private void Update()
	{
		MoveXZ();
	}

	private void MoveXZ()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		var move = transform.right * horizontal + transform.forward * vertical;
		_character.Move(_movementSpeed * Time.deltaTime * move);
	}
}
