
using UnityEngine;


public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] protected MeshRenderer _meshRenderer;
	[SerializeField] private Texture2D[] _sprites;
	private int _spriteCounter = 0;

	private void OnDestroy()
	{
		_meshRenderer.sharedMaterial.mainTexture = _sprites[0];
	}

	protected virtual void Update()
	{
		if (Time.frameCount % 40 == 0) CheckSprite();
	}

	private void CheckSprite()
	{
		_spriteCounter++;
		_meshRenderer.sharedMaterial.mainTexture = _sprites[_spriteCounter % 2];
	}

}
