using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseControllerDebugging : MonoBehaviour
{
    // Detecta cuando el jugador presiona ESC
    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene("DebuggingRace_Pausa");
        }
    }
}
