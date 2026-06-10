using System;
using TMPro;
using UnityEngine;

public class EndControllerCarreras : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI km;
    [SerializeField] private TextMeshProUGUI tiempo;
    [SerializeField] private TextMeshProUGUI monedas;
    void Start()
    {
        km.text = PlayerPrefs.GetInt("Kilometros").ToString();
        tiempo.text = PlayerPrefs.GetFloat("Tiempo").ToString();
        monedas.text = PlayerPrefs.GetInt("Monedas").ToString();
    }
}
