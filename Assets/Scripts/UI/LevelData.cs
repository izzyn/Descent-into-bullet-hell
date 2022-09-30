using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    [System.Serializable]
    public enum buttonType
    {
        Heaven,
        Hell,
        Extra,
        none
    }
    public buttonType typeOfButton;
    public string bossSelectonScene;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(PlayGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayGame()
    {
        Debug.Log("Hewwo");
        if(GameObject.Find("ButtonManager") == null)
        {
            if(typeOfButton != buttonType.none)
            {
                GameObject.Find("DataStorage").GetComponent<bossFightData>().selectionScene = typeOfButton;
            }
        }
        SceneManager.LoadScene(bossSelectonScene);

    }
}
