using UnityEngine;
using TMPro;

public class PantallaFinManager : MonoBehaviour
{
    [Header("Textos de la Interfaz")]
    public TextMeshProUGUI textoParesEncontrados;
    public TextMeshProUGUI textoEnergiaRestante;
    public TextMeshProUGUI textoTiempoSobrante; 
    public TextMeshProUGUI textoMedallonesGanados; 

    void Start()
    {
        
        int pares = PlayerPrefs.GetInt("ParesJugador", 0);
        int energiaFinal = PlayerPrefs.GetInt("EnergiaFinal", 0);
        float tiempoFinal = PlayerPrefs.GetFloat("TiempoSobrante", 0f);

        
        if (textoParesEncontrados != null)
        {
            textoParesEncontrados.text = "Pares Encontrados: " + pares;
        }

        
        if (textoEnergiaRestante != null)
        {
            textoEnergiaRestante.text = "Energía Restante: " + energiaFinal + " pts";
        }

        
        if (textoTiempoSobrante != null)
        {
            if (tiempoFinal > 0)
            {
                int minutos = Mathf.FloorToInt(tiempoFinal / 60);
                int segundos = Mathf.FloorToInt(tiempoFinal % 60);
                textoTiempoSobrante.text = string.Format("Tiempo Sobrante: {0:00}:{1:00}", minutos, segundos);
            }
            else
            {
                textoTiempoSobrante.text = "Tiempo Sobrante: 00:00";
            }
        }

        
        if (textoMedallonesGanados != null)
        {
            textoMedallonesGanados.text = "¡Has obtenido " + energiaFinal + " Medallones para tu equipo!";
        }
    }
}