using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DropRoundSequenceController : MonoBehaviour
{
    [Header("API")]
    [SerializeField] private string apiUrl = "https://10.14.255.40:8004/getPrompt/";
    [SerializeField] private int requestTimeoutSeconds = 10;
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

        DisableLegacyDropEvents();
    }

    private void DisableLegacyDropEvents()
    {
        foreach (var box in dropBoxes)
        {
            if (box != null)
                box.onDropped.RemoveAllListeners();
        }

        draggableSpawner0?.DisableAutoSpawn();
        draggableSpawner1?.DisableAutoSpawn();
        draggableSpawner2?.DisableAutoSpawn();

        draggableSpawner0?.SetApiControlled(true);
        draggableSpawner1?.SetApiControlled(true);
        draggableSpawner2?.SetApiControlled(true);
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

    private string BuildPromptUrl(int id)
    {
        string baseUrl = apiUrl.Trim();

        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.TrimEnd('/');
        }

        int lastSlash = baseUrl.LastIndexOf('/');
        if (lastSlash >= 0 && lastSlash < baseUrl.Length - 1)
        {
            string tail = baseUrl.Substring(lastSlash + 1);
            if (int.TryParse(tail, out _))
            {
                baseUrl = baseUrl.Substring(0, lastSlash);
            }
        }

        return baseUrl + "/" + id;
    }

    private IEnumerator LoadPromptFromApi()
    {
        string requestUrl = BuildPromptUrl(promptId);
        Debug.Log("Solicitando prompt: " + requestUrl);

        using UnityWebRequest request = UnityWebRequest.Get(requestUrl);
        request.certificateHandler = new ByPassCertificateJE();
        request.timeout = requestTimeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                "Error API (" + request.responseCode + "): "
                + request.error
                + "\nURL: " + requestUrl);

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
        LogAnswerGroup("Rol", answers, 0, 4);
        LogAnswerGroup("Contexto", answers, 4, 4);
        LogAnswerGroup("Tarea", answers, 8, 4);

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

        draggableSpawner1?.SpawnApiOptions(null);
        draggableSpawner2?.SpawnApiOptions(null);
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
            Debug.Log("Spawneando opciones de Contexto");
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
            answerTracker?.RecordRoundComplete();
            PromptCompleted();
        }
    }

    private static void LogAnswerGroup(string label, ApiAnswer[] answers, int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ApiAnswer answer = answers[start + i];
            Debug.Log(
                label + " [" + i + "]: "
                + answer.texto + " | correcta=" + answer.correcta);
        }
    }

    private void PromptCompleted()
    {
        Debug.Log("Prompt completado correctamente");

        promptId = promptIds[Random.Range(0, promptIds.Length)];

        StartCoroutine(LoadPromptFromApi());
    }
}