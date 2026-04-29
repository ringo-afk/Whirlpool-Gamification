using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float tiempo;
    [SerializeField] TextMeshProUGUI textTiempo;
    void Start()
    {
        tiempo = GameControl.Instance.tiempoInicio;
    }

    // Update is called once per frame
    void Update()
    {
        tiempo -= Time.deltaTime;
        textTiempo.text = Mathf.FloorToInt((tiempo+1)/60)%99 + ":" + (Mathf.CeilToInt(tiempo)%60).ToString("D2");

        if (tiempo <= 0.0f)
        {
        timerEnded();
        } 
    }

    void timerEnded()
    {
    //SceneManager.LoadScene("Perdiste");
    }
}
