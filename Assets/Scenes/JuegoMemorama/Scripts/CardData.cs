using UnityEngine;
[System.Serializable]
public class CardData
{
    public int pairID;         // Para saber qué problema va con qué prompt (Ej: 1 con 1)
    public string cardText;    // El texto que se va a mostrar (el problema o el prompt)
    public bool isProblem;     // true si es el Problema, false si es el Prompt
}
