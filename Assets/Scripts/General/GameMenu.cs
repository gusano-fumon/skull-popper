using System;

using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;


public class GameMenu : Singleton<GameMenu>
{
    [SerializeField] private GameObject _winImage, _gameOverImage;

	public static bool IsPaused;
	public static Action OnVictory;
	public static Action OnPlayerDeath;
	[SerializeField] private GameObject _mainMenu;
	[SerializeField] private GameObject _pauseMenu;
	[SerializeField] private GameObject _controlsPanel;
	[SerializeField] private GameObject _optionsPanel;
	[SerializeField] private GameObject _retryPanel;
	[SerializeField] private GameObject _tutorialPanel;
	public PlayerUI playerUI;

	public static bool inGame = false;

	[Space]
	[SerializeField] private Button _playButton;
	[SerializeField] private Button _retryButton;
	[SerializeField] private Button _mainMenuButton;
	[SerializeField] private Button _quitButton;

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		
		_retryButton.onClick.AddListener(Retry);
		_mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        _playButton.onClick.AddListener(async () => {
            await SceneController.LoadScene(1);
			playerUI.Init();
            _mainMenu.SetActive(false);
			_tutorialPanel.SetActive(true);

			inGame = true;
			ScoreController.Init();
        });

        _quitButton.onClick.AddListener(QuitGame);

		OnVictory += Victory;
		OnPlayerDeath += PlayerDeath;
		GatedArea.OnTutorialComplete += TutorialComplete;
	}

	public void QuitGame()
	{
		SceneController.QuitGame();
	}

	private void OnDestroy()
	{
		OnVictory -= Victory;
		OnPlayerDeath -= PlayerDeath;
	}
    
	private void PlayerDeath()
    {
		inGame = false;
        _retryPanel.SetActive(true);
		_gameOverImage.SetActive(true);
		_winImage.SetActive(false);

		UniTask.Delay(1000).ContinueWith(() => {
        	Cursor.lockState = CursorLockMode.None;
		});
    }

    private void Victory()
    {
		inGame = false;
        Cursor.lockState = CursorLockMode.None;
        _retryPanel.SetActive(true);
		_gameOverImage.SetActive(false);
		_winImage.SetActive(true);
	}

	private void Retry()
	{
		UniTask.Void(async () => {
			ScoreController.Init();
			await SceneController.LoadScene(1);
			playerUI.Init();
			_retryPanel.SetActive(false);
		});
	}

	private void TutorialComplete()
	{
		_tutorialPanel.SetActive(false);
	}

	private void ReturnToMainMenu()
	{
		UniTask.Void(async () => {
			_retryPanel.SetActive(false);
			_mainMenu.SetActive(true);
			await SceneController.LoadScene(0);
		});
	}

	public void OpenControlsPanel()
    {
        _controlsPanel.SetActive(true);
    }

    public void CloseControlsPanel()
    {
        _controlsPanel.SetActive(false);
    }

	public void OpenOptionsPanel()
    {
        _optionsPanel.SetActive(true);
    }

	public void CloseOptionsPanel()
    {
        _optionsPanel.SetActive(false);
    }

	private void Quit()
	{
		SceneController.QuitGame();
	}

	public void Pause()
	{
		IsPaused = true;
		_pauseMenu.SetActive(true);
		playerUI.gameObject.SetActive(false);
		AudioFactory.Instance.StopMusic();
		AudioFactory.Instance.StopSFX();
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
	}

	public void Resume()
	{
		IsPaused = false;
		playerUI.gameObject.SetActive(true);
		_pauseMenu.SetActive(false);
		Time.timeScale = 1;
		AudioFactory.Instance.PlayMusic(AudioType.BackgroundMusic);
		Cursor.lockState = CursorLockMode.Locked;
	}
}
