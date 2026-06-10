using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;
    [SerializeField] private int juegoId = 5;

    void Start()
    {
        int coins = PlayerPrefs.GetInt("FinalCoins", 0);
        int lives = PlayerPrefs.GetInt("FinalLives", 0);
        float time = PlayerPrefs.GetFloat("FinalTime", 0f);

        coinsText.text = "" + coins;
        livesText.text = "" + lives;
        timeText.text = "Tiempo: " + FormatTime(time);

        //StartCoroutine(Guardar(time, coins));

    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    
    private IEnumerator Guardar(float time, int coins)
    {
        int kilometrosObtenidos = Mathf.FloorToInt(time);

        PartidaData data = new PartidaData
        {
            usuario_id = PlayerPrefs.GetInt("IDUsuario", 1),
            juego_id = juegoId,
            kilometros = kilometrosObtenidos,
            monedas_ganadas = coins
        };

        string jsonData = JsonUtility.ToJson(data);
        string url = "https://10.14.255.40:8000/api/juegos/guardar-partida";

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

    }


    public void StartGame()
    {
        SceneManager.LoadScene("GameSceneRoadRush");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuRoadRush");
    }
}

