using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _retryPanel;
    [SerializeField] private Image _endImage;
    [SerializeField] private UnityEngine.Sprite _winSprite;
    [SerializeField] private UnityEngine.Sprite _gameOverSprite;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        PlayerController.OnPlayerDeath += PlayerDeath;
        VictoryZone.OnVictory += Victory;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= PlayerDeath;
        VictoryZone.OnVictory -= Victory;
    }

    private void Start()
    {
        _retryButton.onClick.AddListener(Retry);
        _mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        _quitButton.onClick.AddListener(Quit);
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
        _endImage.sprite = _winSprite;
    }

    private void Retry()
    {
        _retryPanel.SetActive(false);
        SceneController.LoadScene(1);
    }

    private void ReturnToMainMenu()
    {
        _retryPanel.SetActive(false);
        SceneController.LoadScene(0);
    }

    private void Quit()
    {
        SceneController.QuitGame();
    }
}
