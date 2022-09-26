using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBarScript : MonoBehaviour
{
    float startHP;
    // Start is called before the first frame update
    void Start()
    {
        startHP = GameObject.FindGameObjectWithTag("mainBoss").GetComponent<BossHp>().health; 
    }
    public void UpdateScale()
    {
        float percent = startHP/ GameObject.FindGameObjectWithTag("mainBoss").GetComponent<BossHp>().health;
        this.gameObject.transform.localScale = new Vector3(1, 1 / percent, 1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
