using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class JugadorTopMemorama
{
    public string Nombre; 
    public int MejorPuntaje;           
}


public static class JsonHelperMemorama
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}


public class BypassSSLMemorama : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; 
    }
}

public class LeaderboardManager : MonoBehaviour
{
    
    public TextMeshProUGUI textoLeaderboard;
    
    private string apiUrl = "https://10.14.255.40:8003/api/juegos/memorama/top";

    void Start()
    {
        if (textoLeaderboard != null)
        {
            textoLeaderboard.text = "Cargando base de datos...";
        }
        
        StartCoroutine(ObtenerLeaderboard());
    }

    private IEnumerator ObtenerLeaderboard()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            
            webRequest.certificateHandler = new BypassSSLMemorama();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                textoLeaderboard.text = "<color=red>Error de conexión al servidor.</color>";
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                ProcesarDatos(jsonResponse);
            }
        }
    }

    private void ProcesarDatos(string json)
    {
        JugadorTopMemorama[] jugadores = JsonHelperMemorama.FromJson<JugadorTopMemorama>(json);

        if (jugadores == null || jugadores.Length == 0)
        {
            textoLeaderboard.text = "No hay registros disponibles.";
            return;
        }

        string textoFinal = "<color=#00FFFF>--- TOP JUGADORES ---</color>\n\n";
        int posicion = 1;

        foreach (JugadorTopMemorama jugador in jugadores)
        {
            textoFinal += $"{posicion}. <b>{jugador.Nombre}</b> - {jugador.MejorPuntaje} pts\n";
            posicion++;
        }

        textoLeaderboard.text = textoFinal;
    }
}