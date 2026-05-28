using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;
    public static UIController Instance;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        coins.text = "0";
    }

    public void CoinUpdate()
    {
        coins.text = GameControl.Instance.monedas.ToString();
    }
}
