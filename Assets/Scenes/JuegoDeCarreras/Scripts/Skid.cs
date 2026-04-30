using UnityEngine;
using UnityEngine.InputSystem;

public class Skid : MonoBehaviour
{
    [SerializeField] TrailRenderer tr;
    [SerializeField] GameObject Player;

    void Start()
    {
        tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && Player.GetComponent<PlayerMovement>().dead == false)
        {
            tr.emitting = true;
        }
        else
        {
            tr.emitting = false;
        }
    }
}
