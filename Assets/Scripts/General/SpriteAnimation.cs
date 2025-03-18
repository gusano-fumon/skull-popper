
using UnityEngine;
using UnityEngine.UI;


public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] protected MeshRenderer _meshRenderer;
    [SerializeField] protected Image _image;
	[SerializeField] private Texture2D[] _sprites;
	[SerializeField] private Sprite[] _imageSprites;
	private int _spriteCounter = 0;

	private void OnDestroy()
	{
		if (_meshRenderer != null) _meshRenderer.sharedMaterial.mainTexture = _sprites[0];
		if (_image != null) _image.sprite = _imageSprites[0];
	}

	protected virtual void Update()
	{
		if (Time.frameCount % 40 == 0) CheckSprite();
	}

	private void CheckSprite()
	{
		_spriteCounter++;

		if (_meshRenderer != null) _meshRenderer.sharedMaterial.mainTexture = _sprites[_spriteCounter % 2];
		if (_image != null) _image.sprite = _imageSprites[_spriteCounter % 2];
	}
}