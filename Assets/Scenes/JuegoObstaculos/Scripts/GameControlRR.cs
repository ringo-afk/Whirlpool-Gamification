using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameControlRR : MonoBehaviour
{
    static public GameControlRR Instance; 

    public UIControllerRR uiController;
    // Trivia
    public GameObject triviaPanel;
    public TriviaController triviaController;
    public float triviaInterval = 15f;
    public float nextTrivia;

    private int lives = 5;
    public int levelsToWin = 4;
    public int currentLevel;
    private float time;
    public bool gameIsRunning;

    // Items
    private int coins;
    private bool hasShield = false;
    
    IEnumerator StartCountDown()
    {
        gameIsRunning = false;
        Time.timeScale = 0f;
        uiController.ShowCountdownText("3");
        yield return new WaitForSecondsRealtime(1f);

        uiController.ShowCountdownText("2");
        yield return new WaitForSecondsRealtime(1f);

        uiController.ShowCountdownText("1");
        yield return new WaitForSecondsRealtime(1f);

        uiController.ShowCountdownText("VAMOS!");
        yield return new WaitForSecondsRealtime(0.5f);

        uiController.HideCountdownText();

        Time.timeScale = 1f;
        gameIsRunning = true;

    }


    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartGame();
    }


    void Update()
    {
        if(!gameIsRunning)
            return;
        
        time += Time.deltaTime;
        uiController.UpdateTime(time);
        
        if(time >= nextTrivia)
        {
            ShowTrivia();
        }
        
    }
    void ShowTrivia()
    {
        gameIsRunning = false;
        Time.timeScale = 0f;
        // Activar panel de trivia
        triviaPanel.SetActive(true);
        triviaController.ShowQuestion();
    }

    public void AnswerTrivia(bool correct)
    {
        triviaPanel.SetActive(false);
        Time.timeScale = 1f;

        if(!correct)
            SpendLives();

        if(lives <= 0)
            return;

        currentLevel++;
        uiController.UpdateLevel(currentLevel);
        
        if(currentLevel > levelsToWin)
        {
            GameOver();
            return;
        }

        nextTrivia = time + triviaInterval;
        StartCoroutine(StartCountDown());

    }

    public void StartGame()
    {
        lives = 5;
        coins = 0;
        time = 0f;
        currentLevel = 1;
        gameIsRunning = false;
        StartCoroutine(StartCountDown());

        nextTrivia = triviaInterval;

        uiController.UpdateLives(lives);
        uiController.UpdateCoins(coins);
        uiController.UpdateTime(time);
        uiController.UpdateLevel(currentLevel);
    }

    public void ActivateShield()
    {
        hasShield = true;
    }
    public void SpendLives()
    {
        if (hasShield)
        {
            hasShield = false;
            return;
        }

        lives = lives - 1;
        uiController.UpdateLives(lives);

        if (lives <= 0)
            GameOver();

    }

    public void AddCoin()
    {
        coins = coins + 1;
        uiController.UpdateCoins(coins);
    }


    public void ResumeGameAfterTrivia()
    {
        Time.timeScale = 1f;
        gameIsRunning = true;
    }

    public void GameOver()
    {
        gameIsRunning = false;
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("FinalCoins", coins);
        PlayerPrefs.SetInt("FinalLives", lives);
        PlayerPrefs.SetFloat("FinalTime", time);

        SceneManager.LoadScene("EndSceneRoadRush");
    }
    
    public void GoToMenu()
    {
        //SceneManager.LoadScene("MenuScene");
    }
}
