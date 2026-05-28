using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void CargarJuego()
    {
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }

    public void CargarInstrucciones()
    {
        SceneManager.LoadScene("Pantalla_Instrucciones");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("NombreDeTuEscenaDeMenu");
    }
}