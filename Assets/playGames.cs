using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class playGames : MonoBehaviour
{
    public string sceneName;
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
        SceneManager.LoadScene(sceneName);
    }
}
