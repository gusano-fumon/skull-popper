
using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;


public static class SceneController 
{
    public static async UniTask LoadScene(int index)
    {
       await SceneManager.LoadSceneAsync(index);
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
