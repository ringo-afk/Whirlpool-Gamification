using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    [Header("Físicas del Vehículo")]
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
        transform.eulerAngles = new Vector3(0, 0, 270);
        groundMult = 1f;
        dead = false;

        StartCoroutine(ObtenerYAplicarMejoras());
    }

    private IEnumerator ObtenerYAplicarMejoras()
    {
        string url = $"{GameControl.Instance.apiBaseUrl}usuarios/{GameControl.Instance.usuarioIdActual}/mejoras/equipadas";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new BypassCertificate();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error API: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                ApiResponseMejoras response = JsonUtility.FromJson<ApiResponseMejoras>(jsonResponse);

                if (response != null && response.success)
                {
                    AplicarModificadores(response.modificadores_acumulados);
                }
            }
        }
    }

    private void AplicarModificadores(Modificadores mods)
    {
        maxSpeed += mods.MaxSpeed;
        accel += mods.Accel;
        friction += mods.Friction;
        grip += mods.Grip;
        driftQuotient += mods.DriftQuotient;
        turnSpeed += mods.TurnSpeed;
    }

    void Update()
    {
        if (Time.deltaTime == 0) return;
        rb.angularVelocity *= 0.9f;
        
        directionMult = Vector2.Dot(rb.linearVelocity.normalized, transform.up) < 0 ? -1 : 1;

        if (!dead)
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                rb.linearVelocityX *= Mathf.Lerp(0.998f, friction, (test / driftQuotient) * Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocityY *= Mathf.Lerp(0.998f, friction, (test / driftQuotient) * Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity.normalized, transform.up * directionMult, (grip / driftQuotient) * groundMult * Time.deltaTime).normalized * rb.linearVelocity.magnitude;
            }
            else
            {
                rb.linearVelocityX *= Mathf.Lerp(0.998f, friction, test * Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocityY *= Mathf.Lerp(0.998f, friction, test * Mathf.Abs(Vector2.Dot(rb.linearVelocity.normalized, transform.right)));
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity.normalized, transform.up * directionMult, grip * groundMult * Time.deltaTime).normalized * rb.linearVelocity.magnitude;
            }

            if (Keyboard.current.upArrowKey.isPressed)
            {
                rb.linearVelocity += (Vector2)transform.up * (Keyboard.current.spaceKey.isPressed ? (accel / (driftQuotient / 3)) : accel) * groundMult * Time.deltaTime;
            }
            
            if (Keyboard.current.leftArrowKey.isPressed) transform.eulerAngles += new Vector3(0, 0, turnSpeed * Time.deltaTime);
            if (Keyboard.current.rightArrowKey.isPressed) transform.eulerAngles -= new Vector3(0, 0, turnSpeed * Time.deltaTime);
            if (Keyboard.current.downArrowKey.isPressed) rb.linearVelocity -= (Vector2)transform.up * (accel * groundMult / 3) * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => groundMult = collision.gameObject.CompareTag("Normal Road") ? 0.4f : groundMult;
    private void OnTriggerEnter2D(Collider2D collision) { if (collision.gameObject.CompareTag("Boost")) rb.linearVelocity += (Vector2)transform.up.normalized * 10 * directionMult; }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Normal Road")) groundMult = 1f;
        if (collision.gameObject.CompareTag("Water") && !dead) { dead = true; StartCoroutine(RespawnSequence()); }
    }

    private Vector3 GetClosestRespawnPoint()
    {
        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        Vector3 closest = Vector3.zero;
        float dist = Mathf.Infinity;
        foreach (var point in respawnPoints)
        {
            float d = (point.transform.position - transform.position).sqrMagnitude;
            if (d < dist) { dist = d; closest = point.transform.position; }
        }
        return closest;
    }

    private IEnumerator RespawnSequence()
    {
        while (dead)
        {
            rb.linearVelocity /= 1.005f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (1f * Time.deltaTime));
            if (sr.color.a < 0f)
            {
                dead = false;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
                rb.linearVelocity = Vector2.zero;
                transform.position = GetClosestRespawnPoint();
            }
            yield return null;
        }
    }
}

[System.Serializable]
public class Modificadores
{
    public float MaxSpeed;
    public float Accel;
    public float Friction;
    public float Grip;
    public float DriftQuotient;
    public float TurnSpeed;
}

[System.Serializable]
public class ApiResponseMejoras
{
    public bool success;
    public Modificadores modificadores_acumulados;
}

public class BypassCertificate : CertificateHandler { protected override bool ValidateCertificate(byte[] certificateData) => true; }