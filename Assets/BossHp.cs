using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp : MonoBehaviour
{
    public int health;

    IEnumerator ReduceHealth()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(1);
            var bossAttack = this.gameObject.GetComponent<bossAttack>();
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
