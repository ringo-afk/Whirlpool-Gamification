using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPantallasFinales : MonoBehaviour
{
    public void ReintentarJuegoCompleto()
    {
        Time.timeScale = 1f;
        GameManager.esUnReintento = true;
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }

    public void RegresarAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        GameManager.esUnReintento = false;
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }
}
