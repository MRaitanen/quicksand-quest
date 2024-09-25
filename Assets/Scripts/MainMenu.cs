using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load "Main" scene
        SceneManager.LoadSceneAsync("Main");
    }

    public void ExitGame()
    {
        // Close game
        Application.Quit();
    }
}
