using UnityEngine;
using UnityEngine.SceneManagement;
 Minijuego-debugging


main
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
    Minijuego-debugging

    public void DebuggingRace()
    {
        SceneManager.LoadScene("DebuggingRace_Instrucciones");
    }
}

}
main
