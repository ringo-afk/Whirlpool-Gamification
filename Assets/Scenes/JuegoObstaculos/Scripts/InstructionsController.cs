using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameSceneRoadRush");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuRoadRush");
    }
}