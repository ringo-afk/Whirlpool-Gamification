using System;
using TMPro;
using UnityEngine;

public class EndControllerCarreras : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI km;
    void Start()
    {
        km.text = PlayerPrefs.GetInt("Kilometros").ToString();
        km.text = PlayerPrefs.GetInt("Monedas").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
