using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {

    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    [SerializeField] private float minX;

    [SerializeField] private float minY;

    public Transform target;

    private Vector3 vel = Vector3.zero;

    private void FixedUpdate() {
        Vector3 targetPosition = target.position + offset;
        float clampedX = Mathf.Clamp(targetPosition.x, minX, float.MaxValue);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, float.MaxValue);
        Vector3 finalTarget = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, finalTarget, ref vel, damping);
    }
}