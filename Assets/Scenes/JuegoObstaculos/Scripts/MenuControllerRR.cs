using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerRR : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("InstruccionesRoadRush");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("2_Menu");
    }
}