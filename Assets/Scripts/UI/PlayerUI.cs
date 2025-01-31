using System;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;


public class PlayerUI : MonoBehaviour
{
	public static Action<int, bool> OnHit;
	public Slider healthSlider;
	public TMP_Text healthText;
	public Slider ammoSlider;
	public TMP_Text ammoText;

	[SerializeField] private Image _damageImage;
	[SerializeField] private RectTransform _playerUI;
	
	[SerializeField] private float _damageFadeDuration = 0.1f;
	[SerializeField] private float _clearFadeDuration = 1.5f;
	[SerializeField] private float _endAlpha = 0.5f;
	[SerializeField] private float _cameraShake = 0.2f;
	[SerializeField] private float _uiShake = 10f;
	[SerializeField] private Vector3 _uiInitialPosition;

	private Sequence damageSequence;

	public Image defaultState, reloadingState;
	public UnityEngine.Sprite upReload, downReload;

	private void Awake()
	{
		MouseLook.OnReload += UpdateAmmo;
		OnHit += UpdateHealth;
	}

	void OnDestroy()
	{
		MouseLook.OnReload -= UpdateAmmo;
		OnHit -= UpdateHealth;
	}

	public void Init()
	{
		_uiInitialPosition = _playerUI.anchoredPosition;
		UpdateHealth(100, false);
		UpdateAmmo(12);
	}

	public void UpdateHealth(int health, bool isDamage)
	{
		if (isDamage) TakeDamage();

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

		var camera = Camera.main;
		var cameraInitialPosition = camera.transform.localPosition;
		camera.transform.DOLocalMove(cameraInitialPosition, 0);

		damageSequence = DOTween.Sequence();
		damageSequence.Append(_damageImage.DOFade(_endAlpha, _damageFadeDuration));
		damageSequence.Append(_damageImage.DOFade(0, _clearFadeDuration));

		camera.DOShakePosition(_damageFadeDuration, _cameraShake);
		_playerUI.DOShakeAnchorPos(_damageFadeDuration, _uiShake);
		_playerUI.DOAnchorPos(_uiInitialPosition, 0);
		camera.transform.DOLocalMove(cameraInitialPosition, 0);
	}

	public void ReloadDirection(bool up)
	{
		reloadingState.sprite = up ? upReload : downReload;
	}
}