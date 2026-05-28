using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; 
    }
}

[System.Serializable]
public class JugadorTop
{
    public string Nombre;
    public int MejorPuntaje;
}

[System.Serializable]
public class TopList
{
    public JugadorTop[] Items;
}

public class LeaderboardAPI : MonoBehaviour
{
    [Header("Arrastra aquí tu TextMeshPro")]
    public TextMeshProUGUI textoLeaderboard;

    void Start()
    {
        textoLeaderboard.text = "Cargando datos desde la API...";
        StartCoroutine(ObtenerTopMemorama());
    }

    IEnumerator ObtenerTopMemorama()
    {
        string url = "https://127.0.0.1:5000/api/juegos/memorama/top";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new BypassCertificate();
            
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                textoLeaderboard.text = "Error de conexión:\n" + webRequest.error;
            }
            else
            {
                string jsonRespuesta = webRequest.downloadHandler.text;
                
                string wrappedJson = "{\"Items\":" + jsonRespuesta + "}";
                TopList listaJugadores = JsonUtility.FromJson<TopList>(wrappedJson);

                string textoFinal = "TOP 4 - CYBER MATCH\n--------------------\n";
                
                foreach (var jugador in listaJugadores.Items)
                {
                    textoFinal += $"{jugador.Nombre} ........ {jugador.MejorPuntaje} pts\n";
                }

                textoLeaderboard.text = textoFinal;
            }
        }
    }
}