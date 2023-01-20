using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class difficultySelection : MonoBehaviour
{
    public int Lives;
    public int HP;
    public int difficultyIndex;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(difficultyInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void difficultyInfo()
    {
        Time.timeScale = 1;
        bossFightData shorthandInfo = GameObject.Find("DataStorage").GetComponent<bossFightData>();
        shorthandInfo.difficultyIndex = difficultyIndex;
        shorthandInfo.HP = HP;
        shorthandInfo.lives = Lives;
    }
}
