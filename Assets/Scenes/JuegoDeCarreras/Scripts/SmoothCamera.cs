using UnityEngine;

public class SmoothCamera : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;

    private Vector3 offset;

    public Transform target;
    public float damping = 0;

    private Vector3 vel = Vector3.zero;

    private void Update()
    {
        offset = new Vector3(rb.linearVelocityX, rb.linearVelocityY, 0) / 10f;
    }
    private void FixedUpdate() {
        transform.position =  Vector3.SmoothDamp(new Vector3(target.position.x,target.position.y,-10), new Vector3(target.position.x,target.position.y,-10) + offset, ref vel, damping);
    }
}