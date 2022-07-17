using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagePlayer : MonoBehaviour
{
        public float damage;
        public bool removeWhenInvincible;
        public bool removeWhenHit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<hpAndLives>() != null)
        {
            if(collision.gameObject.GetComponent<hpAndLives>().invincible != true)
            {
                collision.gameObject.GetComponent<hpAndLives>().HP = collision.gameObject.GetComponent<hpAndLives>().HP - damage;
                if(removeWhenHit)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if(removeWhenInvincible)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
