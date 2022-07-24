using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAttack: MonoBehaviour
{
    [System.Serializable]
    public struct attackBasic
    {
        public enum attackType
        {
            shootDown,
            wait,
            summonGun
        }
        public attackType Type;
        public float duration;
        public GameObject bullet;
        public gunSpawnInfo spawnInfo;

    }
    public List<attackBasic> attackBasicList = new List<attackBasic>();
    public List<int> hpThreshold = new List<int>();
    public GameObject[] test;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pickAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator shootBullets(attackBasic attackInfo)
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
            gunSpawned.transform.localScale = new Vector2(savedScale.x * attackInfo.spawnInfo.gunInfo[i].gunScaleX, savedScale.y * attackInfo.spawnInfo.gunInfo[i].gunScaleY);
            gunSpawned.transform.position = new Vector3(attackInfo.spawnInfo.gunInfo[i].spawnLocation.transform.position.x, attackInfo.spawnInfo.gunInfo[i].spawnLocation.transform.position.y, -2);
            gunSpawned.GetComponent<SpriteRenderer>().sprite = attackInfo.spawnInfo.gunInfo[i].gunTexture;
            if(gunSpawned.GetComponent<SpriteRenderer>().sharedMaterial == attackInfo.spawnInfo.gunInfo[i].bullet.GetComponent<SpriteRenderer>().sharedMaterial)
            {
                changeColour(attackInfo.spawnInfo.gunInfo[i].colourConfig.stage, attackInfo.spawnInfo.gunInfo[i].colourConfig.Colour, gunSpawned);
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
            StartCoroutine(spawnBullets(attackInfo.spawnInfo.gunInfo[i], gunSpawned));

            yield return new WaitForSeconds(attackInfo.spawnInfo.spawnDelay);
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
    IEnumerator spawnBullets(gunSpawnInfo.spawnedGun attackInfo, GameObject gunSource)
    {
        for (int i = 0; i < attackInfo.bulletAmmount; i++)
        {
            if (attackInfo.multiShoot)
            {
                for(int j = 0; j < attackInfo.multiShootConfig.bulletMultiplier; j++)
                {
                    float startAngle = (attackInfo.multiShootConfig.angle / 2) * -1;
                    float angleChunks = attackInfo.multiShootConfig.angle / attackInfo.multiShootConfig.bulletMultiplier;

                    GameObject shootBullet = Instantiate(attackInfo.bullet);
                    createBullet(shootBullet, attackInfo);
                    if(attackInfo.beam)
                    {
                        StartCoroutine(destroyBeam(shootBullet, attackInfo.beamConfig.lifespan, gunSource));
                    }
                    Debug.Log(shootBullet.GetComponent<SpriteRenderer>().material.GetColor("_Color"));
                    changeColour(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour, shootBullet);
                    test = GameObject.FindGameObjectsWithTag("bullet");
                    Debug.Log(test.Length);
                    float width = gunSource.GetComponent<SpriteRenderer>().bounds.size.x;
                    if(gunSource.GetComponent<SpriteRenderer>().flipY)
                    {
                        shootBullet.transform.position = gunSource.transform.Find("bulletSpawnFlipped").position;
                    }
                    else
                    {
                        shootBullet.transform.position = gunSource.transform.Find("bulletSpawn").position;
                    }
                    shootBullet.transform.rotation = gunSource.transform.rotation;
                    shootBullet.transform.Rotate(new Vector3(0, 0, shootBullet.transform.rotation.z + startAngle + (angleChunks*j)));
                    shootBullet.GetComponent<moveBullet>().movementSpeedMultiplier = attackInfo.bulletSpeedMultiplier;

                }
            }
            else
            {
                GameObject shootBullet = Instantiate(attackInfo.bullet);
                createBullet(shootBullet, attackInfo);
                if (attackInfo.beam)
                {
                    StartCoroutine(destroyBeam(shootBullet, attackInfo.beamConfig.lifespan,gunSource));
                }
                Debug.Log(shootBullet.GetComponent<SpriteRenderer>().material.GetColor("_Color"));
                changeColour(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour, shootBullet);
                test = GameObject.FindGameObjectsWithTag("bullet");
                Debug.Log(test.Length);
                float width = gunSource.GetComponent<SpriteRenderer>().bounds.size.x;
                if (gunSource.GetComponent<SpriteRenderer>().flipY)
                {
                    shootBullet.transform.position = gunSource.transform.Find("bulletSpawnFlipped").position;
                }
                else
                {
                    shootBullet.transform.position = gunSource.transform.Find("bulletSpawn").position;
                }
                shootBullet.transform.rotation = gunSource.transform.rotation;
                shootBullet.GetComponent<moveBullet>().movementSpeedMultiplier = attackInfo.bulletSpeedMultiplier;
            }
            yield return new WaitForSeconds(attackInfo.bulletDelay);
            attackInfo.colourConfig.stage++;
            if (attackInfo.colourConfig.stage >= attackInfo.colourConfig.Colour.Count)
            {
                attackInfo.colourConfig.stage = 0;
            }
        }
        if(!attackInfo.beam)
        {
            Destroy(gunSource);
        }
    }
    public static GameObject createBullet(GameObject shootBullet, gunSpawnInfo.spawnedGun attackInfo)
    {
        shootBullet.GetComponent<moveBullet>().scaleUpX = attackInfo.scaleX;
        shootBullet.GetComponent<moveBullet>().scaleUpY = attackInfo.scaleY;
        shootBullet.GetComponent<moveBullet>().incrementalGrowth = attackInfo.incrementalGrowth;
        shootBullet.GetComponent<moveBullet>().growthMultiplier = attackInfo.growthMultiplier;
        shootBullet.GetComponent<damagePlayer>().damage = attackInfo.bulletDamage;
        return shootBullet;
    }
    IEnumerator destroyBeam(GameObject shootBullet, float time, GameObject gunSource)
    {
        yield return new WaitForSeconds(time);
        Destroy(shootBullet);
        Destroy(gunSource);
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
    public static void summonBullet(GameObject gunSource, gunSpawnInfo.spawnedGun attackInfo, float angle)
    {

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
        public GameObject bullet;
        public GameObject gun;
        public Sprite gunTexture;
        public float bulletDelay;
        public float bulletAmmount;
        public float bulletDamage;
        public float bulletSpeedMultiplier;
        public float growthMultiplier;
        public float scaleX; //bullet scale (is a multiplier)
        public float scaleY;
        public float gunScaleX;
        public float gunScaleY;
        public bool shootTowardsPlayer;
        public bool multiShoot;
        public bool tracksPlayer;
        public bool incrementalGrowth;
        public bool beam;
        public float trackingAccuracy;
        public multiShootSettings multiShootConfig;
        public colourChange colourConfig;
        public beamSettings beamConfig;
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
    public List<Color> Colour = new List<Color>();
}
[System.Serializable]
public class beamSettings
{
    public float lifespan;
}
