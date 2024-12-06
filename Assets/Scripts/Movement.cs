using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // Sterowanie prawo/lewo
        float moveZ = Input.GetAxis("Vertical");   // Sterowanie przód/ty³

        Vector3 move = new Vector3(moveX, 0, moveZ);
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}
