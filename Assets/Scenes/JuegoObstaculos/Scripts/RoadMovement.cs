using Unity.VisualScripting;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    public float velocity;
    public float roadHeight;
    public float resetY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * velocity;

        if(transform.position.y < resetY)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + roadHeight * 2,
                transform.position.z
            );
        }
    }
}
