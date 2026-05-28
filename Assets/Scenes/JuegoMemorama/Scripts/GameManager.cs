using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Configuración del Tablero")]
    public GameObject cardPrefab;
    public Transform boardPanel;
    
    [Header("Base de Datos Interna")]
    public List<CardData> allAvailablePairs = new List<CardData>(); 
    private List<CardData> deckInPlay = new List<CardData>();
    private List<CardVisual> allCardsOnBoard = new List<CardVisual>();
    
    [Header("Funcionalidad 3: Memoria del Robot")]
    public List<CardVisual> memoriaRobot = new List<CardVisual>();
    
    [Header("Funcionalidad 5: Economía y Poderes")]
    public int energiaJugador = 5; 
    public TextMeshProUGUI textoEnergia; 
    public Button btnSaltarTurno;       
    public Button btnCongelar;          
    public Button btnRevolver;          
    public int costoSaltar = 1;          
    public int costoCongelar = 2;        
    public int costoRevolver = 2;        
    private bool saltarTurnoEnemigoActivo = false;
    
    [Header("Efectos Visuales")]
    public Image panelFondo; 

    [Header("Salón de la Fama (F4)")]
    public Transform panelParesColumna; 
    
    public bool canPlayerPlay = true; 
    
    [Header("Memoria del Tablero")]
    private CardVisual firstCardRevealed;
    private CardVisual secondCardRevealed;

    void Start()
    {
        GenerateBoard(8); 
        ActualizarBotonesPoderes(); 
    }

    public void GenerateBoard(int totalCards)
    {
        int totalPairs = totalCards / 2;
        deckInPlay.Clear();

        for (int i = 0; i < totalPairs; i++)
        {
            deckInPlay.Add(allAvailablePairs[i * 2]);
            deckInPlay.Add(allAvailablePairs[(i * 2) + 1]);
        }

        ShuffleDeck();
        PopulateGrid();
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deckInPlay.Count; i++)
        {
            CardData temp = deckInPlay[i];
            int randomIndex = Random.Range(i, deckInPlay.Count);
            deckInPlay[i] = deckInPlay[randomIndex];
            deckInPlay[randomIndex] = temp;
        }
    }

    private void PopulateGrid()
    {
        foreach (Transform child in boardPanel)
        {
            Destroy(child.gameObject);
        }

        allCardsOnBoard.Clear();

        for (int i = 0; i < deckInPlay.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, boardPanel);
            CardVisual cv = newCard.GetComponent<CardVisual>();
            cv.SetupCard(deckInPlay[i], this); 
            allCardsOnBoard.Add(cv);
        }
    }

    public void CardRevealed(CardVisual card)
    {
        if (firstCardRevealed == card) return;

        card.FlipCard();
        
        // FUNCIONALIDAD 4: Escalar
        card.GetComponent<CardAnimator>().AnimarPopUp();

        if (firstCardRevealed == null)
        {
            firstCardRevealed = card;
            Debug.Log("Primera carta revelada.");
        }
        else if (secondCardRevealed == null)
        {
            secondCardRevealed = card;
            Debug.Log("Segunda carta revelada.");
            canPlayerPlay = false; 
            
            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        yield return new WaitForSeconds(1.8f); 

        if (firstCardRevealed.cardData.pairID == secondCardRevealed.cardData.pairID)
        {
            Debug.Log("¡MATCH del Jugador! Ganas +1 Energía.");
            
            // REDISEÑO VISUAL (F4)
            firstCardRevealed.GetComponent<CardAnimator>().AnimarViajeAlSofa(panelParesColumna);
            secondCardRevealed.GetComponent<CardAnimator>().AnimarViajeAlSofa(panelParesColumna);

            energiaJugador++;
            ActualizarBotonesPoderes();
            canPlayerPlay = true; 
        }
        else
        {
            Debug.Log("Error. Turno del Robot.");
            
            firstCardRevealed.GetComponent<CardAnimator>().AnimarRegreso();
            secondCardRevealed.GetComponent<CardAnimator>().AnimarRegreso();
            firstCardRevealed.UnflipCard();
            secondCardRevealed.UnflipCard();

            if (saltarTurnoEnemigoActivo)
            {
                Debug.Log("Poder Activo: El robot pierde su turno. ¡Vas de nuevo!");
                saltarTurnoEnemigoActivo = false; 
                canPlayerPlay = true;
            }
            else
            {
                StartCoroutine(RobotTurnRoutine());
            }
        }

        firstCardRevealed = null;
        secondCardRevealed = null;
    }

    // --- FUNCIONALIDAD 5: MÉTODOS DE LOS BOTONES ---

    public void ActivarPoderSaltar()
    {
        if (energiaJugador >= costoSaltar && canPlayerPlay)
        {
            energiaJugador -= costoSaltar;
            saltarTurnoEnemigoActivo = true;
            ActualizarBotonesPoderes();
            Debug.Log("Poder comprado: Saltar turno enemigo.");
            StartCoroutine(FlashPantalla(new Color(1f, 0f, 0f, 0.5f))); 
        }
    }

    public void ActivarPoderCongelar()
    {
        if (energiaJugador >= costoCongelar && canPlayerPlay)
        {
            energiaJugador -= costoCongelar;
            ActualizarBotonesPoderes();
            Debug.Log("Poder comprado: Inmunidad activa. Tu turno sigue gratis.");
            StartCoroutine(FlashPantalla(new Color(0f, 1f, 1f, 0.5f))); 
        }
    }

    public void ActivarPoderRevolver()
    {
        if (energiaJugador >= costoRevolver && canPlayerPlay)
        {
            energiaJugador -= costoRevolver;
            ActualizarBotonesPoderes();
            Debug.Log("Poder comprado: Tablero revuelto (Solo cartas boca abajo).");

            List<Transform> cartasOcultas = new List<Transform>();
            List<int> indicesDisponibles = new List<int>();

            foreach (CardVisual card in allCardsOnBoard)
            {
                if (!card.isFlipped) 
                {
                    cartasOcultas.Add(card.transform);
                    indicesDisponibles.Add(card.transform.GetSiblingIndex());
                }
            }

            for (int i = 0; i < indicesDisponibles.Count; i++)
            {
                int temp = indicesDisponibles[i];
                int randomIndex = Random.Range(i, indicesDisponibles.Count);
                indicesDisponibles[i] = indicesDisponibles[randomIndex];
                indicesDisponibles[randomIndex] = temp;
            }

            for (int i = 0; i < cartasOcultas.Count; i++)
            {
                cartasOcultas[i].SetSiblingIndex(indicesDisponibles[i]);
            }

            memoriaRobot.Clear();
            StartCoroutine(FlashPantalla(new Color(1f, 0.9f, 0f, 0.5f))); 
        }
    }

    private void ActualizarBotonesPoderes()
    {
        if (textoEnergia != null) textoEnergia.text = "Movimientos/Energía: " + energiaJugador;
        
        if (btnSaltarTurno != null) btnSaltarTurno.interactable = (energiaJugador >= costoSaltar);
        if (btnCongelar != null) btnCongelar.interactable = (energiaJugador >= costoCongelar);
        if (btnRevolver != null) btnRevolver.interactable = (energiaJugador >= costoRevolver);
    }

    private IEnumerator FlashPantalla(Color colorFlash)
    {
        if (panelFondo == null) yield break;
        Color colorOriginal = panelFondo.color; 
        panelFondo.color = colorFlash;
        yield return new WaitForSeconds(0.15f);
        panelFondo.color = colorOriginal;
    }

    // --- FUNCIONALIDAD 3: IA DEL ROBOT ---
    private IEnumerator RobotTurnRoutine()
    {
        Debug.Log("🤖 Turno del Robot. Revisando memoria...");
        yield return new WaitForSeconds(1.0f);
        memoriaRobot.RemoveAll(c => c.isFlipped == true && c != firstCardRevealed && c != secondCardRevealed);
        CardVisual botCard1 = null;
        CardVisual botCard2 = null;

        for (int i = 0; i < memoriaRobot.Count; i++)
        {
            for (int j = i + 1; j < memoriaRobot.Count; j++)
            {
                if (memoriaRobot[i].cardData.pairID == memoriaRobot[j].cardData.pairID && !memoriaRobot[i].isFlipped && !memoriaRobot[j].isFlipped)
                {
                    botCard1 = memoriaRobot[i];
                    botCard2 = memoriaRobot[j];
                    break;
                }
            }
            if (botCard1 != null) break;
        }

        List<CardVisual> cartasDisponibles = new List<CardVisual>();
        foreach (CardVisual card in allCardsOnBoard)
        {
            if (!card.isFlipped) cartasDisponibles.Add(card);
        }

        if (cartasDisponibles.Count < 2) yield break; 

        if (botCard1 == null)
        {
            int randomIndex1 = Random.Range(0, cartasDisponibles.Count);
            botCard1 = cartasDisponibles[randomIndex1];
        }

        botCard1.FlipCard();
        botCard1.GetComponent<CardAnimator>().AnimarPopUp();
        if (!memoriaRobot.Contains(botCard1)) memoriaRobot.Add(botCard1);
        
        yield return new WaitForSeconds(1.0f);

        if (botCard2 == null)
        {
            foreach (CardVisual conocida in memoriaRobot)
            {
                if (conocida.cardData.pairID == botCard1.cardData.pairID && conocida != botCard1 && !conocida.isFlipped)
                {
                    botCard2 = conocida; 
                    break;
                }
            }
            if (botCard2 == null)
            {
                cartasDisponibles.Remove(botCard1);
                int randomIndex2 = Random.Range(0, cartasDisponibles.Count);
                botCard2 = cartasDisponibles[randomIndex2];
            }
        }

        botCard2.FlipCard();
        botCard2.GetComponent<CardAnimator>().AnimarPopUp();
        if (!memoriaRobot.Contains(botCard2)) memoriaRobot.Add(botCard2);
        yield return new WaitForSeconds(2.0f);

        if (botCard1.cardData.pairID == botCard2.cardData.pairID)
        {
            Debug.Log("🤖 ¡El Robot hizo MATCH! Viajando al Salón de la Fama.");
            
            botCard1.GetComponent<CardAnimator>().AnimarViajeAlSofa(panelParesColumna);
            botCard2.GetComponent<CardAnimator>().AnimarViajeAlSofa(panelParesColumna);
            
            StartCoroutine(RobotTurnRoutine());
            yield break;
        }
        else
        {
            botCard1.GetComponent<CardAnimator>().AnimarRegreso();
            botCard2.GetComponent<CardAnimator>().AnimarRegreso();
            botCard1.UnflipCard();
            botCard2.UnflipCard();
        }
        canPlayerPlay = true;
    }
}