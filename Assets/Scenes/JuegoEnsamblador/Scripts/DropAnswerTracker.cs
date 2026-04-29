using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropAnswerTracker : MonoBehaviour
{
    public const string CorrectSetsKey = "CorrectSetsCompleted";

    [Header("Round setup")]
    [Tooltip("How many drop slots must be correct to count a full correct set.")]
    [SerializeField] private int requiredCorrectDrops = 3;

    [Header("Progress")]
    [SerializeField] private int correctSetsCompleted;
    [SerializeField] private UnityEvent<int> onCorrectSetsChanged;

    private readonly HashSet<DropObject> registeredDropObjects = new HashSet<DropObject>();
    private bool roundAlreadyCounted;

    public int CorrectSetsCompleted => correctSetsCompleted;

    private void Awake()
    {
        correctSetsCompleted = PlayerPrefs.GetInt(CorrectSetsKey, 0);
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
