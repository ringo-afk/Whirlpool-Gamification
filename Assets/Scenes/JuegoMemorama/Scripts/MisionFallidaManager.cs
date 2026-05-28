using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MisionFallidaManager : MonoBehaviour
{
    public TextMeshProUGUI textoParesEncontrados;
    public TextMeshProUGUI textoTiempoSobrante;
    public TextMeshProUGUI textoEnergiaRestante;

    void Start()
    {
        int pares = PlayerPrefs.GetInt("ParesJugador", 0);
        float tiempoFinal = PlayerPrefs.GetFloat("TiempoSobrante", 0f);
        int energia = PlayerPrefs.GetInt("EnergiaFinal", 0);

        if (textoParesEncontrados != null) textoParesEncontrados.text = "Pares Encontrados: " + pares + "/8";
        if (textoEnergiaRestante != null) textoEnergiaRestante.text = "Energía Restante: " + energia + " pts";

        if (textoTiempoSobrante != null)
        {
            int minutos = Mathf.FloorToInt(tiempoFinal / 60);
            int segundos = Mathf.FloorToInt(tiempoFinal % 60);
            textoTiempoSobrante.text = string.Format("Tiempo Sobrante: {0:00}:{1:00}", minutos, segundos);
        }
    }

    public void Reintentar()
    {
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }
}