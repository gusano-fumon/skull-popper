using UnityEngine;
using  UnityEngine.SceneManagement;

public static class SceneController 
{
    public static void LoadScene(int index)
    {
       SceneManager.LoadScene(index);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
