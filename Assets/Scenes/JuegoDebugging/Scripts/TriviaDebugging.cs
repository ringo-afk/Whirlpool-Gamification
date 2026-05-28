using TMPro;
using UnityEngine;

public class TriviaDebugging : MonoBehaviour
{
    //-----------------------------------
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;
    public TextMeshProUGUI answer3Text;

    private int currentQuestion = 0;
    private int correctAnswer = 0;

    //-------------------------------
    private string[] questions =
    {
        "Crea una descripción para una lavadora _____",
        "Genera un anuncio para un refrigerador con ahorro de _____",
        "Escribe un correo para explicar una garantía de _____",
        "Crea una imagen de una cocina moderna con _____",
        "Redacta instrucciones para usar una secadora de forma _____",
        "Genera ideas para mejorar la experiencia del _____",
        "Crea un prompt para comparar dos modelos de _____",
        "Escribe una respuesta amable para un cliente _____",
        "Diseña una campaña para electrodomésticos _____",
        "Crea una guía rápida para limpiar un _____"
    };

    private string[,] answers =
    {
        { "ayer", "eficiente", "azulmente" },
        { "zapato", "energía", "rápido" },
        { "producto", "nube", "correr" },
        { "lunes", "hambre", "electrodomésticos" },
        { "mesa", "segura", "cinco" },
        { "cliente", "teclado", "océano" },
        { "verde", "lavadoras", "saltando" },
        { "triángulo", "kilómetro", "molesto" },
        { "sustentables", "perro", "234" },
        { "mañana", "horno", "ruido" }
    };

    private int[] correctAnswers =
    {
        1, // eficiente
        1, // energia
        0, // producto
        2, // electrodomesticos
        1, // segura
        0, // cliente
        1, // lavadoras
        2, // molesto
        0, // sustentables
        1  // horno
    };

    void Start()
    {
        ShowQuestion();
    }

    //--------------------------------------------
    public void SelectAnswer(int answerIndex)
    {
        if (answerIndex == correctAnswer)
        {
            if (SFXManagerDebugging.Instance != null)
            {
                SFXManagerDebugging.Instance.PlayQuestionBoostSound();
            }

            GameControlDebugging.Instance.CorrectAnswer();
        }
        else
        {
            GameControlDebugging.Instance.WrongAnswer();
        }

        currentQuestion++;

        if (currentQuestion >= questions.Length)
        {
            currentQuestion = 0;
        }

        ShowQuestion();
    }

    //-----------------------------------------------
    void ShowQuestion()
    {
        questionText.text = questions[currentQuestion];

        answer1Text.text = answers[currentQuestion, 0];
        answer2Text.text = answers[currentQuestion, 1];
        answer3Text.text = answers[currentQuestion, 2];

        correctAnswer = correctAnswers[currentQuestion];
    }
}