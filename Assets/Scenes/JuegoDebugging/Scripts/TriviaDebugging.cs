using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class TriviaDebugging : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;
    public TextMeshProUGUI answer3Text;
    public TextMeshProUGUI feedbackText;

    public string apiUrl = "https://10.14.255.40:8002/api/debuggingrace/preguntas";

    private List<PreguntaAPI> preguntas = new List<PreguntaAPI>();

    private int currentQuestion = 0;
    private int correctAnswer = 0;
    private bool canAnswer = false;

    void Start()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        StartCoroutine(LoadQuestionsFromAPI());
    }

    IEnumerator LoadQuestionsFromAPI()
    {
        questionText.text = "Cargando preguntas...";

        UnityWebRequest web = UnityWebRequest.Get(apiUrl);
        web.certificateHandler = new ForceAcceptAll();

        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            questionText.text = "Error al cargar preguntas";
            Debug.Log("Error API: " + web.error);
        }
        else
        {
            preguntas = JsonConvert.DeserializeObject<List<PreguntaAPI>>(web.downloadHandler.text);

            if (preguntas == null || preguntas.Count == 0)
            {
                questionText.text = "No hay preguntas disponibles";
            }
            else
            {
                canAnswer = true;
                ShowQuestion();
            }
        }
    }

    public void SelectAnswer(int answerIndex)
    {
        if (!canAnswer)
        {
            return;
        }

        canAnswer = false;

        if (answerIndex == correctAnswer)
        {
            if (SFXManagerDebugging.Instance != null)
            {
                SFXManagerDebugging.Instance.PlayQuestionBoostSound();
            }

            GameControlDebugging.Instance.CorrectAnswer();

            if (feedbackText != null)
            {
                feedbackText.color = Color.green;
                feedbackText.text = "Correcto: recibiste boost";
            }
        }
        else
        {
            GameControlDebugging.Instance.WrongAnswer();

            if (feedbackText != null)
            {
                feedbackText.color = Color.red;
                feedbackText.text = "Incorrecto: la CPU recibió boost";
            }
        }

        StartCoroutine(NextQuestionAfterFeedback());
    }

    IEnumerator NextQuestionAfterFeedback()
    {
        yield return new WaitForSeconds(1f);

        currentQuestion++;

        if (currentQuestion >= preguntas.Count)
        {
            currentQuestion = 0;
        }

        ShowQuestion();

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        canAnswer = true;
    }

    void ShowQuestion()
    {
        PreguntaAPI preguntaActual = preguntas[currentQuestion];

        questionText.text = preguntaActual.prompt;

        answer1Text.text = "";
        answer2Text.text = "";
        answer3Text.text = "";

        correctAnswer = 0;

        for (int i = 0; i < preguntaActual.respuestas.Count; i++)
        {
            if (i == 0)
            {
                answer1Text.text = preguntaActual.respuestas[i].texto;
            }
            else if (i == 1)
            {
                answer2Text.text = preguntaActual.respuestas[i].texto;
            }
            else if (i == 2)
            {
                answer3Text.text = preguntaActual.respuestas[i].texto;
            }

            if (preguntaActual.respuestas[i].correcta)
            {
                correctAnswer = i;
            }
        }
    }
}