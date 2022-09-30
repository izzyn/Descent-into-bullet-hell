using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossStart : MonoBehaviour
{
    public string bossScene;
    public GameObject objectReferance;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(selectDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void selectDifficulty()
    {
        if(objectReferance.activeInHierarchy == false)
        {
            objectReferance.SetActive(true);
            foreach (var item in GameObject.FindGameObjectsWithTag("difficultyButton"))
            {
                if (item.GetComponent<LevelData>() != null)
                {
                    item.GetComponent<LevelData>().bossSelectonScene = bossScene;
                }
            }
        }
        else
        {
            objectReferance.SetActive(false);
        }
    }
}
