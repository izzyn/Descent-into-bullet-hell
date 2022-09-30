using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; 

public class referanceInactive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ButtonManagerScript.completeButtons += setReferance; //sets the referance after all buttons have loaded in
    }
    private void setReferance()
    {
        foreach (Transform button in GameObject.Find("buttonPanel").transform) //for every child of the button panel (aka every button)
        {
            if (button.gameObject.GetComponent<bossStart>() != null)
            {
                button.gameObject.GetComponent<bossStart>().objectReferance = this.gameObject;
            }
        }
        this.gameObject.SetActive(false);
        ButtonManagerScript.completeButtons -= setReferance;
    }
}
