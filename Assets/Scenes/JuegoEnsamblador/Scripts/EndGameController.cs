using UnityEngine;
using TMPro;

public class EndGameController : MonoBehaviour
{
    public TextMeshProUGUI timeIngameText;
    public TextMeshProUGUI correctAnswersText;

    void Start()
    {
        int elapsedSeconds = PlayerPrefs.GetInt("TimeIngame", 0);
        int correctSets = PlayerPrefs.GetInt(DropAnswerTracker.CorrectSetsKey, 0);
        int minutes = elapsedSeconds / 60;
        int seconds = elapsedSeconds % 60;

        if (timeIngameText != null)
        {
            timeIngameText.text = $"{minutes:00}:{seconds:00}";
        }

        if (correctAnswersText != null)
        {
            correctAnswersText.text = correctSets.ToString();
        }
    }
}
