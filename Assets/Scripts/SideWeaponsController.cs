using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWeaponsController : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        float rotationInput = 0;

        if (Input.GetKey(KeyCode.Q)) rotationInput = -1;
        if (Input.GetKey(KeyCode.E)) rotationInput = 1;

        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
    }
}
