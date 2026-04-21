    using System;
    using Unity.Mathematics;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accel = 15f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float friction = 0.9f;
        [SerializeField] private float grip = 3f;
        [SerializeField] private float driftQuotient = 2f;
        [SerializeField] private float turnSpeed = 120f;
        private int directionMult;
        void Start()
        {
            transform.eulerAngles = new Vector3(0,0,270);
        }

        // Update is called once per frame
        void Update()
        {
            //float velocityAngle = Mathf.Rad2Deg*(Mathf.Deg2Rad*450 + (float)Math.Atan2(-rb.linearVelocityX,rb.linearVelocityY)) % (Mathf.PI * 2);
            if(Vector2.Dot(rb.linearVelocity.normalized, transform.up) < 0)
        {
            directionMult = -1;
        }
        else
        {
            directionMult = 1;
        }
            if (Keyboard.current.spaceKey.isPressed)
            {
                rb.linearVelocityX *= Mathf.Lerp(0.998f,0.998f-(0.998f-friction)/driftQuotient,Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocityY *= Mathf.Lerp(0.998f,0.998f-(0.998f-friction)/driftQuotient,Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity.normalized, transform.up*directionMult, (grip/driftQuotient)*Time.deltaTime).normalized * rb.linearVelocity.magnitude;
            }
            else
            {
                rb.linearVelocityX *= Mathf.Lerp(0.998f,friction,Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocityY *= Mathf.Lerp(0.998f,friction,Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity.normalized, transform.up*directionMult, grip*Time.deltaTime).normalized * rb.linearVelocity.magnitude;
            }
            

            //Debug.Log(Mathf.Abs(Mathf.Sin(velocityAngle - ((Mathf.PI / 180) * (90 + transform.eulerAngles.z)))));
            if (Keyboard.current.upArrowKey.isPressed)
            {
                //rb.linearVelocityX += (float)(accel*Math.Cos((Math.PI / 180) * (90 + transform.eulerAngles.z)));
                //rb.linearVelocityY += (float)(accel*Math.Sin((Math.PI / 180) * (90 + transform.eulerAngles.z)));
                rb.linearVelocity += (Vector2)transform.up * accel * Time.deltaTime;
            }
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                transform.eulerAngles += new Vector3(0,0,turnSpeed * Time.deltaTime);
            }
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                transform.eulerAngles -= new Vector3(0,0,turnSpeed * Time.deltaTime);
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                /*if(Vector2.Dot(rb.linearVelocity.normalized, transform.right) > 0)
                {
                    rb.linearVelocityX *= Mathf.Lerp(0.998f,friction,Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                    rb.linearVelocityY *= Mathf.Lerp(0.998f,friction,Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                }
                else
                {
                    rb.linearVelocityX -= (float)((accel/3)*Math.Cos((Math.PI / 180) * (90 + transform.eulerAngles.z)));
                    rb.linearVelocityY -= (float)((accel/3)*Math.Sin((Math.PI / 180) * (90 + transform.eulerAngles.z)));
                }*/
                rb.linearVelocity -= (Vector2)transform.up * (accel/3) * Time.deltaTime;
            }
        }
    }
