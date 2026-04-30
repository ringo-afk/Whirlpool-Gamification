using UnityEngine;
using TMPro;

public class UIController1 : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public void UpdateTimerText(int time)
    {
        timeText.text = "Time:" + time;
    }
}