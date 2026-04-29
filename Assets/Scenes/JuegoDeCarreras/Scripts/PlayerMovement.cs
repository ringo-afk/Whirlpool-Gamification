using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accel = 15f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float friction = 0.9f;
    private float groundMult = 1f;
    [SerializeField] private float grip = 3f;
    [SerializeField] private float driftQuotient = 2f;
    [SerializeField] private float turnSpeed = 120f;
    private int directionMult;
    [SerializeField] private float test = 0f;
    public bool dead = false;

    void Start()
    {
        transform.eulerAngles = new Vector3(0,0,270);
        groundMult = 1f;
        dead = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.deltaTime == 0) return;
        rb.angularVelocity *= 0.9f;
        //float velocityAngle = Mathf.Rad2Deg*(Mathf.Deg2Rad*450 + (float)Math.Atan2(-rb.linearVelocityX,rb.linearVelocityY)) % (Mathf.PI * 2);
        if(Vector2.Dot(rb.linearVelocity.normalized, transform.up) < 0)
        {
            directionMult = -1;
        }
        else
        {
            directionMult = 1;
        }

        if(!dead){
            if (Keyboard.current.spaceKey.isPressed)
            {
                rb.linearVelocityX *= Mathf.Lerp(0.998f,friction,(test/driftQuotient)*Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocityY *= Mathf.Lerp(0.998f,friction,(test/driftQuotient)*Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity.normalized, transform.up*directionMult, (grip/driftQuotient) * groundMult *Time.deltaTime).normalized * rb.linearVelocity.magnitude;
            }
            else
            {
                rb.linearVelocityX *= Mathf.Lerp(0.998f,friction,test*Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocityY *= Mathf.Lerp(0.998f,friction,test*Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity.normalized, transform.up*directionMult, grip * groundMult * Time.deltaTime).normalized * rb.linearVelocity.magnitude;
            }
            

            //Debug.Log(Mathf.Abs(Mathf.Sin(velocityAngle - ((Mathf.PI / 180) * (90 + transform.eulerAngles.z)))));
            if (Keyboard.current.upArrowKey.isPressed)
            {
                //rb.linearVelocityX += (float)(accel*Math.Cos((Math.PI / 180) * (90 + transform.eulerAngles.z)));
                //rb.linearVelocityY += (float)(accel*Math.Sin((Math.PI / 180) * (90 + transform.eulerAngles.z)));
                if (Keyboard.current.spaceKey.isPressed)
                {
                    rb.linearVelocity += (Vector2)transform.up * (accel/(driftQuotient/3)) * groundMult * Time.deltaTime;
                }
                else
                {
                    rb.linearVelocity += (Vector2)transform.up * accel * groundMult * Time.deltaTime;
                }
                
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
                rb.linearVelocity -= (Vector2)transform.up * (accel * groundMult/3) * Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Normal Road"))
        {
            groundMult = 0.4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            rb.linearVelocity += (Vector2)transform.up.normalized * 10 * directionMult;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Normal Road"))
        {
            groundMult = 1f;
        }

        if (collision.gameObject.CompareTag("Water") && dead == false)
        {
            dead = true;
            StartCoroutine(RespawnSequence());
        }
    }
        
        private Vector3 GetClosestRespawnPoint()
    {
        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        Vector3 closestPoint = new Vector3();
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject point in respawnPoints)
        {
            float distanceToPoint = (point.transform.position - currentPosition).sqrMagnitude;

            if (distanceToPoint < shortestDistance)
            {
                shortestDistance = distanceToPoint;
                closestPoint = point.transform.position;
            }
        }
        return closestPoint;
    }

    private IEnumerator RespawnSequence()
    {
        while(dead){
            rb.linearVelocity /= 1.005f;
            sr.color = new Color(r: sr.color.r, g: sr.color.g,b: sr.color.b,a: sr.color.a-(1f * Time.deltaTime));
                if(sr.color.a < 0f)
                {
                    dead = false;
                    sr.color = new Color(r: sr.color.r, g: sr.color.g,b: sr.color.b,a: 1f);
                    rb.linearVelocity = new Vector2(0,0);
                    transform.position = GetClosestRespawnPoint();
                }
                yield return null;
        }
        yield return null;
    }
}


