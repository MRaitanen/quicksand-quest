using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        // Load "MainMenu" Scene
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
