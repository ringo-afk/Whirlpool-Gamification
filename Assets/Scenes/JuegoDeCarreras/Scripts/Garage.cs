using UnityEngine;
using UnityEngine.SceneManagement;

public class Garage : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("MenuCarreras");
    }
}
