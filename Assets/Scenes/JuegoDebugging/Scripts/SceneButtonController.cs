using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonController : MonoBehaviour
{
    
    public void GoToInstructions()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DebuggingRace_Instrucciones");
    }

    
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DebuggingRace");
    }

    
    public void GoToPause()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("DebuggingRace_Pausa");
    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("2_Menu");
    }


    public void GoToFinalScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DebuggingRace_FinalScene");
    }

    // Cierra el juego cuando esté exportado
    public void QuitGame()
    {
        Application.Quit();
    }
}
