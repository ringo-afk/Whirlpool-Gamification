using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;

    void Start()
    {
        int coins = PlayerPrefs.GetInt("FinalCoins", 0);
        int lives = PlayerPrefs.GetInt("FinalLives", 0);
        float time = PlayerPrefs.GetFloat("FinalTime", 0f);

        coinsText.text = "" + coins;
        livesText.text = "" + lives;
        timeText.text = "Tiempo: " + FormatTime(time);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
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