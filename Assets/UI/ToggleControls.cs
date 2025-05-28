using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject; // Object to toggle

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if H key was pressed this frame
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (targetObject != null)
            {
                // Toggle the object's active state
                targetObject.SetActive(!targetObject.activeSelf);
            }
            else
            {
                Debug.LogWarning("No target object assigned to ToggleControls script!");
            }
        }
    }
}
