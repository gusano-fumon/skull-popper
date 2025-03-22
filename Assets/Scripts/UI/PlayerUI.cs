using System;

using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

using DG.Tweening;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	public static Action<int, bool> OnHit;
	public Slider healthSlider;
	public TMP_Text healthText, ammoChangeText, healthChangeText, ammoText, scoreText;
	public Slider ammoSlider;

	[SerializeField] private Image _damageImage;
	[SerializeField] private RectTransform _playerUI;
	
	[SerializeField] private float _damageFadeDuration = .1f;
	[SerializeField] private float _clearFadeDuration = 1.5f;
	[SerializeField] private float _endAlpha = .5f;
	[SerializeField] private float _cameraShake = .2f;
	[SerializeField] private float _uiShake = 10f;
	[SerializeField] private Vector3 _uiInitialPosition;

	public GameObject defaultState, bubbleWand, reloadingStateUp, reloadingStateDown;
	private Sequence damageSequence;

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
		SetUI();
	}

	private void SetUI()
	{
		healthText.SetText("<sketchy>100");
		healthSlider.value = 100;

		ammoText.SetText("<sketchy>12");
		ammoSlider.value = 12;
	}

	public void UpdateHealth(int health, bool isDamage)
	{
		if (isDamage) TakeDamage();

		AnimateTextChange((int)healthSlider.value - health, true).Forget();

		healthSlider.value = health;
		if (health <= 0)
		{
			healthText.SetText("<sketchy>DEAD");
			return;
		}

		healthText.SetText($"<sketchy>{health}");
	}

	private async UniTaskVoid AnimateTextChange(int value, bool isHealth)
	{
		value = -value;

		var textGameObject = Instantiate(
			isHealth ? healthChangeText.gameObject : ammoChangeText.gameObject,
			isHealth ? healthText.transform.position : ammoText.transform.position,
			isHealth ? healthText.transform.rotation : ammoText.transform.rotation,
			isHealth ? healthText.transform : ammoText.transform
		);

		var text = textGameObject.GetComponent<TMP_Text>();

		var endPos = isHealth ? new Vector3(88, 35, 0) : new Vector3(-120, 80, 0);
		var endText = !isHealth && value > 0 ? "Max!" : value.ToString();


		text.DOKill();
		text.color = value > 0 ? Color.green : Color.red;
		text.SetText($"<sketchy>{endText}");
		text.DOFade(1f, .2f);

		text.transform
			.DOLocalMove(endPos, .5f)
			.From(Vector3.zero)
			.SetEase(Ease.OutSine);

		await UniTask.Delay(500);
		text.DOFade(0f, .2f);
		await UniTask.Delay(200);
		Destroy(text.gameObject);
	}

	public void UpdateAmmo(int ammo)
	{
		AnimateTextChange((int)ammoSlider.value - ammo, false).Forget();

		ammoSlider.value = ammo;
		ammoText.SetText($"<sketchy>{ammo}");
	}

	public async UniTaskVoid ShootAnimation()
	{
		bubbleWand.transform.DOScale(Vector3.one * 1.1f, .05f).SetEase(Ease.OutExpo);
		await UniTask.Delay(100);
		bubbleWand.transform.DOScale(Vector3.one, .01f).SetEase(Ease.OutQuad);
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
		reloadingStateUp.SetActive(up);
		reloadingStateDown.SetActive(!up);
	}
}