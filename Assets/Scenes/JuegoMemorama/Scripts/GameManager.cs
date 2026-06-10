using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform boardPanel;

    public AudioSource reproductorSFX;     
    public AudioClip sonidoGanar;
    public AudioClip sonidoPerder;

    public List<CardData> allAvailablePairs = new List<CardData>();
    private List<CardData> deckInPlay = new List<CardData>();
    private List<CardVisual> allCardsOnBoard = new List<CardVisual>();

    public List<CardVisual> memoriaRobot = new List<CardVisual>();

    public int energiaJugador = 5;
    public TextMeshProUGUI textoEnergia;
    
    public TextMeshProUGUI textoDificultad;
    
    public TextMeshProUGUI textoNombreJugador;
    public TextMeshProUGUI textoNombreRobot;
    public Color colorTurnoActivo = new Color(0f, 1f, 1f, 1f); 
    public Color colorTurnoInactivo = new Color(0.3f, 0.3f, 0.3f, 1f); 

    public GameObject panelOscurecidoBoton;
    public GameObject panelTextoExplicacion;
    public TextMeshProUGUI textoDeLaExplicacion;
    private CardVisual cartaMatch1Guardada;
    private CardVisual cartaMatch2Guardada;

    public GridLayoutGroup gridLayoutTablero;
    public Button btnSaltarTurno;
    public Button btnCongelar;
    public Button btnRevolver;
    public int costoSaltar = 1;
    public int costoCongelar = 2;
    public int costoRevolver = 2;

    private int paresEncontradosJugador = 0;
    private int paresEncontradosBot = 0;
    private int totalParesPosibles;
    public float tiempoRestante = 0f;

    public TextMeshProUGUI textoTiempoRestante;
    public float tiempoDePartida = 60f;
    public GameObject panelPausa;
    private bool juegoActivo = false;

    public static bool esUnReintento = false;
    public static int dificultadGuardada = 8;
    private int cartasSeleccionadas = 8;
    private string nombreDificultad = "Normal";

    public UnityEngine.UI.Image imgBtnNormal;
    public UnityEngine.UI.Image imgBtnDificil;

    public Color colorNoSeleccionado = Color.white;
    public Color colorSeleccionado = Color.cyan;

    public GameObject panelMenuInicio;
    public GameObject panelInstrucciones;

    private bool saltarTurnoEnemigoActivo = false;

    public Image panelFondo;
    public Transform panelParesColumna;

    public bool canPlayerPlay = true;

    private CardVisual firstCardRevealed;
    private CardVisual secondCardRevealed;

    void Start()
    {
        if (esUnReintento)
        {
            if (panelMenuInicio != null) panelMenuInicio.SetActive(false);
            if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
            if (panelPausa != null) panelPausa.SetActive(false);
            if (panelOscurecidoBoton != null) panelOscurecidoBoton.SetActive(false);
            if (panelTextoExplicacion != null) panelTextoExplicacion.SetActive(false);

            cartasSeleccionadas = dificultadGuardada;
            if (cartasSeleccionadas == 8) nombreDificultad = "Normal";
            else if (cartasSeleccionadas == 12) nombreDificultad = "Difícil";

            esUnReintento = false;
            BotonJugar();
            return;
        }

        if (panelMenuInicio != null) panelMenuInicio.SetActive(true);
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelOscurecidoBoton != null) panelOscurecidoBoton.SetActive(false);
        if (panelTextoExplicacion != null) panelTextoExplicacion.SetActive(false);

        ActualizarColoresBotones();
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

    public void ActualizarIndicadorTurno(bool esTurnoJugador)
    {
        if (textoNombreJugador != null) textoNombreJugador.color = esTurnoJugador ? colorTurnoActivo : colorTurnoInactivo;
        if (textoNombreRobot != null) textoNombreRobot.color = esTurnoJugador ? colorTurnoInactivo : colorTurnoActivo;
    }

    public void SeleccionarNormal()
    {
        cartasSeleccionadas = 8;
        nombreDificultad = "Normal";
        ActualizarColoresBotones();
    }

    public void SeleccionarDificil()
    {
        cartasSeleccionadas = 12;
        nombreDificultad = "Difícil";
        ActualizarColoresBotones();
    }

    private void ActualizarColoresBotones()
    {
        if (imgBtnNormal != null) imgBtnNormal.color = (cartasSeleccionadas == 8) ? colorSeleccionado : colorNoSeleccionado;
        if (imgBtnDificil != null) imgBtnDificil.color = (cartasSeleccionadas == 12) ? colorSeleccionado : colorNoSeleccionado;
    }

    public void BotonJugar()
    {
        dificultadGuardada = cartasSeleccionadas;

        if (panelMenuInicio != null) panelMenuInicio.SetActive(false);
        if (textoDificultad != null) textoDificultad.text = "Nivel: " + nombreDificultad;

        if (gridLayoutTablero != null)
        {
            if (cartasSeleccionadas == 8) gridLayoutTablero.cellSize = new Vector2(95f, 120f);
            else if (cartasSeleccionadas == 12) gridLayoutTablero.cellSize = new Vector2(65f, 85f);
        }

        GenerateBoard(cartasSeleccionadas);
        ActualizarBotonesPoderes();
        canPlayerPlay = true;
        juegoActivo = true;
        ActualizarIndicadorTurno(true); 
    }

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
        foreach (Transform child in boardPanel) Destroy(child.gameObject);
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
        }
        else if (secondCardRevealed == null)
        {
            secondCardRevealed = card;
            canPlayerPlay = false;
            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        yield return new WaitForSeconds(1.8f);

        if (firstCardRevealed.cardData.pairID == secondCardRevealed.cardData.pairID)
        {
            paresEncontradosJugador++;
            RevisarFinDeJuego();
            cartaMatch1Guardada = firstCardRevealed;
            cartaMatch2Guardada = secondCardRevealed;

            if (textoDeLaExplicacion != null)
            {
                string textoProblema = firstCardRevealed.cardData.isProblem ? firstCardRevealed.cardData.cardText : secondCardRevealed.cardData.cardText;
                string textoPrompt = !firstCardRevealed.cardData.isProblem ? firstCardRevealed.cardData.cardText : secondCardRevealed.cardData.cardText;
                int idDelPar = firstCardRevealed.cardData.pairID;
                string textoExplicacion = ObtenerExplicacion(idDelPar);
                
                textoDeLaExplicacion.text = $"<color=#FF5555><b>Problema:</b></color> {textoProblema}\n\n<color=#55FF55><b>Prompt IA:</b></color> {textoPrompt}\n\n<color=#FFFF55><b>¿Por qué es un buen Match?</b></color>\n{textoExplicacion}";
            }

            if (panelOscurecidoBoton != null) panelOscurecidoBoton.SetActive(true);
            if (panelTextoExplicacion != null) panelTextoExplicacion.SetActive(true);

            energiaJugador++;
            ActualizarBotonesPoderes();
            canPlayerPlay = false;
        }
        else
        {
            firstCardRevealed.GetComponent<CardAnimator>().AnimarRegreso();
            secondCardRevealed.GetComponent<CardAnimator>().AnimarRegreso();
            firstCardRevealed.UnflipCard();
            secondCardRevealed.UnflipCard();

            if (saltarTurnoEnemigoActivo)
            {
                saltarTurnoEnemigoActivo = false;
                canPlayerPlay = true;
            }
            else
            {
                ActualizarIndicadorTurno(false); 
                StartCoroutine(RobotTurnRoutine());
            }
        }

        firstCardRevealed = null;
        secondCardRevealed = null;
    }

    public void BotonContinuarTrasMatch()
    {
        if (panelOscurecidoBoton != null) panelOscurecidoBoton.SetActive(false);
        if (panelTextoExplicacion != null) panelTextoExplicacion.SetActive(false);

        if (cartaMatch1Guardada != null) cartaMatch1Guardada.GetComponent<CardAnimator>().AnimarViajeAlSofa(panelParesColumna);
        if (cartaMatch2Guardada != null) cartaMatch2Guardada.GetComponent<CardAnimator>().AnimarViajeAlSofa(panelParesColumna);

        cartaMatch1Guardada = null;
        cartaMatch2Guardada = null;

        canPlayerPlay = true;
    }

    public void ActivarPoderSaltar()
    {
        if (energiaJugador >= costoSaltar && canPlayerPlay)
        {
            energiaJugador -= costoSaltar;
            saltarTurnoEnemigoActivo = true;
            ActualizarBotonesPoderes();
            StartCoroutine(FlashPantalla(new Color(1f, 0f, 0f, 0.5f)));
        }
    }

    public void ActivarPoderCongelar()
    {
        if (energiaJugador >= costoCongelar && canPlayerPlay)
        {
            energiaJugador -= costoCongelar;
            ActualizarBotonesPoderes();
            StartCoroutine(FlashPantalla(new Color(0f, 1f, 1f, 0.5f)));
        }
    }

    public void ActivarPoderRevolver()
    {
        if (energiaJugador >= costoRevolver && canPlayerPlay)
        {
            energiaJugador -= costoRevolver;
            ActualizarBotonesPoderes();

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

            for (int i = 0; i < cartasOcultas.Count; i++) cartasOcultas[i].SetSiblingIndex(indicesDisponibles[i]);

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
        ActualizarIndicadorTurno(true); 
    }

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
        esUnReintento = true;
        dificultadGuardada = cartasSeleccionadas;
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }

    public void CargarEscenaMenu()
    {
        Time.timeScale = 1f;
        esUnReintento = false;
        SceneManager.LoadScene("JuegoMemoramaRediseño");
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        esUnReintento = false;
        SceneManager.LoadScene("2_Menu");
    }

    private void RevisarFinDeJuego(bool seAcaboElTiempo = false)
    {
        if (paresEncontradosJugador + paresEncontradosBot >= totalParesPosibles || seAcaboElTiempo)
        {
            PlayerPrefs.SetFloat("TiempoSobrante", tiempoDePartida);
            PlayerPrefs.SetInt("EnergiaFinal", energiaJugador);
            PlayerPrefs.SetInt("ParesJugador", paresEncontradosJugador);

            bool jugadorGana = (paresEncontradosJugador > paresEncontradosBot && !seAcaboElTiempo);
            StartCoroutine(TransicionFinDeJuego(jugadorGana));
        }
    }

    private IEnumerator TransicionFinDeJuego(bool jugadorGana)
    {
        canPlayerPlay = false;
        juegoActivo = false;

        if (jugadorGana)
        {
            if (reproductorSFX != null && sonidoGanar != null)
            {
                reproductorSFX.PlayOneShot(sonidoGanar);
                yield return new WaitForSeconds(sonidoGanar.length);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }

            SceneManager.LoadScene("Pantalla_Fin");
        }
        else
        {
            if (reproductorSFX != null && sonidoPerder != null)
            {
                reproductorSFX.PlayOneShot(sonidoPerder);
                yield return new WaitForSeconds(sonidoPerder.length);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }

            SceneManager.LoadScene("Pantalla_Perdiste");
        }
    }

    private string ObtenerExplicacion(int pairID)
    {
        switch (pairID)
        {
            case 1:
                return "Asignarle el rol de 'DBA' (Administrador de Base de Datos) le da contexto a la IA para generar una consulta SQL experta y exacta que identifique fallos en el inventario.";
            case 2:
                return "Pedir específicamente 'empatía' y ofrecer una solución accionable (el descuento) transforma una situación de crisis en una oportunidad para retener al cliente.";
            case 3:
                return "Al adjuntar el PDF y pedir solo los 'puntos clave de ganancias y pérdidas', la IA filtra cientos de páginas y extrae directamente la información financiera crítica.";
            case 4:
                return "Al proporcionarle el bloque de código exacto, la IA actúa como un 'linter' avanzado, leyendo la lógica y encontrando el error de sintaxis en segundos.";
            case 5:
                return "Asignar el rol de 'Analista de logística' asegura que el código en Python no sea genérico, sino que aplique modelos matemáticos enfocados a cadenas de suministro y stock.";
            case 6:
                return "El uso de palabras clave como 'técnicas de copywriting' y 'urgencia' obliga a la IA a generar ganchos comerciales persuasivos que invitan al clic inmediato.";
            case 7:
                return "Solicitar un script de automatización (VBA) para tareas repetitivas elimina días enteros de trabajo manual y reduce a cero los errores humanos al consolidar datos.";
            case 8:
                return "Pedirle a la IA que baje el nivel técnico para un 'usuario sin conocimientos' traduce de inmediato la jerga ingenieril compleja en instrucciones digeribles para el consumidor final.";
            case 9:
                return "El enfoque en 'Retorno de Inversión (ROI)' ataca directamente el problema del precio, dándole a tu equipo argumentos lógicos sobre cómo el equipo se pagará solo a largo plazo.";
            default:
                return "¡Excelente uso de la Inteligencia Artificial para resolver un problema de la empresa!";
        }
    }
}