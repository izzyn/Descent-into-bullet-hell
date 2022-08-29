using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBullet : MonoBehaviour
{
    public float movementSpeedMultiplier;
    public float growthMultiplier;
    public bool isBeam;
    float growthRate;
    public float scaleUpY;
    public float scaleUpX;
    public bool stopGrowingpls;
    public bool incrementalGrowth;
    public bool lockOnGun;
    public GameObject gunOrigin;
    public float offset;
    public Vector3 startSize;
    GameObject spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        startSize = gameObject.transform.localScale;
        growthRate = 0.1f * movementSpeedMultiplier;
        if(!incrementalGrowth)
        {
            if(!isBeam)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * scaleUpX, gameObject.transform.localScale.y * scaleUpY);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(startSize.x, gameObject.transform.localScale.y * scaleUpY);
            }
        }
        else
        {
            if (!isBeam)
            {
                gameObject.transform.localScale = new Vector2((gameObject.transform.localScale.x * scaleUpX)/10, (gameObject.transform.localScale.y * scaleUpY)/10);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().size = new Vector2(startSize.x, (gameObject.transform.localScale.y * scaleUpY)/10);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(lockOnGun)
        {
            if(gunOrigin.GetComponent<SpriteRenderer>().flipY)
            {
                spawnLocation = gunOrigin.transform.Find("bulletSpawnFlipped").gameObject;
            }
            else
            {
                spawnLocation = gunOrigin.transform.Find("bulletSpawn").gameObject;
            }
            gameObject.transform.position = spawnLocation.transform.position;
            gameObject.transform.rotation = spawnLocation.transform.rotation;
            gameObject.transform.Rotate(0, 0, offset);
        }
    }
    private void FixedUpdate()
    {
        if(isBeam)
        {
            var saveSize = gameObject.GetComponent<SpriteRenderer>().size;
            if (!stopGrowingpls)
            {
                saveSize.x += growthRate;
            }
            if(incrementalGrowth)
            {
                if(saveSize.y < startSize.y * scaleUpY)
                {
                        saveSize.y += ((saveSize.y * scaleUpY) / 10) * growthMultiplier;
                }
            }
            gameObject.GetComponent<SpriteRenderer>().size = saveSize;

        }
        else
        {
            if (incrementalGrowth)
            {
                var saveSize = gameObject.transform.localScale;
                if (saveSize.y < startSize.y * scaleUpY)
                {
                    saveSize.y += ((gameObject.transform.localScale.y * scaleUpY) / 10) * growthMultiplier;
                }
                if (saveSize.x < startSize.x * scaleUpX)
                {
                    saveSize.x += ((gameObject.transform.localScale.x * scaleUpX) / 10) * growthMultiplier;
                }
                gameObject.transform.localScale = saveSize;
            }
            gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * movementSpeedMultiplier;
        }
        growthRate = growthRate + (0.1f * movementSpeedMultiplier);
    }
}
