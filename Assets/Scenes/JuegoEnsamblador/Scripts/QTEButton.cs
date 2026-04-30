using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class QTEButton : MonoBehaviour, IPointerDownHandler
{
    public KeyCode[] qteKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    public bool useRandomKey = true;
    public KeyCode currentKey = KeyCode.W;

    public Image keyImage;
    public Sprite wSprite;
    public Sprite aSprite;
    public Sprite sSprite;
    public Sprite dSprite;
    public bool wrongIfClicked = false;
    public bool CorrectIfCLicked = false;


    public float minSecondsBetweenSpawns;
    public float maxSecondsBetweenSpawns;
    public float visibleSeconds;
    public bool triggerWrongOnTimeout = true;
    public int timeLostOnWrongInput;
    public int timeWinOnCorrectInput;
    public bool triggerCorrectOnTimeOut = true;

    public UnityEvent onCorrectInput;
    public UnityEvent onWrongInput;

    private bool isQTEActive;
    private float nextSpawnTimer;
    private float visibleTimer;

    private void OnEnable()
    {
        HideQTE();
        ScheduleNextSpawn();
    }

    private void Update()
    {
        if (!isQTEActive)
        {
            nextSpawnTimer -= Time.deltaTime;
            if (nextSpawnTimer <= 0f)
            {
                ShowQTE();
            }
            return;
        }

        if (IsKeyPressedThisFrame(currentKey))
        {
            ApplyCorrectInputReward();
            onCorrectInput?.Invoke();
            HideQTE();
            ScheduleNextSpawn();
            return;
        }

        for (int i = 0; i < qteKeys.Length; i++)
        {
            if (qteKeys[i] != currentKey && IsKeyPressedThisFrame(qteKeys[i]))
            {
                ApplyWrongInputPenalty();
                onWrongInput?.Invoke();
                HideQTE();
                ScheduleNextSpawn();
                break;
            }

            if (qteKeys[i] == currentKey && IsKeyPressedThisFrame(qteKeys[i]))
            {
                ApplyCorrectInputReward();
                onCorrectInput?.Invoke();
                HideQTE();
                ScheduleNextSpawn();
                break;
            }
        }

        visibleTimer -= Time.deltaTime;
        if (visibleTimer <= 0f)
        {
            if (triggerWrongOnTimeout)
            {
                ApplyWrongInputPenalty();
                onWrongInput?.Invoke();
            }

            if (triggerCorrectOnTimeOut)
            {
                ApplyCorrectInputReward();
                onWrongInput?.Invoke();
            }

            HideQTE();
            ScheduleNextSpawn();
        }
    }

    public void SetupQTE()
    {
        SelectKey();
        UpdateSprite();
    }

    public void SelectKey()
    {
        if (!useRandomKey || qteKeys == null || qteKeys.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, qteKeys.Length);
        currentKey = qteKeys[randomIndex];
    }

    public void UpdateSprite()
    {
        if (keyImage == null)
        {
            return;
        }

        switch (currentKey)
        {
            case KeyCode.W:
                keyImage.sprite = wSprite;
                break;
            case KeyCode.A:
                keyImage.sprite = aSprite;
                break;
            case KeyCode.S:
                keyImage.sprite = sSprite;
                break;
            case KeyCode.D:
                keyImage.sprite = dSprite;
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (wrongIfClicked && isQTEActive)
        {
            ApplyWrongInputPenalty();
            onWrongInput?.Invoke();
            HideQTE();
            ScheduleNextSpawn();
        }

        if (CorrectIfCLicked && isQTEActive)
        {
            ApplyCorrectInputReward();
            onCorrectInput?.Invoke();
            HideQTE();
            ScheduleNextSpawn();
        }
    }

    private void ApplyWrongInputPenalty()
    {
        if (GameControl.Instance == null)
        {
            return;
        }

        GameControl.Instance.timeToLose = Mathf.Max(0, GameControl.Instance.timeToLose - Mathf.Max(0, timeLostOnWrongInput));
        GameControl.Instance.RefreshTimerDisplay();
        GameControl.Instance.sfxManager.QTESound();
    }

    private void ApplyCorrectInputReward()
    {
        if (GameControl.Instance == null)
        {
            return;
        }

        GameControl.Instance.timeToLose = Mathf.Max(0, GameControl.Instance.timeToLose + Mathf.Max(0, timeWinOnCorrectInput));
        GameControl.Instance.RefreshTimerDisplay();
    }

    public void ShowQTE()
    {
        SetupQTE();
        isQTEActive = true;
        visibleTimer = Mathf.Max(0.1f, visibleSeconds);
        SetVisualActive(true);
    }

    public void HideQTE()
    {
        isQTEActive = false;
        visibleTimer = 0f;
        SetVisualActive(false);
    }

    public void ScheduleNextSpawn()
    {
        float minTime = Mathf.Max(0.1f, minSecondsBetweenSpawns);
        float maxTime = Mathf.Max(minTime, maxSecondsBetweenSpawns);
        nextSpawnTimer = Random.Range(minTime, maxTime);
    }

    private void SetVisualActive(bool isActive)
    {
        if (keyImage != null)
        {
            keyImage.enabled = isActive;
        }
    }

    private bool IsKeyPressedThisFrame(KeyCode key)
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }

        switch (key)
        {
            case KeyCode.W:
                return keyboard.wKey.wasPressedThisFrame;
            case KeyCode.A:
                return keyboard.aKey.wasPressedThisFrame;
            case KeyCode.S:
                return keyboard.sKey.wasPressedThisFrame;
            case KeyCode.D:
                return keyboard.dKey.wasPressedThisFrame;
            default:
                return false;
        }
    }
}
