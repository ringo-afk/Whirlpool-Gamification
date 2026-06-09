using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DropRoundSequenceController : MonoBehaviour
{
    [Header("API")]
    [SerializeField] private string apiUrl = "https://10.22.146.252:8443/getPrompt/";
    [SerializeField] private int promptId = 14;
    [SerializeField]private int[] promptIds = {11,12,13,14};
    [Header("References")]
    [SerializeField] private TextMeshProUGUI promptTextUi;
    [SerializeField] private DropObject[] dropBoxes;

    [SerializeField] private DraggableSetSpawner draggableSpawner0;
    [SerializeField] private DraggableSetSpawner draggableSpawner1;
    [SerializeField] private DraggableSetSpawner draggableSpawner2;

    [SerializeField] private DropAnswerTracker answerTracker;

    private int filledBoxCount = 0;

    private List<ApiAnswer> contextoOptions;
    private List<ApiAnswer> tareaOptions;

    private void Awake()
    {
        if (answerTracker == null)
            answerTracker = FindObjectOfType<DropAnswerTracker>();
    }

    private void OnEnable()
    {
        foreach (var box in dropBoxes)
        {
            if (box != null)
                box.OnDropFilled += HandleBoxFilled;
        }
    }

    private void OnDisable()
    {
        foreach (var box in dropBoxes)
        {
            if (box != null)
                box.OnDropFilled -= HandleBoxFilled;
        }
    }

    private void Start()
    {promptId = promptIds[Random.Range(0, promptIds.Length)];

    StartCoroutine(LoadPromptFromApi());
    }

    private IEnumerator LoadPromptFromApi()
    {
        UnityWebRequest request =
            UnityWebRequest.Get(apiUrl + promptId);

        request.certificateHandler =
            new BypassCertificate();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        Debug.Log(request.downloadHandler.text);

        PromptResponse prompt =
            JsonUtility.FromJson<PromptResponse>(
                request.downloadHandler.text);

        if (prompt == null)
        {
            Debug.LogError("No se pudo parsear PromptResponse");
            yield break;
        }

        if (promptTextUi != null)
            promptTextUi.text = prompt.Prompt;

        ApiAnswer[] answers =
            JsonHelper.FromJson<ApiAnswer>(
                prompt.Respuestas);

        if (answers == null || answers.Length < 12)
        {
            Debug.LogError(
                "Se esperaban al menos 12 respuestas. Llegaron: "
                + (answers == null ? 0 : answers.Length));

            yield break;
        }

        Debug.Log("Respuestas cargadas: " + answers.Length);

        foreach (var box in dropBoxes)
        {
            if (box == null) continue;

            box.SetRequiredAnswerId("correct");
            box.ResetDropState();
        }

        filledBoxCount = 0;

        List<ApiAnswer> rol =
            new List<ApiAnswer>(answers).GetRange(0, 4);

        contextoOptions =
            new List<ApiAnswer>(answers).GetRange(4, 4);

        tareaOptions =
            new List<ApiAnswer>(answers).GetRange(8, 4);

        answerTracker?.BeginNewRound();

        draggableSpawner0.SpawnApiOptions(rol);
    }

    private void HandleBoxFilled()
    {
        filledBoxCount++;

        Debug.Log(
            "Caja completada. Total: "
            + filledBoxCount);

        if (filledBoxCount == 1)
        {
            draggableSpawner1.SpawnApiOptions(
                contextoOptions);
        }
        else if (filledBoxCount == 2)
        {
            draggableSpawner2.SpawnApiOptions(
                tareaOptions);
        }
        else if (filledBoxCount >= 3)
        {
            PromptCompleted();
        }
    }

    private void PromptCompleted()
    {
    Debug.Log("Prompt completado");

    answerTracker?.NotifyDropChanged();

    promptId = promptIds[Random.Range(0, promptIds.Length)];

    StartCoroutine(LoadPromptFromApi());
    }
}