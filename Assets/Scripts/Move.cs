using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //prêdkoœæ poruszania i obracania siê postaci
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    void Update ()
    {
        // poruszanie siê postaci
       float moveVertical = 0f;
       float moveHorizontal = 0f;

       if (Input.GetKey(KeyCode.W)) //przód
       {
           moveVertical = 1f;
       }

       else if (Input.GetKey(KeyCode.S)) //ty³
       {
           moveVertical = -1f;
       }

       if (Input.GetKey(KeyCode.A)) //lewo
       {
           moveHorizontal = -1f;
       }

       else if (Input.GetKey(KeyCode.D)) //prawo
       {
           moveHorizontal = 1f;
       }

       Vector3 moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
       transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

       //obracanie siê postaci
       if (Input.GetKey(KeyCode.Q))
       {
           transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime); //lewo
       }

       else if (Input.GetKey(KeyCode.E))
       {
           transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); //prawo
       }
    }
}