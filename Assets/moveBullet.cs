using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBullet : MonoBehaviour
{
    public float movementSpeedMultiplier;
    bool stillInvincible = true;
    public bool isBeam;
    float growthRate;
    public bool stopGrowingpls;
    // Start is called before the first frame update
    void Start()
    {
        growthRate = 0.1f * movementSpeedMultiplier;

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if(isBeam)
        {
            if(!stopGrowingpls)
            {
                var saveSize = gameObject.GetComponent<SpriteRenderer>().size;
                saveSize.x += growthRate;
                gameObject.GetComponent<SpriteRenderer>().size = saveSize;
            }
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * movementSpeedMultiplier;
        }
        growthRate = growthRate + (0.1f * movementSpeedMultiplier);
    }
}
