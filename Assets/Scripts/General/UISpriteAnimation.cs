
using UnityEngine;
using UnityEngine.UI;


public class UISpriteAnimation : MonoBehaviour
{
    [SerializeField] protected Image _image;
	[SerializeField] private UnityEngine.Sprite[] _sprites;
	private int _spriteCounter = 0;

	protected virtual void Update()
	{
		if (Time.frameCount % 40 == 0) CheckSprite();
	}

	private void CheckSprite()
	{
		_spriteCounter++;
		_image.sprite = _sprites[_spriteCounter % 2];
	}

}
