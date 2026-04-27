using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameControl : MonoBehaviour
{
    public int timeToLose = 60;
    static public GameControl Instance;
    public UIController UIController;
    public QTEButton QteButton;
    private Coroutine timerCoroutine;


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
        timeToLose = PlayerPrefs.GetInt("TimeToLose", timeToLose);
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
        PlayerPrefs.SetInt("TimeToLose", timeToLose);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        SceneManager.LoadScene("Pausado");
    }

    System.Collections.IEnumerator MatchTime()
    {
        while (timeToLose > 0)
        {
            yield return new WaitForSeconds(1f);
            timeToLose--;

            if (UIController != null)
            {
                UIController.UpdateTimerText(timeToLose);
            }
        }

        SceneManager.LoadScene("EscenaFinal");
    }
}
