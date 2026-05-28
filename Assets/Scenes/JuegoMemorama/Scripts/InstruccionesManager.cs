using UnityEngine;
using UnityEngine.SceneManagement;

public class InstruccionesManager : MonoBehaviour
{
    public void VolverAlJuego()
    {
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }
}