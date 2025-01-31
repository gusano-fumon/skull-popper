using System;

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
	[SerializeField] private GameObject _tutorialPanel;
	public PlayerUI playerUI;

	public static bool inGame = false;

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
			_tutorialPanel.SetActive(true);

			inGame = true;
			ScoreController.Init();
        });

        _quitButton.onClick.AddListener(() => SceneController.QuitGame());

		OnVictory += Victory;
		OnPlayerDeath += PlayerDeath;
		GatedArea.OnTutorialComplete += TutorialComplete;
	}
    
	private void PlayerDeath()
    {
		inGame = false;
        Cursor.lockState = CursorLockMode.None;
        _retryPanel.SetActive(true);
        _endImage.sprite = _gameOverSprite;
    }

    private void Victory()
    {
		inGame = false;
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

	private void Quit()
	{
		SceneController.QuitGame();
	}
}
