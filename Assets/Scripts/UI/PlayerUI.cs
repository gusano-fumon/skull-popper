using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;


public class PlayerUI : MonoBehaviour
{
	public Slider healthSlider;
	public TMP_Text healthText;
	public Slider ammoSlider;
	public TMP_Text ammoText;

	[SerializeField] private Image _damageImage;
	[SerializeField] private Camera _playerCamera;
	[SerializeField] private RectTransform _playerUI;
	
	[SerializeField] private float _damageFadeDuration = 0.1f;
	[SerializeField] private float _clearFadeDuration = 1.5f;
	[SerializeField] private float _endAlpha = 0.5f;
	[SerializeField] private float _cameraShake = 0.2f;
	[SerializeField] private float _uiShake = 10f;
	[SerializeField] private Vector3 _uiInitialPosition;
	[SerializeField] private Vector3 _cameraInitialPosition;

	private Sequence damageSequence;

	private void Start()
	{
		_uiInitialPosition = _playerUI.anchoredPosition;
		_cameraInitialPosition = _playerCamera.transform.localPosition;
	}

	public void UpdateHealth(int health)
	{
		healthSlider.value = health;
		if (health <= 0)
		{
			healthText.text = "DEAD";
			return;
		}
		healthText.text = health.ToString();
	}

	public void UpdateAmmo(int ammo)
	{
		ammoSlider.value = ammo;
		ammoText.text = ammo.ToString();
	}

	public void TakeDamage()
	{
		damageSequence?.Kill();
		_playerUI.DOAnchorPos(_uiInitialPosition, 0);
		_playerCamera.transform.DOLocalMove(_cameraInitialPosition, 0);

		damageSequence = DOTween.Sequence();
		damageSequence.Append(_damageImage.DOFade(_endAlpha, _damageFadeDuration));
		damageSequence.Append(_damageImage.DOFade(0, _clearFadeDuration));

		_playerCamera.DOShakePosition(_damageFadeDuration, _cameraShake);
		_playerUI.DOShakeAnchorPos(_damageFadeDuration, _uiShake);
		_playerUI.DOAnchorPos(_uiInitialPosition, 0);
		_playerCamera.transform.DOLocalMove(_cameraInitialPosition, 0);
	}
}
