using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _retryPanel;
    [SerializeField] private TMP_Text _retryText;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        PlayerController.OnPlayerDeath += PlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= PlayerDeath;
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
        _retryText.text = "You died!";
        _retryText.color = Color.red;
    }

    private void Victory()
    {
        Cursor.lockState = CursorLockMode.None;
        _retryPanel.SetActive(true);
        _retryText.text = "Victory!";
        _retryText.color = Color.green;
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
