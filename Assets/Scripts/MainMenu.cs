using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("StartingScreen"); 
    }

    public void ExitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
