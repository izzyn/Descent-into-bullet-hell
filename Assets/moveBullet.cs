using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBullet : MonoBehaviour
{
    public float movementSpeedMultiplier;
    float growthRate;
    public bool stopGrowingpls;
    public GameObject gunOrigin;
    public float offset;
    public Vector3 startSize;
    GameObject spawnLocation;
    public bool shootsWhenDestroyed;
    public bool shootsWhileTraveling;
    //[HideInInspector]
    public BulletSimple bulletInfo;
    public BulletSimple dieProperties;
    public bool shootsWhenDie;
    public bossAttack sourceScript;
    // Start is called before the first frame update
    void Start()
    {
        startSize = gameObject.transform.localScale;
        growthRate = 0.1f * movementSpeedMultiplier;
        if(!bulletInfo.incrementalGrowth)
        {
            if(!bulletInfo.beam)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * bulletInfo.scaleX, gameObject.transform.localScale.y * bulletInfo.scaleY);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(startSize.x, gameObject.transform.localScale.y * bulletInfo.scaleY);
            }
        }
        else
        {
            if (!bulletInfo.beam)
            {
                gameObject.transform.localScale = new Vector2((gameObject.transform.localScale.x * bulletInfo.scaleX)/10, (gameObject.transform.localScale.y * bulletInfo.scaleY)/10);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(startSize.x, (gameObject.transform.localScale.y * bulletInfo.scaleY)/10);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(bulletInfo.lockedToGun && gunOrigin != null)
        {
            if(gunOrigin.transform.Find("bulletSpawn") != null)
            {
                if (gunOrigin.GetComponent<SpriteRenderer>().flipY)
                {
                    spawnLocation = gunOrigin.transform.Find("bulletSpawnFlipped").gameObject;
                }
                else
                {
                    spawnLocation = gunOrigin.transform.Find("bulletSpawn").gameObject;
                }
            }
            else
            {
                spawnLocation = gunOrigin.gameObject;
            }
            gameObject.transform.position = spawnLocation.transform.position;
            gameObject.transform.rotation = spawnLocation.transform.rotation;
            gameObject.transform.Rotate(0, 0, offset);
        }
    }
    private void FixedUpdate()
    {
        if(bulletInfo.beam)
        {
            var saveSize = gameObject.GetComponent<SpriteRenderer>().size;
            if (!stopGrowingpls)
            {
                saveSize.x += growthRate;
            }
            if(bulletInfo.beam)
            {
                if(saveSize.y < startSize.y * bulletInfo.scaleY)
                {
                        saveSize.y += ((saveSize.y * bulletInfo.scaleY) / 10) * bulletInfo.growthMultiplier;
                }
            }
            gameObject.GetComponent<SpriteRenderer>().size = saveSize;

        }
        else
        {
            if (bulletInfo.incrementalGrowth)
            {
                var saveSize = gameObject.transform.localScale;
                if (saveSize.y < startSize.y * bulletInfo.scaleY)
                {
                    saveSize.y += ((gameObject.transform.localScale.y * bulletInfo.scaleY) / 10) * bulletInfo.growthMultiplier;
                }
                if (saveSize.x < startSize.x * bulletInfo.scaleX)
                {
                    saveSize.x += ((gameObject.transform.localScale.x * bulletInfo.scaleX) / 10) * bulletInfo.growthMultiplier;
                }
                gameObject.transform.localScale = saveSize;
            }
            gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * (movementSpeedMultiplier*4);
        }
        growthRate = growthRate + (0.1f * (movementSpeedMultiplier/5));
    }
}
