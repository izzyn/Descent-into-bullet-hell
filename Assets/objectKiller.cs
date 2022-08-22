using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectKiller : MonoBehaviour
{
    // Start is called before the first frame update
    public enum kil{
        up,
        down,
        left,
        right,
        all
    }
    public kil direction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(direction)
        {
            case kil.up:
                {
                    if(collision.transform.right.y > 0)
                    {
                        kill(collision.gameObject);
                    }
                    break;
                }
            case kil.down:
                {
                    if (collision.transform.right.y < 0)
                    {
                        kill(collision.gameObject);
                    }
                    break;
                }
            case kil.left:
                {
                    if (collision.transform.right.x < 0)
                    {
                        kill(collision.gameObject);
                    }
                    break;
                }
            case kil.right:
                {
                    if (collision.transform.right.x > 0)
                    {
                        kill(collision.gameObject);
                    }
                    break;
                }
            case kil.all:
                {
                    if(!collision.GetComponent<moveBullet>().isBeam)
                    {
                        Destroy(collision.gameObject);
                    }
                    break;
                }
        }
    }
    public static void kill(GameObject collision)
    {
        if (collision.GetComponent<moveBullet>().isBeam)
        {
            //collision.GetComponent<moveBullet>().stopGrowingpls = true;
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
