using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossFightData : MonoBehaviour
{
    public List<bossdata> bossData;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(GameObject.FindGameObjectsWithTag("BossDataStore").Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class bossdata
{
    public GameObject boss;
    public GameObject button;

    [System.Serializable]
    public enum selection
    {
        Heaven,
        Hell,
        Extra
    }
    public selection buttonPlace;
}
