using System.Text.RegularExpressions;
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

    public void JuegoEnsamblaje()
    {
        SceneManager.LoadScene("Reglas");
    }
    public void CyberMatch(){
        SceneManager.LoadScene("JuegoMemoramaRediseño");

    }
}
