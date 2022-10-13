using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    bool menuOpen = false;
    [SerializeField]
    GameObject activatedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen)
            {
                menuOpen = false;
                Time.timeScale = 1;
                activatedObject.SetActive(false);
            }
            else
            {
                print("hi");
                menuOpen = true;
                Time.timeScale = 0.0f;
                activatedObject.SetActive(true);
            }
        }
    }
}
