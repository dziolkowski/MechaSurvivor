using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenMenu : MonoBehaviour
{
    public GameObject menuPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && menuPopup.activeSelf == false) {
            menuPopup.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuPopup.activeSelf == true) {
            menuPopup.SetActive(false);
        }

    }
}
