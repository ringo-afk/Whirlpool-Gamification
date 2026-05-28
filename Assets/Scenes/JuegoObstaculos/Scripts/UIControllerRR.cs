using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIControllerRR : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI countdownText;

    public GameObject darkness;

    public void ShowCountdownText(string text)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = text;
    }
    public void HideCountdownText()
    {
        countdownText.gameObject.SetActive(false);
    }
    public void ShowDarkness(float duration)
    {
        StartCoroutine(DarknessRoutine(duration));
    }
    
    IEnumerator DarknessRoutine(float duration)
    {
        darkness.SetActive(true);

        yield return new WaitForSeconds(duration);

        darkness.SetActive(false);
    }

    public void UpdateTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        timeText.text = string.Format("{0:00}:{1:00}:{2:00}",
            minutes, seconds, milliseconds);


    }

    public void UpdateLives(int lives)
    {
        livesText.text = "" + lives;
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = "" + coins;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Nivel " + level;
    }

}
