using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    static public GameControl Instance;
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject panelPausa;
    public TextMeshProUGUI textLaps;
    [NonSerialized] public int monedas;
    public float tiempoInicio;
    public SFXManager sfxManager;
    public UIController uiController;
    public Timer timer;
    [NonSerialized] public FinishLine finishLine;
    [NonSerialized] public Checkpoint checkpoints;
    public LapsText lapsText;
    [NonSerialized] public int Laps;
    public int TotalLaps;
    [NonSerialized] public int checkpointsInt;
    public int checkpointsNeeded;
    

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        panelPausa.SetActive(false);
        Pause(false);
        Instance.monedas = 0;
    }

    public void Pause(bool pause)
    {
        panelPausa.SetActive(pause);
        if (pause)
        {

            Time.timeScale = 0f;
        }
        else
        {
           Time.timeScale = 1f;
        }
        Debug.Log(Time.timeScale);
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("Circuito1");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuCarreras");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause(true);
        }
    }
}