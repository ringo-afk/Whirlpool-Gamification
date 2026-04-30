using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public void UpdateTimerText(int time)
    {
        timeText.text = "Time:" + time;
    }
}