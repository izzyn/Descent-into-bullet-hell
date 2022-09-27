using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referanceInactive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform button in GameObject.Find("buttonPanel").transform)
        {
            button.gameObject.GetComponent<bossStart>().objectReferance = this.gameObject;
        }
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
