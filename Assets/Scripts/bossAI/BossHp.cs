using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp : MonoBehaviour
{
    public float health;

    IEnumerator ReduceHealth()
    {
        while (health != 0) //0 check
        {
            yield return new WaitForSeconds(0.1f); //reduces HP every second
            health -= 0.1f;
            var bossAttack = this.gameObject.GetComponent<bossAttack>(); //checks for phase update.
            bossAttack.updatePhase(health, bossAttack.phaseAttacksList);
            GameObject.Find("helthRed").GetComponent<healthBarScript>().UpdateScale();
            if(health < 0)
            {
                health = 0;
                bossAttack.updatePhase(0, bossAttack.phaseAttacksList);
                GameObject.Find("helthRed").GetComponent<healthBarScript>().UpdateScale();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReduceHealth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
