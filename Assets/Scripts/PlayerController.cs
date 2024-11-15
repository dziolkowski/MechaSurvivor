using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Prêdkoœæ poruszania siê
    public float rotationSpeed = 100f; // Prêdkoœæ obracania siê
    public Transform cameraTransform; // Transform kamery
    private CharacterController characterController;

    void Start()
    {
        // Pobranie komponentu CharacterController
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        // Pobranie wejœcia z klawiatury dla ruchu
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical"); // W/S

        // Kierunek ruchu wzglêdem kamery
        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        moveDirection.y = 0f; // Wyeliminowanie ruchu w osi Y

        // Normalizacja wektora kierunku i poruszanie siê
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    void Rotate()
    {
        // Pobranie wejœcia z klawiatury dla obrotu
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationInput = 1f;
        }

        // Obracanie postaci
        if (rotationInput != 0f)
        {
            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
        }
    }
}

