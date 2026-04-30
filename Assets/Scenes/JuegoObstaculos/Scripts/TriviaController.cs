using TMPro;
using UnityEngine;

public class TriviaController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answerA;
    public TextMeshProUGUI answerB;
    public TextMeshProUGUI answerC;
    public TextMeshProUGUI answerD;
    private bool answered = false;


    private int correctAnswer;

    public void ShowQuestion()
    {
        answered = false;

        questionText.text = "¿Qué es un prompt?";

        answerA.text = "Una instrucción para una IA";
        answerB.text = "Un tipo de obstáculo";
        answerC.text = "Una variable de Unity";
        answerD.text = "Un auto";

        correctAnswer = 0;
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