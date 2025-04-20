using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraSprite : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 directionToLookAt = camPos - transform.position;
        Quaternion rot = Quaternion.LookRotation(directionToLookAt);
        Vector3 euler = rot.eulerAngles;
        euler = euler + offset; //apply offset to euler
        transform.eulerAngles = euler;
    }
}
