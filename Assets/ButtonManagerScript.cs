using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManagerScript : MonoBehaviour
{
    public GameObject defaultButton;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in GameObject.Find("buttonPanel").transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < GameObject.Find("DataStorage").GetComponent<bossFightData>().bossData.Count; i++)
        {
            List<bossdata> shortHandList = GameObject.Find("DataStorage").GetComponent<bossFightData>().bossData;
            if(shortHandList[i].buttonPlace == GameObject.Find("DataStorage").GetComponent<bossFightData>().selectionScene)
            {
                GameObject newButton = Instantiate(defaultButton);
                if (shortHandList[i].defaultSprite != null)
                {
                    newButton.GetComponent<Image>().type = Image.Type.Simple;
                    newButton.GetComponent<Image>().sprite = shortHandList[i].defaultSprite;
                }
                newButton.GetComponent<LevelData>().bossSelectonScene = shortHandList[i].sceneName;
                newButton.transform.SetParent(GameObject.Find("buttonPanel").transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
