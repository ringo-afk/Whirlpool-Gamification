using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private const string ResumeFromPauseKey = "ResumeFromPause";
    [SerializeField] private string JuegoEnsamblaje;

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            ResumeGame();
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("JuegoEnsamblaje");
        }
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("2_Menu");
    }
    public void resume()
    {
        SceneManager.LoadScene("JuegoEnsamblaje");
        ResumeGame();
    }

    public void ResumeGame()
    {
        PlayerPrefs.SetInt(ResumeFromPauseKey, 1);
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene("JuegoEnsamblaje");
    }
}
