using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DropAnswerTracker : MonoBehaviour
{
    public const string CorrectSetsKey = "CorrectSetsCompleted";
    

    [SerializeField] private int requiredCorrectDrops = 3;

    [SerializeField] private int correctSetsCompleted;
    [SerializeField] private UnityEvent<int> onCorrectSetsChanged;

    private readonly HashSet<DropObject> registeredDropObjects = new HashSet<DropObject>();
    private bool roundAlreadyCounted;

    public int CorrectSetsCompleted => correctSetsCompleted;
    public event Action RoundCompleted;

    private void Awake()
    {
        correctSetsCompleted = 0;
        SaveProgress();
    }

    public void RegisterDropObject(DropObject dropObject)
    {
        if (dropObject == null) return;
        registeredDropObjects.Add(dropObject);
    }

    public void UnregisterDropObject(DropObject dropObject)
    {
        if (dropObject == null) return;
        registeredDropObjects.Remove(dropObject);
    }

    public void NotifyDropChanged()
    {
        EvaluateRound();
    }

    public void BeginNewRound()
    {
        roundAlreadyCounted = false;
    }

    public void ResetTracker()
    {
        correctSetsCompleted = 0;
        roundAlreadyCounted = false;
        SaveProgress();
        onCorrectSetsChanged?.Invoke(correctSetsCompleted);
    }

    private void EvaluateRound()
    {
        int correctCount = 0;

        foreach (DropObject dropObject in registeredDropObjects)
        {
            if (dropObject != null && dropObject.IsCurrentlyCorrect())
            {
                correctCount++;
            }
        }

        bool allCorrect = correctCount >= requiredCorrectDrops;

        if (allCorrect && !roundAlreadyCounted)
        {
            correctSetsCompleted++;
            roundAlreadyCounted = true;
            SaveProgress();
            onCorrectSetsChanged?.Invoke(correctSetsCompleted);
            RoundCompleted?.Invoke();
        }
        else if (!allCorrect)
        {
            roundAlreadyCounted = false;
        }
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(CorrectSetsKey, correctSetsCompleted);
        PlayerPrefs.Save();
    }
}
