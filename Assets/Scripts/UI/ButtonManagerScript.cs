using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManagerScript : MonoBehaviour
{
    public delegate void CompleteButtons(); 
    public GameObject defaultButton;
    public static event CompleteButtons completeButtons; //Defining event that gets called when the buttons have finished loading in
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in GameObject.Find("buttonPanel").transform) //Resets all buttons by destroying previous buttons
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < GameObject.Find("DataStorage").GetComponent<bossFightData>().bossData.Count; i++) //loads in new buttons
        {
            List<bossdata> shortHandList = GameObject.Find("DataStorage").GetComponent<bossFightData>().bossData;
            if(shortHandList[i].buttonPlace == GameObject.Find("DataStorage").GetComponent<bossFightData>().selectionScene) //Loads in the button information from the data storage
            {
                GameObject newButton = Instantiate(defaultButton); //Creates the new button
                if (shortHandList[i].defaultSprite != null) //Sets the information
                {
                    newButton.GetComponent<Image>().type = Image.Type.Simple;
                    newButton.GetComponent<Image>().sprite = shortHandList[i].defaultSprite;
                }
                newButton.GetComponent<bossStart>().bossScene = shortHandList[i].sceneName;
                newButton.transform.SetParent(GameObject.Find("buttonPanel").transform);
            }
        }
        completeButtons(); //Invokes event when buttons have loaded in
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
