using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IniciarJugar : MonoBehaviour
{
     [SerializeField] private string JuegoEnsamblaje;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("JuegoEnsamblaje");
        }
    }
}
