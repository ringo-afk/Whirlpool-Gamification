using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class GameControlDebugging : MonoBehaviour
{
    public static GameControlDebugging Instance;

    [Header("API")]
    public string resultadoApiUrl = "https://127.0.0.1:8002/api/debuggingrace/resultado";
    public int idUsuario = 1;

    [Header("Datos del jugador")]
    public int lives = 5;
    public int correctAnswers = 0;
    public int rewardPoints = 0;

    [Header("Progreso de carrera")]
    public float playerProgress = 0f;
    public float enemyProgress = 0f;
    public float goalProgress = 180f;

    [Header("Velocidad")]
    public float baseSpeed = 3f;
    public float boostAmount = 10f;

    [Header("Estado del juego")]
    public bool gameRunning = true;

    private float gameTime = 0f;
    private float baseProgress = 0f;
    private float playerBonus = 0f;
    private float enemyBonus = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lives = 5;
        correctAnswers = 0;
        rewardPoints = 0;

        baseProgress = 0f;
        playerBonus = 0f;
        enemyBonus = 0f;

        playerProgress = 0f;
        enemyProgress = 0f;

        gameTime = 0f;
        gameRunning = true;
    }

    void Update()
    {
        if (!gameRunning)
        {
            return;
        }

        gameTime += Time.deltaTime;

        baseProgress += baseSpeed * Time.deltaTime;

        playerProgress = baseProgress + playerBonus;
        enemyProgress = baseProgress + enemyBonus;

        CheckGameResult();
    }

    //----------------------------
    public void CorrectAnswer()
    {
        correctAnswers++;
        rewardPoints += 100;

        playerBonus += boostAmount;
    }

    //--------------------------------
    public void WrongAnswer()
    {

        enemyBonus += boostAmount;
    }

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            LoseGame();
        }
    }

    public void AddRewardPoints(int points)
    {
        rewardPoints += points;
    }

    public void AddPlayerBoost(float boost)
    {
        // Boost por power-up
        playerBonus += boost;
    }

    private void CheckGameResult()
    {
        if (playerProgress >= goalProgress)
        {
            WinGame();
        }
        else if (enemyProgress >= goalProgress)
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        gameRunning = false;

        PlayerPrefs.SetString("DebuggingResult", "Ganaste");
        SaveFinalData();

        StartCoroutine(SendResultAndGoFinal());
    }

    private void LoseGame()
    {
        gameRunning = false;

        PlayerPrefs.SetString("DebuggingResult", "Perdiste");
        SaveFinalData();

        StartCoroutine(SendResultAndGoFinal());
    }

    private void SaveFinalData()
    {
        PlayerPrefs.SetInt("DebuggingLives", lives);
        PlayerPrefs.SetInt("DebuggingCorrectAnswers", correctAnswers);
        PlayerPrefs.SetInt("DebuggingRewardPoints", rewardPoints);
        PlayerPrefs.SetFloat("DebuggingTime", gameTime);
        PlayerPrefs.Save();
    }

    IEnumerator SendResultAndGoFinal()
    {
        ResultadoDebuggingAPI resultado = new ResultadoDebuggingAPI();

        resultado.idUsuario = idUsuario;
        resultado.kilometros = playerProgress;
        resultado.monedasGanadas = rewardPoints;

        string json = JsonUtility.ToJson(resultado);

        UnityWebRequest web = new UnityWebRequest(resultadoApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        web.uploadHandler = new UploadHandlerRaw(bodyRaw);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.certificateHandler = new ForceAcceptAll();

        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error guardando resultado: " + web.error);
            Debug.Log("Respuesta API: " + web.downloadHandler.text);
        }
        else
        {
            Debug.Log("Resultado guardado en BD: " + web.downloadHandler.text);
        }

        SceneManager.LoadScene("DebuggingRace_FinalScene");
    }
}
