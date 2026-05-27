using UnityEngine;

public class ParallaxDebugging : MonoBehaviour
{
    public float speed = 3f;
    public float resetX = -18f;
    public float startX = 18f;

    //Parallax
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= resetX)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }
}
