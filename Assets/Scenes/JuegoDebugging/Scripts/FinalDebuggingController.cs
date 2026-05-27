using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDebuggingController : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI correctAnswersText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI rewardText;

    void Start()
    {
        string result = PlayerPrefs.GetString("DebuggingResult", "Resultado");
        int lives = PlayerPrefs.GetInt("DebuggingLives", 0);
        int correctAnswers = PlayerPrefs.GetInt("DebuggingCorrectAnswers", 0);
        int points = PlayerPrefs.GetInt("DebuggingRewardPoints", 0);
        float time = PlayerPrefs.GetFloat("DebuggingTime", 0f);

        resultText.text = result;
        livesText.text = "Vidas restantes: " + lives;
        correctAnswersText.text = "Respuestas correctas: " + correctAnswers;
        pointsText.text = "Puntos: " + points;
        timeText.text = "Tiempo: " + FormatTime(time);
        rewardText.text = "Recompensa: " + GetReward(points, result);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    string GetReward(int points, string result)
    {
        if (result == "Perdiste")
        {
            return "Sigue practicando prompts";
        }

        if (points >= 600)
        {
            return "Experto en prompts";
        }
        else if (points >= 300)
        {
            return "Buen creador de prompts";
        }
        else
        {
            return "Principiante de prompts";
        }
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DebuggingRace");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("2_Menu");
    }
}
