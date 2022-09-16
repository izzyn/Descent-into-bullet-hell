using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp : MonoBehaviour
{
    public int health;

    IEnumerator ReduceHealth()
    {
        while (health > 0) //0 check
        {
            yield return new WaitForSeconds(1); //reduces HP every second
            var bossAttack = this.gameObject.GetComponent<bossAttack>(); //checks for phase update.
            bossAttack.updatePhase(health, bossAttack.phaseAttacksList);
            health--;
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
