using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Predkosc poruszania sie
    public float rotationSpeed = 100f; // Predkosc obracania sie
    public Transform cameraTransform; // Transform kamery
    public float rotateCooldown = 1f; // Czas cooldownu w sekundach
    private float lastRotateTime = -Mathf.Infinity;
    private CharacterController characterController;

    private float defaultMoveSpeed;
    private float defaultRotationSpeed;

    private int slowZones = 0; // Licznik plam spowalniajacych gracza
    private bool isRotating = false;

    [SerializeField] AudioClip skillSFX;
    
    [SerializeField] private Animator animator;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Zapisujemy domyslne wartosci predkosci przy starcie gry
        defaultMoveSpeed = moveSpeed;
        defaultRotationSpeed = rotationSpeed;
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical"); // W/S

        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        moveDirection.y = 0f;

        if (moveDirection.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
            moveDirection.Normalize();
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void Rotate()
    {
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            rotationInput = 1f;
        }

        if (rotationInput != 0f)
        {
            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
        }

        // Obrot specjalny po klawiszu
        if (Time.time - lastRotateTime >= rotateCooldown && !isRotating)
        {
            if (Input.GetKeyDown(KeyCode.V)) // Lewo 90�
            {
                SkillRotate(-90f);
            }
            else if (Input.GetKeyDown(KeyCode.B)) // Prawo 90�
            {
                SkillRotate(90f);
            }
            else if (Input.GetKeyDown(KeyCode.Space)) // 180�
            {
                SkillRotate(180f);
            }
        }
}

    private void SkillRotate(float value)
    {
        animator.SetTrigger("QuickRotate");
        GetComponent<AudioSource>().PlayOneShot(skillSFX);
        StartCoroutine(SmoothRotate(value));
        lastRotateTime = Time.time;
    }

    public void ModifySpeed(float moveMultiplier, float rotationMultiplier)
    {
        slowZones++; // Zwiekszamy licznik aktywnych stref spowolnienia
        moveSpeed = defaultMoveSpeed * moveMultiplier;
        rotationSpeed = defaultRotationSpeed * rotationMultiplier;
    }

    public void RemoveSlow()
    {
        slowZones = Mathf.Max(0, slowZones - 1); // Zmniejszamy licznik, ale nie schodzimy ponizej zera
        if (slowZones == 0)
        {
            ResetSpeed(); // Przywroc predkosc tylko gdy gracz nie znajduje sie juz w zadnej strefie
        }
    }

    public void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        rotationSpeed = defaultRotationSpeed;
    }

    IEnumerator SmoothRotate(float angle)
    {
        if (isRotating) yield break;

        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, angle, 0f);

        float duration = 1f; // Czas obrotu
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
    }
}


