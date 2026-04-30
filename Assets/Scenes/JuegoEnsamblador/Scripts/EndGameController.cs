using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public TextMeshProUGUI timeIngameText;
    public TextMeshProUGUI correctAnswersText;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int elapsedSeconds = PlayerPrefs.GetInt("TimeIngame", 0);
        int correctSets = PlayerPrefs.GetInt(DropAnswerTracker.CorrectSetsKey, 0);
        int minutes = elapsedSeconds / 60;
        int seconds = elapsedSeconds % 60;
        int score = (elapsedSeconds * 100) + (correctSets*100);

        if (timeIngameText != null)
        {
            timeIngameText.text = $"{minutes:00}:{seconds:00}";
        }

        if (correctAnswersText != null)
        {
            correctAnswersText.text = correctSets.ToString();
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        
    }
    
    public void GoToMenu()
    {
        SceneManager.LoadScene("2_Menu");
    }
}
