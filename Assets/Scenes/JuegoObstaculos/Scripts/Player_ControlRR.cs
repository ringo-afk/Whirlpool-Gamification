using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_ControlRR : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rig;
    public SpriteRenderer sr;
    // Rotacion
    public float maxRotation = 20f; // inclinación máxima
    public float rotationSpeed = 5f; // suavidad
    // Movimiento
    private float xInput;
    private float yInput;
    // Disparos
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireRate = 0.3f;
    private bool canShoot = false;

    // Carro resbaloso
    public float slipperyMult = 3f;
    public bool isSlippery = false;


    // Funciones para aumentar velocidad
    public void SetSlippery(float duration)
    {
        StopCoroutine("SlipperyEffect");
        StartCoroutine(SlipperyEffect(duration));
    }

    IEnumerator SlipperyEffect(float duration)
    {
        isSlippery = true;

        yield return new WaitForSeconds(duration);

        isSlippery = false;
    }


    // Funciones de power-up disparo destructivo
    public void ActivateShooting(float duration)
    {
        StopCoroutine("ShootingPower");
        StartCoroutine(ShootingPower(duration));
    }

    IEnumerator ShootingPower(float duration)
    {
        canShoot = true;
        StartCoroutine(AutoShoot());
        yield return new WaitForSeconds(duration);

        canShoot = false;
    }

    IEnumerator AutoShoot()
    {
        while (canShoot)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(fireRate);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xInput = 0f;
        yInput = 0f;

        if(Keyboard.current.leftArrowKey.isPressed) // Izquierda
            xInput = -1f;
        if(Keyboard.current.rightArrowKey.isPressed) // Derecha
            xInput = 1f;
        if(Keyboard.current.upArrowKey.isPressed) // Arriba
            yInput = 1f;
        if(Keyboard.current.downArrowKey.isPressed) // Abajo
            yInput = -1f;
        

        
        float targetRotation = -xInput * maxRotation; // Rotacion de carro
        float currentZ = transform.rotation.eulerAngles.z;
        if (currentZ > 180) // Debido a que unity toma 360 grados
            currentZ -= 360;

        float newZ = Mathf.Lerp(currentZ, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, newZ);


    }

    public void FixedUpdate()
    {
        float currentSpeed = moveSpeed;
        
        if(isSlippery)
            currentSpeed = moveSpeed * slipperyMult;

        rig.linearVelocity = new Vector2(xInput * currentSpeed, yInput * currentSpeed);

    }
}
