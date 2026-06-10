using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [Header("Configuración de Partida")]
    [SerializeField] private int juegoId = 5;

    private bool guardandoPartida = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !guardandoPartida)
        {
            if (GameControl.Instance.checkpointsInt >= GameControl.Instance.checkpointsNeeded)
            {
                if (GameControl.Instance.Laps + 1 >= GameControl.Instance.TotalLaps)
                {
                    guardandoPartida = true;
                    PlayerPrefs.SetInt("Monedas", GameControl.Instance.monedas);
                    
                    StartCoroutine(GuardarYTerminar());
                }
                else
                {
                    GameControl.Instance.Laps += 1;
                    GameControl.Instance.lapsText.UpdateLaps();
                }
            }

            GameControl.Instance.checkpointsInt = 0;

            foreach (Checkpoint cp in Checkpoint.AllCheckpoints)
            {
                cp.ResetCheckpoint();
            }
        }
    }

    private IEnumerator GuardarYTerminar()
    {
        float tiempoTranscurrido = GameControl.Instance.tiempoInicio - GameControl.Instance.timer.tiempo;
        int kilometrosObtenidos = Mathf.FloorToInt(tiempoTranscurrido);
        PlayerPrefs.SetFloat("Tiempo", tiempoTranscurrido);
        PlayerPrefs.SetInt("Kilometros", kilometrosObtenidos);
        PlayerPrefs.SetInt("Monedas", GameControl.Instance.monedas);

        PartidaData data = new PartidaData
        {
            usuario_id = GameControl.Instance.usuarioIdActual,
            juego_id = juegoId,
            kilometros = kilometrosObtenidos,
            monedas_ganadas = GameControl.Instance.monedas
        };

        string jsonData = JsonUtility.ToJson(data);
        string url = $"{GameControl.Instance.apiBaseUrl}juegos/guardar-partida";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            webRequest.certificateHandler = new BypassCertificate();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error al guardar resultados en BD: " + webRequest.error);
            }
        }

        SceneManager.LoadScene("Ganaste");
    }
}

[System.Serializable]
public class PartidaData
{
    public int usuario_id;
    public int juego_id;
    public int kilometros;
    public int monedas_ganadas;
}