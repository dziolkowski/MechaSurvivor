using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Predkosc poruszania sie
    public float rotationSpeed = 100f; // Predkosc obracania sie
    public Transform cameraTransform; // Transform kamery
    private CharacterController characterController;
    private float defaultMoveSpeed;
    private float defaultRotationSpeed;
    private bool isSlowed = false; // Flaga, zeby uniknac wielokrotnego spowolnienia

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
            moveDirection.Normalize();
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    void Rotate()
    {
        float rotationInput = 0f;

        // Obrot myszk¹
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

        // Szybkie obroty klawiszami
        if (Input.GetKeyDown(KeyCode.V)) // Lewo 90°
        {
            transform.Rotate(Vector3.up, -90f);
        }
        else if (Input.GetKeyDown(KeyCode.B)) // Prawo 90°
        {
            transform.Rotate(Vector3.up, 90f);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) // 180°
        {
            transform.Rotate(Vector3.up, 180f);
        }
    }

    public void ModifySpeed(float moveMultiplier, float rotationMultiplier)
    {
        if (!isSlowed) // Zapobiegamy wielokrotnemu zmniejszaniu predkosci
        {
            moveSpeed = defaultMoveSpeed * moveMultiplier;
            rotationSpeed = defaultRotationSpeed * rotationMultiplier;
            isSlowed = true;
        }
    }

    public void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        rotationSpeed = defaultRotationSpeed;
        isSlowed = false;
    }
}


