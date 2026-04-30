using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject howTo;

    public void GoToGame()
    {
        SceneManager.LoadScene("Circuito1");
    }

    public void GoToGarage()
    {
        SceneManager.LoadScene("Garage");
    }

    public void HowTo(bool yes)
    {
        howTo.SetActive(yes);
    }
}
