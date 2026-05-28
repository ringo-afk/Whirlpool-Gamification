using TMPro;
using UnityEngine;

public class LapsText : MonoBehaviour
{
    void Start()
    {
        GameControl.Instance.textLaps.text = (GameControl.Instance.Laps + 1) + "/" + GameControl.Instance.TotalLaps;
    }

    public void UpdateLaps()
    {
        GameControl.Instance.textLaps.text = (GameControl.Instance.Laps + 1) + "/" + GameControl.Instance.TotalLaps;
    }
}
