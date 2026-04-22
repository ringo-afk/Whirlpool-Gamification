using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject panelPausa;
    void Start()
    {
        panelPausa.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause(true);
        }
    }
}