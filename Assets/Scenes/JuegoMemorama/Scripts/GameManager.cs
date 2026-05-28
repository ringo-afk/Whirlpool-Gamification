using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables y UI
    [Header("Configuración del Tablero")]
    public GameObject cardPrefab;
    public Transform boardPanel;

    [Header("Efectos de Sonido")]
    public AudioSource reproductorSFX;     public AudioClip sonidoVoltearCarta;
    public AudioClip sonidoBoton;

    [Header("Base de Datos Interna")]
    public List<CardData> allAvailablePairs = new List<CardData>();
    private List<CardData> deckInPlay = new List<CardData>();
    private List<CardVisual> allCardsOnBoard = new List<CardVisual>();

    [Header("Funcionalidad 3: Memoria del Robot")]
    public List<CardVisual> memoriaRobot = new List<CardVisual>();

    [Header("Funcionalidad 5: Economía y Poderes")]
    public int energiaJugador = 5;
    public TextMeshProUGUI textoEnergia;
    [Header("Textos del HUD")]
    public TextMeshProUGUI textoDificultad;
    [Header("Configuración del Tablero Dinámico")]
    public GridLayoutGroup gridLayoutTablero;
    public Button btnSaltarTurno;
    public Button btnCongelar;
    public Button btnRevolver;
    public int costoSaltar = 1;
    public int costoCongelar = 2;
    public int costoRevolver = 2;

    [Header("Control de Partida")]
    private int paresEncontradosJugador = 0;
    private int paresEncontradosBot = 0;
    private int totalParesPosibles;
    public float tiempoRestante = 0f;

    [Header("Sistema de Tiempo y Pausa")]
    public TextMeshProUGUI textoTiempoRestante;
    public float tiempoDePartida = 60f;
    public GameObject panelPausa;
    private bool juegoActivo = false;

    [Header("Selección de Dificultad")]
    private int cartasSeleccionadas = 8;
    private string nombreDificultad = "Fácil";

    public UnityEngine.UI.Image imgBtnFacil;
    public UnityEngine.UI.Image imgBtnMedio;
    public UnityEngine.UI.Image imgBtnDificil;

    public Color colorNoSeleccionado = Color.white;
    public Color colorSeleccionado = Color.cyan;

    [Header("Pantallas de Menú")]
    public GameObject panelMenuInicio;
    public GameObject panelInstrucciones;

    private bool saltarTurnoEnemigoActivo = false;

    [Header("Efectos Visuales")]
    public Image panelFondo;

    [Header("Salón de la Fama (F4)")]
    public Transform panelParesColumna;

    public bool canPlayerPlay = true;

    [Header("Memoria del Tablero")]
    private CardVisual firstCardRevealed;
    private CardVisual secondCardRevealed;

    #endregion

    #region Inicio y Update
    void Start()
    {
        if (panelMenuInicio != null) panelMenuInicio.SetActive(true);
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    void Update()
    {
        if (juegoActivo && tiempoDePartida > 0f)
        {
            tiempoDePartida -= Time.deltaTime;

            int minutos = Mathf.FloorToInt(tiempoDePartida / 60);
            int segundos = Mathf.FloorToInt(tiempoDePartida % 60);
            if (textoTiempoRestante != null) textoTiempoRestante.text = string.Format("Tiempo: {0:00}:{1:00}", minutos, segundos);

            if (tiempoDePartida <= 0f)
            {
                tiempoDePartida = 0f;
                juegoActivo = false;
                RevisarFinDeJuego(true);
            }
        }
    }
    #endregion

    public void SeleccionarFacil()
    {
        cartasSeleccionadas = 8;
        nombreDificultad = "Fácil";
        ActualizarColoresBotones();
    }

    public void SeleccionarMedio()
    {
        cartasSeleccionadas = 12;
        nombreDificultad = "Medio";
        ActualizarColoresBotones();
    }

    public void SeleccionarDificil()
    {
        cartasSeleccionadas = 16;
        nombreDificultad = "Difícil";
        ActualizarColoresBotones();
    }

    private void ActualizarColoresBotones()
    {
        if (imgBtnFacil != null) imgBtnFacil.color = (cartasSeleccionadas == 8) ? colorSeleccionado : colorNoSeleccionado;
        if (imgBtnMedio != null) imgBtnMedio.color = (cartasSeleccionadas == 12) ? colorSeleccionado : colorNoSeleccionado;
        if (imgBtnDificil != null) imgBtnDificil.color = (cartasSeleccionadas == 16) ? colorSeleccionado : colorNoSeleccionado;
    }

    public void BotonJugar()
    {
        if (panelMenuInicio != null) panelMenuInicio.SetActive(false);

        if (textoDificultad != null)
        {
            textoDificultad.text = "Nivel: " + nombreDificultad;
        }

        if (gridLayoutTablero != null)
        {
            if (cartasSeleccionadas == 8)
            {
                gridLayoutTablero.cellSize = new Vector2(95f, 120f);
            }
            else if (cartasSeleccionadas == 12)
            {
                gridLayoutTablero.cellSize = new Vector2(80f, 100);
            }
            else if (cartasSeleccionadas == 16)
            {
                gridLayoutTablero.cellSize = new Vector2(80f, 70f);
            }
        }

        GenerateBoard(cartasSeleccionadas);
        ActualizarBotonesPoderes();
        canPlayerPlay = true;
        juegoActivo = true;
    }

    #region Selección de Dificultad

    public void BotonAbrirInstrucciones()
    {
        if (panelMenuInicio != null) panelMenuInicio.SetActive(false);
        if (panelInstrucciones != null) panelInstrucciones.SetActive(true);
    }

    public void BotonCerrarInstrucciones()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (panelMenuInicio != null) panelMenuInicio.SetActive(true);
    }

    #endregion
    #region Lógica del Tablero

    public void GenerateBoard(int totalCards)
    {
        int totalPairs = totalCards / 2;
        totalParesPosibles = totalPairs;
        paresEncontradosJugador = 0;
        paresEncontradosBot = 0;
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
            paresEncontradosJugador++;
            RevisarFinDeJuego();

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

    #endregion

    #region Poderes y UI auxiliares

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

    private IEnumerator RobotTurnRoutine()
    {
        Debug.Log("Turno del Robot. Revisando memoria...");
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
            Debug.Log("¡El Robot hizo MATCH!.");
            paresEncontradosBot++;
            RevisarFinDeJuego();

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

    #endregion

    #region Sistema de Tiempo y Pausa

    public void PausarJuego()
    {
        if (panelPausa != null) panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoActivo = false;
    }

    public void ContinuarJuego()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoActivo = true;
    }

    public void ReintentarPartida()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RevisarFinDeJuego(bool seAcaboElTiempo = false)
    {
        if (paresEncontradosJugador + paresEncontradosBot >= totalParesPosibles || seAcaboElTiempo)
        {
            PlayerPrefs.SetFloat("TiempoSobrante", tiempoDePartida);
            PlayerPrefs.SetInt("EnergiaFinal", energiaJugador);
            PlayerPrefs.SetInt("ParesJugador", paresEncontradosJugador);

            if (paresEncontradosJugador > paresEncontradosBot && !seAcaboElTiempo)
            {
                SceneManager.LoadScene("Pantalla_Fin");
            }
            else
            {
                SceneManager.LoadScene("Pantalla_Perdiste");
            }
        }
    }
    #endregion
}