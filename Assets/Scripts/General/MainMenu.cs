using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private GameObject _controlsPanel;

    private void Start()
    {
        _playButton.onClick.AddListener(() => SceneController.LoadScene(1));
        _quitButton.onClick.AddListener(() => SceneController.QuitGame());
    }

    public void OpenControlsPanel()
    {
        _controlsPanel.SetActive(true);
    }

    public void CloseControlsPanel()
    {
        _controlsPanel.SetActive(false);
    }
}
