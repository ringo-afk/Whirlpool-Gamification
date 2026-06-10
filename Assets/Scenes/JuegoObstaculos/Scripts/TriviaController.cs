using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TriviaController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answerA;
    public TextMeshProUGUI answerB;
    public TextMeshProUGUI answerC;
    public TextMeshProUGUI answerD;
    private bool answered = false;
    private int correctAnswer;

    private string apiBaseUrl = "https://10.14.255.40:8010/getPrompt/"; 
    private List<int> preguntasDisponibles = new List<int>();
        
    [Serializable]
    public class Respuesta
    {
        public string texto;
        public bool correcta;
    }

    [Serializable]
    public class PreguntaApi
    {
        public int IDPrompt;
        public string Prompt;
        public Respuesta[] Respuestas;
    }

    void Start()
    {
        ReiniciarPreguntas();
    }

    void ReiniciarPreguntas()
    {
        preguntasDisponibles.Clear();

        for (int i = 15; i <= 25; i++)
        {
            preguntasDisponibles.Add(i);
        }
    }


    public void ShowQuestion()
    {
        answered = false;
        
        if (preguntasDisponibles.Count == 0)
        {
            ReiniciarPreguntas();
        }

        int posicionRandom = UnityEngine.Random.Range(0, preguntasDisponibles.Count);

        int idPrompt = preguntasDisponibles[posicionRandom];

        preguntasDisponibles.RemoveAt(posicionRandom);

        StartCoroutine(CargarPregunta(idPrompt));

    }

    IEnumerator CargarPregunta(int idPrompt)
    {
        string url = apiBaseUrl + idPrompt;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.certificateHandler = new BypassCertificate();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            Debug.Log("Pregunta recibida:");
            Debug.Log(json);

            PreguntaApi pregunta =
                JsonUtility.FromJson<PreguntaApi>(json);

            MostrarPregunta(pregunta);
        }
        else
        {
            Debug.LogError("Error API: " + request.error);

            questionText.text =
                "No se pudo cargar la pregunta";

            answerA.text = "";
            answerB.text = "";
            answerC.text = "";
            answerD.text = "";
        }
    }

    void MostrarPregunta(PreguntaApi pregunta)
    {
        questionText.text = pregunta.Prompt;

        answerA.text = pregunta.Respuestas[0].texto;
        answerB.text = pregunta.Respuestas[1].texto;
        answerC.text = pregunta.Respuestas[2].texto;
        answerD.text = pregunta.Respuestas[3].texto;

        for (int i = 0; i < pregunta.Respuestas.Length; i++)
        {
            if (pregunta.Respuestas[i].correcta)
            {
                correctAnswer = i;
                break;
            }
        }
    }

    public void SelectAnswer(int index)
    {
        if(answered)
            return;
        answered = true;            
        bool correct = index == correctAnswer;

        GameControlRR.Instance.AnswerTrivia(correct);
    }
}

