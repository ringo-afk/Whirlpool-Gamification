using TMPro;
using UnityEngine;

public class DropRoundSequenceController : MonoBehaviour
{
    [System.Serializable]
    private class RoundData
    {
        [TextArea(2, 6)]
        public string promptText;
        public string[] requiredAnswerIdsForBoxes;
        public Draggable[] draggableOptionsBox1; // ← nuevo
        public Draggable[] draggableOptionsBox2;
        public Draggable[] draggableOptionsBox3;
    }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI promptTextUi;
    [SerializeField] private DropObject[] dropBoxes;
    [SerializeField] private DraggableSetSpawner draggableSpawner0; // caja 1 (al inicio)
    [SerializeField] private DraggableSetSpawner draggableSpawner1; // caja 2
    [SerializeField] private DraggableSetSpawner draggableSpawner2; // caja 3
    [SerializeField] private DropAnswerTracker answerTracker;

    [Header("Rounds")]
    [SerializeField] private RoundData[] rounds;
    [SerializeField] private bool loopRounds = true;

    private int currentRoundIndex = -1;
    private int filledBoxCount = 0;

    private void Awake()
    {
        if (answerTracker == null)
            answerTracker = FindObjectOfType<DropAnswerTracker>();
    }

    private void OnEnable()
    {
        foreach (var box in dropBoxes)
            if (box != null) box.OnDropFilled += HandleBoxFilled;
    }

    private void OnDisable()
    {
        foreach (var box in dropBoxes)
            if (box != null) box.OnDropFilled -= HandleBoxFilled;
    }

    private void Start()
    {
        LoadRound(0);
    }

    private void HandleBoxFilled()
    {
        filledBoxCount++;

        if (filledBoxCount == 1)
            draggableSpawner1?.SpawnNextSet();
        else if (filledBoxCount == 2)
            draggableSpawner2?.SpawnNextSet();
        else if (filledBoxCount >= 3)
            EvaluateAndAdvance();
    }

    private void EvaluateAndAdvance()
    {
        answerTracker?.NotifyDropChanged();

        int nextIndex = currentRoundIndex + 1;

        if (nextIndex >= rounds.Length)
        {
            if (!loopRounds) return;
            nextIndex = 0;
        }

        LoadRound(nextIndex);
    }

    private void LoadRound(int roundIndex)
    {
        if (rounds == null || rounds.Length == 0) return;
        if (roundIndex < 0 || roundIndex >= rounds.Length) return;

        currentRoundIndex = roundIndex;
        filledBoxCount = 0;

        RoundData round = rounds[roundIndex];
        if (round == null) return;

        if (promptTextUi != null)
            promptTextUi.text = round.promptText;

        ApplyBoxAnswers(round.requiredAnswerIdsForBoxes);

        // Preparar spawners 1 y 2 sin spawnear aún
        if (draggableSpawner1 != null)
            draggableSpawner1.SetDraggablePrefabs(round.draggableOptionsBox2);

        if (draggableSpawner2 != null)
            draggableSpawner2.SetDraggablePrefabs(round.draggableOptionsBox3);

        // Spawner 0 spawnea inmediatamente al cargar la ronda
        if (draggableSpawner0 != null)
        {
            draggableSpawner0.SetDraggablePrefabs(round.draggableOptionsBox1);
            draggableSpawner0.SpawnNextSet();
        }

        answerTracker?.BeginNewRound();
    }

    private void ApplyBoxAnswers(string[] requiredAnswerIds)
    {
        if (dropBoxes == null) return;

        for (int i = 0; i < dropBoxes.Length; i++)
        {
            if (dropBoxes[i] == null) continue;

            string requiredId = (requiredAnswerIds != null && i < requiredAnswerIds.Length)
                ? requiredAnswerIds[i]
                : string.Empty;

            dropBoxes[i].SetRequiredAnswerId(requiredId);
            dropBoxes[i].ResetDropState();
        }
    }
}