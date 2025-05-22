using UnityEngine;

public class RotateAndMove : MonoBehaviour {
    [SerializeField] private bool rotate;
    [SerializeField] private bool moveUpDown;
    public float rotationSpeed = 100f; // Speed of rotation
    public float moveSpeed = 1f; // Speed of upward movement
    public float moveDistance = 0.5f; // Distance to move up and down

    private Vector3 startPosition;

    void Start() {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update() {
        if (rotate == true) {
            // Rotate the object around its Y-axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if (moveUpDown == true) {
            // Move the object up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
