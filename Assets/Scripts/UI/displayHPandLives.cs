using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayHPandLives : MonoBehaviour
{
    public GameObject cloneObject;
    public Sprite hpFull;
    public Sprite hpEmpty;
    public Sprite Lives;
    List<GameObject> HPList = new List<GameObject>();
    List<GameObject> LivesList = new List<GameObject>();
    double HPLeft;
    double LivesLeft;
    // Start is called before the first frame update
    void Start()
    {
        hpAndLives.playerLoaded += setHPLives;
    }
    public void setHPLives()
    {
        double HPRows = System.Math.Ceiling(GameObject.FindGameObjectWithTag("Player").GetComponent<hpAndLives>().HP / 5);
        double newLives = GameObject.FindGameObjectWithTag("Player").GetComponent<hpAndLives>().Lives;
        double LivesRows = System.Math.Ceiling(newLives / 3);
        HPLeft = GameObject.FindGameObjectWithTag("Player").GetComponent<hpAndLives>().HP;
        LivesLeft = GameObject.FindGameObjectWithTag("Player").GetComponent<hpAndLives>().Lives;
        float height = cloneObject.GetComponent<SpriteRenderer>().bounds.size.y;
        int res = Convert.ToInt32(HPRows);
        for (int i = 0; i < HPList.Count; i++)
        {
            GameObject.Destroy(HPList[i]);
        }
        for (int i = 0; i < LivesList.Count; i++)
        {
            GameObject.Destroy(LivesList[i]);
        }

        for (int i = 0; i < HPRows; i++)
        {
            for (int j = 0; j < 5 && HPList.Count < HPLeft; j++)
            {
                GameObject newDisplay = Instantiate(cloneObject);
                HPList.Add(newDisplay);
                newDisplay.GetComponent<SpriteRenderer>().sprite = hpFull;
                float width = newDisplay.GetComponent<SpriteRenderer>().bounds.size.x;
                newDisplay.transform.position = new Vector3(-9.35f + ((0.05f + width) * (j)), 5.374f - ((0.05f + height) * (i)), -5);
            }
        }
        if (LivesLeft > 0)
        {
            for (int i = 0; i < LivesRows; i++)
            {
                for (int j = 0; j < 3 && LivesList.Count < LivesLeft; j++)
                {
                    GameObject newDisplay = Instantiate(cloneObject);
                    LivesList.Add(newDisplay);
                    newDisplay.GetComponent<SpriteRenderer>().sprite = Lives;
                    float width = newDisplay.GetComponent<SpriteRenderer>().bounds.size.x;
                    newDisplay.transform.position = new Vector3(-9.25f + ((0.05f + width) * j), 5.374f - ((0.05f + height) * (i + res)), -5);
                }
            }
        }
        hpAndLives.playerLoaded -= setHPLives;
    }
    public void updateThing()
    {
        double differanceHP = HPLeft - GameObject.FindGameObjectWithTag("Player").GetComponent<hpAndLives>().HP;
        double differanceLives = LivesLeft - GameObject.FindGameObjectWithTag("Player").GetComponent<hpAndLives>().Lives;

        if(differanceHP > HPLeft || differanceLives < 0)
        {
            differanceHP = HPLeft;
        }
        if(differanceLives > LivesLeft || differanceLives < 0)
        {
            differanceLives = LivesLeft;
        }
        Debug.Log(differanceHP);
        Debug.Log(differanceLives);

        for (int i = 0; i < HPList.Count; i++)
        {
            HPList[i].GetComponent<SpriteRenderer>().sprite = hpFull;
        }
        if(differanceHP > 0)
        {
            for (int i = 0; i < differanceHP; i++)
            {
                HPList[(HPList.Count - 1) - (i)].GetComponent<SpriteRenderer>().sprite = hpEmpty;
            }
        }
        for (int i = 0; i < LivesList.Count; i++)
        {
            LivesList[i].GetComponent<SpriteRenderer>().enabled = true;
        }
        if(differanceLives > 0)
        {
            for (int i = 0; i < differanceLives; i++)
            {
                LivesList[(LivesList.Count - 1) - (i)].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
