using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDebugging : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    public float minX = -6f;
    public float maxX = 6f;
    public float minY = -4f;
    public float maxY = 4f;

    private float xInput;
    private float yInput;

    private bool isCrashing = false;

    void Update()
    {
        if (isCrashing)
        {
            return;
        }

        xInput = 0f;
        yInput = 0f;

        // Detecta el movimiento del jugador
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            xInput = -1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            xInput = 1f;
        }

        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            yInput = 1f;
        }

        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            yInput = -1f;
        }

        // Evita que el carro se salga de la pista
        float limitedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float limitedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(limitedX, limitedY, transform.position.z);
    }

    void FixedUpdate()
    {
        if (isCrashing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
    }

    public void CrashAnimation()
    {
        if (!isCrashing)
        {
            StartCoroutine(CrashRoutine());
        }
    }

    IEnumerator CrashRoutine()
    {
        isCrashing = true;

        float duration = 0.6f;
        float timer = 0f;
        float startRotation = transform.eulerAngles.z;

        // Hace que el carro gire 360 grados
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newRotation = startRotation + 360f * (timer / duration);
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, startRotation);

        yield return new WaitForSeconds(0.2f);

        isCrashing = false;
    }
}