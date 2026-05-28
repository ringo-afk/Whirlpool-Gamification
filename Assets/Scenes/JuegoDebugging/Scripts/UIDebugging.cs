using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugging : MonoBehaviour
{
    
    public Image[] hearts;
    // ---------------------------------
    public RectTransform playerMiniCar;
    public RectTransform enemyMiniCar;

    public float startX = -250f;
    public float finishX = 250f;

    public TextMeshProUGUI correctAnswersText;
    public TextMeshProUGUI rewardPointsText;

    void Update()
    {
        if (GameControlDebugging.Instance == null)
        {
            return;
        }

        UpdateHearts();
        UpdateRaceProgress();
        UpdateTexts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < GameControlDebugging.Instance.lives)
            {
                hearts[i].gameObject.SetActive(true);
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateRaceProgress()
    {

        // ----------------------------------------------------------
        float playerPercent = GameControlDebugging.Instance.playerProgress / GameControlDebugging.Instance.goalProgress;
        float enemyPercent = GameControlDebugging.Instance.enemyProgress / GameControlDebugging.Instance.goalProgress;

        playerPercent = Mathf.Clamp01(playerPercent);
        enemyPercent = Mathf.Clamp01(enemyPercent);

        //----------------------------------------------------
        float playerX = Mathf.Lerp(startX, finishX, playerPercent);
        float enemyX = Mathf.Lerp(startX, finishX, enemyPercent);

        playerMiniCar.anchoredPosition = new Vector2(playerX, playerMiniCar.anchoredPosition.y);
        enemyMiniCar.anchoredPosition = new Vector2(enemyX, enemyMiniCar.anchoredPosition.y);
    }

    void UpdateTexts()
    {
        correctAnswersText.text = "Correctas: " + GameControlDebugging.Instance.correctAnswers;
        rewardPointsText.text = "Puntos: " + GameControlDebugging.Instance.rewardPoints;
    }
}