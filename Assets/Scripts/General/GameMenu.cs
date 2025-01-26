using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;


public class GameMenu : Singleton<GameMenu>
{
    [SerializeField] private Image _endImage;
    [SerializeField] private UnityEngine.Sprite _winSprite;
    [SerializeField] private UnityEngine.Sprite _gameOverSprite;
	public static Action OnVictory;
	public static Action OnPlayerDeath;
	[SerializeField] private GameObject _mainMenu;
	[SerializeField] private GameObject _controlsPanel;
	[SerializeField] private GameObject _retryPanel;
	public PlayerUI playerUI;

	[Space]
	[SerializeField] private Button _playButton;
	[SerializeField] private Button _retryButton;
	[SerializeField] private Button _mainMenuButton;
	[SerializeField] private Button _quitButton;

	public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		
		_retryButton.onClick.AddListener(Retry);
		_mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        _playButton.onClick.AddListener(async () => {
            await SceneController.LoadScene(1);
			playerUI.Init();
            _mainMenu.SetActive(false);
        });

        _quitButton.onClick.AddListener(() => SceneController.QuitGame());

		OnVictory += Victory;
		OnPlayerDeath += PlayerDeath;
	}
    
	private void PlayerDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        _retryPanel.SetActive(true);
        _endImage.sprite = _gameOverSprite;
    }

    private void Victory()
    {
        Cursor.lockState = CursorLockMode.None;
        _retryPanel.SetActive(true);
	}

	private void Retry()
	{
		UniTask.Void(async () => {
			await SceneController.LoadScene(1);
			playerUI.Init();
			_retryPanel.SetActive(false);
		});
	}

	private void ReturnToMainMenu()
	{
		_retryPanel.SetActive(false);
		SceneController.LoadScene(0).Forget();
	}

	public void OpenControlsPanel()
    {
        _controlsPanel.SetActive(true);
    }

    public void CloseControlsPanel()
    {
        _controlsPanel.SetActive(false);
    }

	private void Quit()
	{
		SceneController.QuitGame();
	}
}
