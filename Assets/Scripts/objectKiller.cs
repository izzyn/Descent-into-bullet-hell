using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectKiller : MonoBehaviour
{
    private bossAttack instansiatedBossAttack;
    // Start is called before the first frame update
    public enum kil{
        up,
        down,
        left,
        right,
        all,
        none
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
                    if(!collision.GetComponent<moveBullet>().bulletInfo.beam)
                    {
                        Destroy(collision.gameObject);
                    }
                    break;
                }
        }
    }
    public void kill(GameObject collision)
    {
        if(collision.GetComponent<moveBullet>().bulletInfo.diesOnWallHit)
        {
            if (collision.GetComponent<moveBullet>().bulletInfo.beam)
            {
                //collision.GetComponent<moveBullet>().stopGrowingpls = true;
            }
            else
            {
                if (collision.GetComponent<moveBullet>() != null)
                {
                    if (collision.GetComponent<moveBullet>().shootsWhenDie && collision.GetComponent<moveBullet>().dieProperties != null)
                    {
                        shatterDie(collision.GetComponent<moveBullet>().sourceScript, collision);
                    }
                }
                Destroy(collision.gameObject);
            }

        }
        else if(collision.GetComponent<moveBullet>().bulletInfo.rickRocketSettings.rickRocketsOnWallHit)
        {
            collision.transform.Rotate(0, 0, 180);
            collision.GetComponent<moveBullet>().bulletInfo.rickRocketSettings.timesItBounces--;
            if(collision.GetComponent<moveBullet>().bulletInfo.rickRocketSettings.timesItBounces == 0)
            {
                collision.GetComponent<moveBullet>().bulletInfo.diesOnWallHit = true;
            }

        }
    }
    public static void shatterDie(bossAttack source, GameObject collision, bool diedEarly = false)
    {
        GameObject ghostObject = new GameObject("killSpawn");
        ghostObject.AddComponent<SpriteRenderer>();
        ghostObject.transform.position = collision.transform.position;
        ghostObject.transform.rotation = collision.transform.rotation;
        ghostObject.tag = "gun";
        if (!diedEarly)
        {
            ghostObject.transform.Rotate(0, 0, 180);
        }
        source.StartCoroutine(source.spawnBullets(collision.GetComponent<moveBullet>().dieProperties, ghostObject.gameObject));
    }
}
