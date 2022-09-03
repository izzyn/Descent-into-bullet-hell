using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAttack: MonoBehaviour
{
    [System.Serializable]
    public struct attackBasic
    {
        public enum attackType //If you wish to add a brand new attack type, do that here:
        {
            shootDown,
            wait,
            summonGun
        }
        public attackType Type; //needed info to initialize an attack
        public float duration;
        public GameObject bullet;
        public gunSpawnInfo spawnInfo; //info used for the "summon gun" attack

    }
    public List<attackBasic> attackBasicList = new List<attackBasic>(); //the list of attacks the boss can have
    public List<int> hpThreshold = new List<int>();
    public GameObject[] test; //Ignore, this is a test list to document the amount of bullets on the screen for stress testing

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pickAttack()); //makes the boss pick attacks when it loads in
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator shootBullets(attackBasic attackInfo) //outdated
    {
        while(true)
        {
            GameObject shootBullet = Instantiate(attackInfo.bullet, transform.position, Quaternion.identity);
            shootBullet.GetComponent<damagePlayer>().damage = 5f;
            shootBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5);
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator spawnGuns(attackBasic attackInfo)
    {
        for(int i = 0; i < attackInfo.spawnInfo.gunInfo.Count; i++)
        {
            GameObject gunSpawned = Instantiate(attackInfo.spawnInfo.gunInfo[i].gun);
            Vector2 savedScale = gunSpawned.transform.localScale;
            attackInfo.spawnInfo.gunInfo[i].bulletConfig.lockedToGun = attackInfo.spawnInfo.gunInfo[i].rotationConfig.lockedToGun;
            gunSpawned.transform.localScale = new Vector2(savedScale.x * attackInfo.spawnInfo.gunInfo[i].gunScaleX, savedScale.y * attackInfo.spawnInfo.gunInfo[i].gunScaleY);
            gunSpawned.transform.position = new Vector3(attackInfo.spawnInfo.gunInfo[i].spawnLocation.transform.position.x, attackInfo.spawnInfo.gunInfo[i].spawnLocation.transform.position.y, -3);
            gunSpawned.GetComponent<SpriteRenderer>().sprite = attackInfo.spawnInfo.gunInfo[i].gunTexture;
            if(gunSpawned.GetComponent<SpriteRenderer>().sharedMaterial == attackInfo.spawnInfo.gunInfo[i].bulletConfig.bullet.GetComponent<SpriteRenderer>().sharedMaterial)
            {
                changeColour(attackInfo.spawnInfo.gunInfo[i].bulletConfig.colourConfig.stage, attackInfo.spawnInfo.gunInfo[i].bulletConfig.colourConfig.Colour, gunSpawned);
            }
            Vector3 newScale = gameObject.transform.localScale;
            if (attackInfo.spawnInfo.gunInfo[i].shootTowardsPlayer != true)
            {
                gunSpawned.transform.Rotate(new Vector3(0, 0, attackInfo.spawnInfo.gunInfo[i].rotation));
                if ((gunSpawned.transform.rotation.eulerAngles.z * Mathf.Rad2Deg) > 90f || (gunSpawned.transform.rotation.eulerAngles.z) < -90f)
                {
                    Debug.Log("Yes");
                    gunSpawned.GetComponent<SpriteRenderer>().flipY = true;
                }
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 target = player.transform.position - gunSpawned.transform.position;
                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                gunSpawned.transform.Rotate(new Vector3(0, 0, angle));
                if (((gunSpawned.transform.rotation.eulerAngles.z) > 90) && ((gunSpawned.transform.rotation.eulerAngles.z) < 270))
                {
                    gunSpawned.GetComponent<SpriteRenderer>().flipY = true;
                }
                else
                {
                    gunSpawned.GetComponent<SpriteRenderer>().flipY = false;
                }
            }
            if(attackInfo.spawnInfo.gunInfo[i].tracksPlayer)
            {
                StartCoroutine(followPlayer(attackInfo.spawnInfo.gunInfo[i], gunSpawned));
            }
            if (attackInfo.spawnInfo.gunInfo[i].rotatesOverTime)
            {
                StartCoroutine(rotateGun(attackInfo.spawnInfo.gunInfo[i], gunSpawned));
            }
            StartCoroutine(spawnBullets(attackInfo.spawnInfo.gunInfo[i].bulletConfig, gunSpawned, attackInfo.spawnInfo.gunInfo[i].shatterConfig));

            yield return new WaitForSeconds(attackInfo.spawnInfo.spawnDelay);
        }
    }
    IEnumerator rotateGun(gunSpawnInfo.spawnedGun attackInfo, GameObject gun)
    {
        yield return new WaitForSeconds(attackInfo.rotationConfig.timeToStart);
        Vector3 saveRotation = gun.transform.rotation.eulerAngles;
        float rotationSteps = 500;
        float degreesPerRotation = (attackInfo.rotationConfig.degrees / rotationSteps);
        if(gun.GetComponent<SpriteRenderer>().flipY)
        {
            degreesPerRotation *= -1;
        }
        while(gun != null)
        {
            for(int i = 0; i < rotationSteps; i++)
            {
                if (gun != null)
                {
                    gun.transform.Rotate(new Vector3(0, 0, degreesPerRotation));
                    yield return new WaitForSeconds(0.01f / attackInfo.rotationConfig.speedMultiplier);
                }
            }
            if(attackInfo.rotationConfig.rotatesBack)
            {
                degreesPerRotation *= -1;
            }
            else
            {
                if(gun != null)
                {
                    Quaternion rotation = Quaternion.Euler(saveRotation);
                    gun.transform.rotation = rotation;
                }
            }
        }
    }
    IEnumerator followPlayer(gunSpawnInfo.spawnedGun attackInfo, GameObject gun)
    {
        while(gun != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 target = player.transform.position - gun.transform.position;
            gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, target);
            gun.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            yield return new WaitForSeconds(attackInfo.trackingAccuracy);
            
        }
    }


    public IEnumerator spawnBullets(BulletSimple attackInfo, GameObject gunSource, shatterShotConfig shatterConfig = null)
    {
        yield return new WaitForSeconds(attackInfo.delayBeforeShooting);
        for (int i = 0; i < attackInfo.bulletAmmount; i++)
        {
            if(gunSource != null)
            {
                if (attackInfo.multiShoot)
                {
                    for(int j = 0; j < attackInfo.multiShootConfig.bulletMultiplier; j++)
                    {
                        float startAngle = (attackInfo.multiShootConfig.angle / 2) * -1;
                        float angleChunks = attackInfo.multiShootConfig.angle / (attackInfo.multiShootConfig.bulletMultiplier-1);

                        GameObject shootBullet = Instantiate(attackInfo.bullet);
                        shootBullet.GetComponent<moveBullet>().offset = (angleChunks*j)+startAngle;
                        if (shatterConfig != null)
                        {
                            createBullet(shootBullet, attackInfo, gunSource, shatterConfig);
                        }
                        else
                        {
                            createBullet(shootBullet, attackInfo, gunSource);
                        }
                        if (attackInfo.beam)
                        {
                            bool kil = false;
                            if(i+1 >= attackInfo.bulletAmmount)
                            {
                                kil = true;
                            }
                            StartCoroutine(destroyBeam(shootBullet, attackInfo.beamConfig.lifespan, gunSource, kil));
                        }
                        Debug.Log(shootBullet.GetComponent<SpriteRenderer>().material.GetColor("_Color"));
                        changeColour(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour, shootBullet);
                        if(attackInfo.colourConfig.colourMode == colourChange.mode.line)
                        {
                            attackInfo.colourConfig.stage = changeColourState(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour.Count);
                        }
                        if (attackInfo.colourConfig.colourMode == colourChange.mode.random)
                        {
                            int newstageL = Random.Range(0, attackInfo.colourConfig.Colour.Count);
                            attackInfo.colourConfig.stage = changeColourState(newstageL, attackInfo.colourConfig.Colour.Count);
                        }
                        test = GameObject.FindGameObjectsWithTag("bullet");
                        Debug.Log(test.Length);
                        float width = gunSource.GetComponent<SpriteRenderer>().bounds.size.x;
                        if (gunSource.transform.Find("bulletSpawn") != null)
                        {
                            if (gunSource.GetComponent<SpriteRenderer>().flipY)
                            {
                                shootBullet.transform.position = placeBullet(gunSource.transform.Find("bulletSpawnFlipped").gameObject);
                            }
                            else
                            {
                                shootBullet.transform.position = placeBullet(gunSource.transform.Find("bulletSpawn").gameObject);
                            }
                        }
                        else
                        {
                            Debug.Log(placeBullet(gunSource));
                            shootBullet.transform.position = placeBullet(gunSource);
                        }
                        shootBullet.transform.rotation = gunSource.transform.rotation;
                        shootBullet.transform.Rotate(new Vector3(0, 0, shootBullet.transform.rotation.z + startAngle + (angleChunks*j)));
                        shootBullet.GetComponent<moveBullet>().movementSpeedMultiplier = attackInfo.bulletSpeedMultiplier;
                        if(shatterConfig != null)
                        {
                            if (shatterConfig.travelShot != null && shatterConfig.shootsWhileTraveling && gunSource != null)
                            {
                                splitShot(shatterConfig.travelShot, shootBullet);
                            }
                        }
                    }
                }
                else
                {
                    GameObject shootBullet = Instantiate(attackInfo.bullet);
                    if(shatterConfig != null)
                    {
                        createBullet(shootBullet, attackInfo, gunSource, shatterConfig);
                    }
                    else
                    {
                        createBullet(shootBullet, attackInfo, gunSource);
                    }
                    if (attackInfo.beam)
                    {
                        bool kil = false;
                        if (i+1 >= attackInfo.bulletAmmount)
                        {
                            kil = true;
                        }
                        StartCoroutine(destroyBeam(shootBullet, attackInfo.beamConfig.lifespan,gunSource, kil));
                    }
                    Debug.Log(shootBullet.GetComponent<SpriteRenderer>().material.GetColor("_Color"));
                    changeColour(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour, shootBullet);
                    test = GameObject.FindGameObjectsWithTag("bullet");
                    Debug.Log(test.Length);
                    float width = gunSource.GetComponent<SpriteRenderer>().bounds.size.x;
                    if(gunSource.transform.Find("bulletSpawn") != null)
                    {
                        if (gunSource.GetComponent<SpriteRenderer>().flipY)
                        {
                            shootBullet.transform.position = placeBullet(gunSource.transform.Find("bulletSpawnFlipped").gameObject);
                        }
                        else
                        {
                            shootBullet.transform.position = placeBullet(gunSource.transform.Find("bulletSpawn").gameObject);
                        }
                    }
                    else
                    {
                        Debug.Log(placeBullet(gunSource));
                        shootBullet.transform.position = placeBullet(gunSource);
                    }
                    shootBullet.transform.rotation = gunSource.transform.rotation;
                    shootBullet.GetComponent<moveBullet>().movementSpeedMultiplier = attackInfo.bulletSpeedMultiplier;
                    attackInfo.colourConfig.stage = changeColourState(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour.Count);
                    if (shatterConfig != null)
                    {
                        if (shatterConfig.travelShot != null && shatterConfig.shootsWhileTraveling && gunSource != null)
                        {
                            splitShot(shatterConfig.travelShot, shootBullet);
                        }
                    }
                }

                yield return new WaitForSeconds(attackInfo.bulletDelay);
                if(attackInfo.colourConfig.colourMode == colourChange.mode.row)
                {
                    attackInfo.colourConfig.stage = changeColourState(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour.Count);
                }
            }
            else
            {
                break;
            }
        }
        if(gunSource != null)
        {
            if (!attackInfo.beam && gunSource.GetComponent<moveBullet>() == null)
            {
                Destroy(gunSource);
            }
        }
    }
    public void splitShot(BulletSimple information, GameObject source)
    {
        StartCoroutine(spawnBullets(information, source));
    }
    public static GameObject createBullet(GameObject shootBullet, BulletSimple attackInfo, GameObject gunSource, shatterShotConfig shatterConfig = null)
    {
        shootBullet.GetComponent<moveBullet>().bulletInfo = attackInfo;
        shootBullet.GetComponent<damagePlayer>().damage = attackInfo.bulletDamage;
        shootBullet.GetComponent<moveBullet>().gunOrigin = gunSource;
        if(shatterConfig != null)
        {
            if(shatterConfig.destoyShoot != null && shatterConfig.shootsWhenDestroyed)
            {
                shootBullet.GetComponent<moveBullet>().shootsWhenDie = shatterConfig.shootsWhenDestroyed;
                shootBullet.GetComponent<moveBullet>().dieProperties = shatterConfig.destoyShoot;
            }
        }
        return shootBullet;
    }
    public static Vector3 placeBullet(GameObject location)
    {
        return new Vector3(location.transform.position.x, location.transform.position.y, -3);
    }
    public int changeColourState(int stage, int count)
    {
        stage++;
        if(stage >= count)
        {
            stage = 0;
        }
        return stage;
    }
    IEnumerator destroyBeam(GameObject shootBullet, float time, GameObject gunSource, bool kil)
    {
        yield return new WaitForSeconds(time);
        Destroy(shootBullet);
        if(kil)
        {
            Destroy(gunSource);
        }
    }
    IEnumerator pickAttack()
    {
        int oldIndex = 0;
        while(true)
        {
            int pickedAttack;
            if (attackBasicList.Count > 1)
            {
                pickedAttack = Random.Range(0, attackBasicList.Count - 1);
                if (pickedAttack >= oldIndex)
                {
                    pickedAttack++;
                }
            }
            else
            {
                pickedAttack = Random.Range(0, attackBasicList.Count);
            }
            oldIndex = pickedAttack;
            IEnumerator attackCoroutine = null;
            switch(attackBasicList[pickedAttack].Type)
            {
                case attackBasic.attackType.shootDown:
                    attackCoroutine = shootBullets(attackBasicList[pickedAttack]);
                    break;
                case attackBasic.attackType.wait:
                    break;
                case attackBasic.attackType.summonGun:
                    attackCoroutine = spawnGuns(attackBasicList[pickedAttack]);
                    break;
                    

            }
            if(attackCoroutine != null)
            {
                StartCoroutine(attackCoroutine);
                yield return new WaitForSeconds(attackBasicList[pickedAttack].duration);
                StopCoroutine(attackCoroutine);
            }
            else
            {
                yield return new WaitForSeconds(attackBasicList[pickedAttack].duration);
            }
        }
    }
    void changeColour(int stage, List<Color> ColourList, GameObject whatToChange)
    {
        whatToChange.GetComponent<SpriteRenderer>().material.SetColor("_Color", ColourList[stage]);

    }
}
[System.Serializable]
public class gunSpawnInfo
{
    [System.Serializable]
    public struct spawnedGun
    {
        public GameObject spawnLocation;
        public float rotation;
        public GameObject gun;
        public Sprite gunTexture;
        public float gunScaleX;
        public float gunScaleY;
        public bool shootTowardsPlayer;
        public bool tracksPlayer;
        public float trackingAccuracy;
        public bool rotatesOverTime;
        public BulletSimple bulletConfig;
        public rotationSettings rotationConfig;
        public shatterShotConfig shatterConfig;
        int stage;
        public enum weaponSpawned
        {
            basicGun
        }
    }
    public float spawnDelay;
    public List<spawnedGun> gunInfo = new List<spawnedGun>();
}
[System.Serializable]
public class multiShootSettings
{
    public int bulletMultiplier;
    public float angle;
}
[System.Serializable]
public class colourChange
{
    public int stage;
    public enum mode
    {
        line,
        row,
        random
    }
    public mode colourMode;
    public List<Color> Colour = new List<Color>();
}
[System.Serializable]
public class beamSettings
{
    public float lifespan;
}
[System.Serializable]
public class rotationSettings
{
    public float timeToStart;
    public float degrees;
    public float speedMultiplier;
    public bool rotatesBack;
    public bool lockedToGun;
}
[System.Serializable]
public class BulletSimple
{
    public float delayBeforeShooting;
    //[HideInInspector]
    public bool lockedToGun;
    public GameObject bullet;
    public float bulletDelay;
    public float bulletAmmount;
    public float bulletDamage;
    public float bulletSpeedMultiplier;
    public float growthMultiplier;
    public float scaleX; //bullet scale (is a multiplier)
    public float scaleY;
    public bool multiShoot;
    public bool incrementalGrowth;
    public bool beam;
    public multiShootSettings multiShootConfig;
    public colourChange colourConfig;
    public beamSettings beamConfig;
}

[System.Serializable]
public class shatterShotConfig
{
    public bool shootsWhileTraveling;
    public BulletSimple travelShot;
    public bool shootsWhenDestroyed;
    public BulletSimple destoyShoot;

}