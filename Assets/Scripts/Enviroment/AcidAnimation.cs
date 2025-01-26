using UnityEngine;


public class AcidAnimation : MonoBehaviour
{
	public MeshRenderer targetRenderer;
	private readonly float _scrollSpeedX = .3f;
    private readonly float _scrollSpeedY = .1f;

	private void Update()
	{
		Vector2 offset = new (
			Time.time * _scrollSpeedX % 1, 
			Time.time * _scrollSpeedY % 1
		);

		targetRenderer.material.mainTextureOffset = offset;
	}
}