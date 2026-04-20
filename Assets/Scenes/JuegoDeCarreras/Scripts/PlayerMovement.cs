using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accel = 0.01f;
    [SerializeField] private Rigidbody2D rb;
    void Start()
    {
        transform.eulerAngles = new Vector3(0,0,270);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX -= (float)(0.1*Math.Cos((Math.PI / 180) * (90 + transform.eulerAngles.z)));
        rb.linearVelocityY -= (float)(0.1*Math.Sin((Math.PI / 180) * (90 + transform.eulerAngles.z)));
        if (Keyboard.current.upArrowKey.isPressed)
        {
            rb.linearVelocityX += (float)(accel*Math.Cos((Math.PI / 180) * (90 + transform.eulerAngles.z)));
            rb.linearVelocityY += (float)(accel*Math.Sin((Math.PI / 180) * (90 + transform.eulerAngles.z)));
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.eulerAngles -= new Vector3(0,0,1);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.eulerAngles += new Vector3(0,0,1);
        }
    }
}
