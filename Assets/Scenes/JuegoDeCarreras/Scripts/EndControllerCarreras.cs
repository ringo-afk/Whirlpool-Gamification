using System;
using TMPro;
using UnityEngine;

public class EndControllerCarreras : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI km;
    void Start()
    {
        km.text = PlayerPrefs.GetInt("Kilometros").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
