using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu_Controller : MonoBehaviour
{
    public void RoadRush()
    {
        SceneManager.LoadScene("MenuRoadRush");
    }

    public void CyberRace()
    {
        SceneManager.LoadScene("MenuCarreras");
    }

    public void JuegoEnsamblador()
    {
        SceneManager.LoadScene("JuegoEnsamblaje");
    }
}
