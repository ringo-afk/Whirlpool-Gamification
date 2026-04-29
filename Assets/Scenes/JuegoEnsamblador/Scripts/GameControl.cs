using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameControl : MonoBehaviour
{
    private const string TimeToLoseKey = "TimeToLose";
    private const string TimeIngameKey = "TimeIngame";
    private const string ResumeFromPauseKey = "ResumeFromPause";

    public int timeToLose = 60;
    static public GameControl Instance;
    public UIController UIController;
    public QTEButton QteButton;
    private Coroutine timerCoroutine;
    public int timeIngame;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        StopAllCoroutines();

        bool shouldResumeFromPause = PlayerPrefs.GetInt(ResumeFromPauseKey, 0) == 1;
        if (shouldResumeFromPause)
        {
            timeToLose = PlayerPrefs.GetInt(TimeToLoseKey, timeToLose);
            PlayerPrefs.SetInt(ResumeFromPauseKey, 0);
            PlayerPrefs.Save();
        }
        else
        {
            // Fresh start: honor inspector value and clear stale saved timer.
            PlayerPrefs.DeleteKey(TimeToLoseKey);
        }
    }

    void SetReferences()
    {
        if (UIController == null)
        {
            UIController = FindAnyObjectByType<UIController>();
        }

        init();
    }

    void init()
    {
        if (UIController != null)
        {
            UIController.UpdateTimerText(timeToLose);
        }

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(MatchTime());
    }

    void Start()
    {
        Time.timeScale = 1f;
        SetReferences();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        PlayerPrefs.SetInt(TimeToLoseKey, timeToLose);
        PlayerPrefs.SetInt(TimeIngameKey, timeIngame);
        PlayerPrefs.SetInt(ResumeFromPauseKey, 1);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        SceneManager.LoadScene("Pausado");
    }

    public void RefreshTimerDisplay()
    {
        if (UIController != null)
        {
            UIController.UpdateTimerText(timeToLose);
        }
    }

    System.Collections.IEnumerator MatchTime()
    {
        timeIngame = 0;
        if (UIController != null)
        {
            UIController.UpdateTimerText(timeToLose);
        }

        while (timeToLose > 0)
        {
            yield return new WaitForSeconds(1f);
            timeIngame++;
            timeToLose--;

            if (UIController != null)
            {
                UIController.UpdateTimerText(timeToLose);
            }
        }

        PlayerPrefs.SetInt(TimeIngameKey, timeIngame);
        PlayerPrefs.SetInt(ResumeFromPauseKey, 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EscenaFinal");
    }
}
