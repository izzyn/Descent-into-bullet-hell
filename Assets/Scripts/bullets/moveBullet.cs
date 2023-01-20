using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBullet : MonoBehaviour
{
    public float movementSpeedMultiplier; //Different properties the bullet needs, explained in the boss attack script
    float growthRate;
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
    public int index;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        startSize = gameObject.transform.localScale;
        growthRate = 0.1f * movementSpeedMultiplier;
        if(!bulletInfo.incrementalGrowth)
        {
            if(bulletInfo.beam) //If the bullet is a beam the X should not increase because it increases over time.
            {
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(startSize.x, gameObject.transform.localScale.y * bulletInfo.scaleY);
            }
        }
        else
        {
            if (!bulletInfo.beam) //Same as previous except it sets to the scale lower than it's finished form so that it can increase over time.
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
        if(bulletInfo.lockedToGun && gunOrigin != null) //Null check and checks if the bullet it supposed to be locked to the gun.
        {
            if(gunOrigin.transform.Find("bulletSpawn") != null)
            {
                if (gunOrigin.GetComponent<SpriteRenderer>().flipY) //Checks for where exactly it should be locked to
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
                spawnLocation = gunOrigin.gameObject; //If the bullet doesn't contain the flipped positions, just set it to the gun instead.
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
            if(saveSize.x < startSize.x * 1000)
            {
                saveSize.x += ((saveSize.x * bulletInfo.scaleX) / 10) * bulletInfo.growthMultiplier;
            }
            if (bulletInfo.incrementalGrowth && saveSize.y < startSize.y * bulletInfo.scaleY)
            {
                saveSize.y += ((saveSize.y * bulletInfo.growthMultiplier) / 10) * bulletInfo.growthMultiplier;
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
