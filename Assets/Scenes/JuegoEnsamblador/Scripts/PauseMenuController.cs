using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private const string ResumeFromPauseKey = "ResumeFromPause";

    [SerializeField] private string gameplaySceneName = "JuegoEnsamblador";

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
    }

    public void ResumeGame()
    {
        PlayerPrefs.SetInt(ResumeFromPauseKey, 1);
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }
}
