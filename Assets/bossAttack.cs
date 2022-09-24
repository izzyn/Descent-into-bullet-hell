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
    [System.Serializable]
    public class phaseAttacks
    {
        public int hpThreshold;
        public List<attackBasic> attackBasicList = new List<attackBasic>(); //the list of attacks the boss can have
    }
    public List<phaseAttacks> phaseAttacksList = new List<phaseAttacks>();
    public GameObject[] test; //Ignore, this is a test list to document the amount of bullets on the screen for stress testing
    public IEnumerator currentPhase;


    // Start is called before the first frame update
    void Start()
    {
        currentPhase = pickAttack(phaseAttacksList[0].attackBasicList);
        StartCoroutine(currentPhase); //makes the boss pick attacks when it loads in
    }
    public void updatePhase(int hp, List<phaseAttacks> listPhaseAttacks) //Updates phase every time the boss loses HP
    {
        for(int i = 1; i < listPhaseAttacks.Count; i++) 
        {
            if(listPhaseAttacks[i].hpThreshold == hp)
            {
                StopCoroutine(currentPhase);
                IEnumerator newPhase = pickAttack(phaseAttacksList[i].attackBasicList); //Sets the new attack list to the one that corresponds to that phase
                StartCoroutine(newPhase);
                currentPhase = newPhase;
                break;
            }
        }
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
        for(int i = 0; i < attackInfo.spawnInfo.gunInfo.Count; i++) //Spawns a gun for each gun in the attacks's list
        {
            GameObject gunSpawned = Instantiate(attackInfo.spawnInfo.gunInfo[i].gun); //Creates an object using the information provided
            Vector2 savedScale = gunSpawned.transform.localScale; 
            attackInfo.spawnInfo.gunInfo[i].bulletConfig.lockedToGun = attackInfo.spawnInfo.gunInfo[i].rotationConfig.lockedToGun;
            gunSpawned.transform.localScale = new Vector2(savedScale.x * attackInfo.spawnInfo.gunInfo[i].gunScaleX, savedScale.y * attackInfo.spawnInfo.gunInfo[i].gunScaleY);
            gunSpawned.transform.position = new Vector3(attackInfo.spawnInfo.gunInfo[i].spawnLocation.transform.position.x, attackInfo.spawnInfo.gunInfo[i].spawnLocation.transform.position.y, -3);
            gunSpawned.GetComponent<SpriteRenderer>().sprite = attackInfo.spawnInfo.gunInfo[i].gunTexture; //All of this just gives the information needed to create the guns and bullets, really long because it's high up in the information hierarchy.
            if(gunSpawned.GetComponent<SpriteRenderer>().sharedMaterial == attackInfo.spawnInfo.gunInfo[i].bulletConfig.bullet.GetComponent<SpriteRenderer>().sharedMaterial)
            {
                changeColour(attackInfo.spawnInfo.gunInfo[i].bulletConfig.colourConfig.stage, attackInfo.spawnInfo.gunInfo[i].bulletConfig.colourConfig.Colour, gunSpawned); //Green screens the gun to the same colour as the bullet
            }
            Vector3 newScale = gameObject.transform.localScale;
            if (attackInfo.spawnInfo.gunInfo[i].shootTowardsPlayer != true)
            {
                gunSpawned.transform.Rotate(new Vector3(0, 0, attackInfo.spawnInfo.gunInfo[i].rotation));
                if ((gunSpawned.transform.rotation.eulerAngles.z * Mathf.Rad2Deg) > 90f || (gunSpawned.transform.rotation.eulerAngles.z) < -90f)
                {
                    Debug.Log("Yes"); //Flips the gun sprite so it doesn't look weird when rotating
                    gunSpawned.GetComponent<SpriteRenderer>().flipY = true;
                }
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 target = player.transform.position - gunSpawned.transform.position;
                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg; //Creates an angle from the gun to the player
                gunSpawned.transform.Rotate(new Vector3(0, 0, angle)); //Rotates the gun to that angle
                if (((gunSpawned.transform.rotation.eulerAngles.z) > 90) && ((gunSpawned.transform.rotation.eulerAngles.z) < 270))
                {
                    gunSpawned.GetComponent<SpriteRenderer>().flipY = true; //Making sure the sprite looks good while rotating
                }
                else
                {
                    gunSpawned.GetComponent<SpriteRenderer>().flipY = false;
                }
            }
            if(attackInfo.spawnInfo.gunInfo[i].tracksPlayer) //Makes the gun rotation follow the player if that setting is on
            {
                StartCoroutine(followPlayer(attackInfo.spawnInfo.gunInfo[i], gunSpawned));
            }
            if (attackInfo.spawnInfo.gunInfo[i].rotatesOverTime) //Same as above but rotating over time instead of towards the player
            {
                StartCoroutine(rotateGun(attackInfo.spawnInfo.gunInfo[i], gunSpawned));
            }
            StartCoroutine(spawnBullets(attackInfo.spawnInfo.gunInfo[i].bulletConfig, gunSpawned, attackInfo.spawnInfo.gunInfo[i].shatterConfig)); //Starts spawning bullets

            yield return new WaitForSeconds(attackInfo.spawnInfo.spawnDelay); //The cooldown between spawning guns
        }
    }
    IEnumerator rotateGun(gunSpawnInfo.spawnedGun attackInfo, GameObject gun) //Rotates the gun (duh)
    {
        yield return new WaitForSeconds(attackInfo.rotationConfig.timeToStart); //Waits for the start cooldown to finish until it starts rotating baptul bim
        Vector3 saveRotation = gun.transform.rotation.eulerAngles;
        float rotationSteps = 500; //Amounts of times it updates the rotation before rotating back
        float degreesPerRotation = (attackInfo.rotationConfig.degrees / rotationSteps); //#-of degrees per rotation step
        if(gun.GetComponent<SpriteRenderer>().flipY) //If the Y is flipped, rotate the other way.
        {
            degreesPerRotation *= -1;
        }
        while(gun != null) //Null check
        {
            for(int i = 0; i < rotationSteps; i++)
            {
                if (gun != null) //Another null check (I'm paranoid)
                {
                    gun.transform.Rotate(new Vector3(0, 0, degreesPerRotation));
                    yield return new WaitForSeconds(0.01f / attackInfo.rotationConfig.speedMultiplier);
                }
            }
            if(attackInfo.rotationConfig.rotatesBack) //if it rotates back,start rotating back 
            {
                degreesPerRotation *= -1;
            }
            else
            {
                if(gun != null) //Null check
                {
                    Quaternion rotation = Quaternion.Euler(saveRotation); //resets the rotation
                    gun.transform.rotation = rotation;
                }
            }
        }
    }
    IEnumerator followPlayer(gunSpawnInfo.spawnedGun attackInfo, GameObject gun) //Makes the gun rotation follow the player over time
    {
        while(gun != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player"); //Finds the player object
            Vector3 target = player.transform.position - gun.transform.position; //Makes a vector between the player and the gun
            gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, target); //Sets the rotation towards the vector
            gun.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            yield return new WaitForSeconds(attackInfo.trackingAccuracy); //The lower the accuracy, the slower it updates.
            
        }
    }


    public IEnumerator spawnBullets(BulletSimple attackInfo, GameObject gunSource, shatterShotConfig shatterConfig = null) //SPAWNS BULLETS
    {
        yield return new WaitForSeconds(attackInfo.delayBeforeShooting); //Waits for a delay before it starts spawning bullets
        for (int i = 0; i < attackInfo.bulletAmmount; i++) //Iterates for each bullet
        {
            if(gunSource != null) //Another null check
            {
                for(int j = 0; j < attackInfo.multiShootConfig.bulletMultiplier; j++) //Iterates for each bullet stream
                {
                    float startAngle = (attackInfo.multiShootConfig.angle / 2) * -1; //Sets the angle to 50% of the total angle below where it starts
                    float angleChunks = attackInfo.multiShootConfig.angle / (attackInfo.multiShootConfig.bulletMultiplier - 1); //Makes all bullet streams equally far away from eachother angle-wise

                    GameObject shootBullet = Instantiate(attackInfo.bullet); //Creates the bullet
                    if(startAngle != 0) //Sets the bullet offsets (used for the move bullet script)
                    {
                        shootBullet.GetComponent<moveBullet>().offset = (angleChunks * j) + startAngle;
                    }
                    else
                    {
                        shootBullet.GetComponent<moveBullet>().offset = 0;
                    }
                    if (shatterConfig != null) //If there is a possibility of the bullet shattering, give the information for that.
                    {
                        createBullet(shootBullet, attackInfo, gunSource, shatterConfig); //Gives the bullet the information it needs
                    }
                    else
                    {
                        createBullet(shootBullet, attackInfo, gunSource);
                    }
                    if (attackInfo.beam)
                    {
                        bool kil = false;
                        if (i + 1 >= attackInfo.bulletAmmount)
                        {
                            kil = true;
                        }
                        StartCoroutine(destroyBullet(shootBullet, attackInfo.beamConfig.lifespan, gunSource, kil)); //Instead of a bullet getting destroyed when they hit walls, beams get destroyed after a period of time
                    }
                    if (attackInfo.dieEarly) //Lets it die early if it should (works exactly like beams except the guns do not get destroyed in the method)
                    {
                        StartCoroutine(destroyBullet(shootBullet, attackInfo.beamConfig.lifespan, gunSource));
                    }
                    Debug.Log(shootBullet.GetComponent<SpriteRenderer>().material.GetColor("_Color"));
                    changeColour(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour, shootBullet); //Sets the colour of the bullet
                    if (attackInfo.colourConfig.colourMode == colourChange.mode.line) //Depending on the setting, the bullet color updates at different points and in different ways
                    {
                        attackInfo.colourConfig.stage = changeColourState(attackInfo.colourConfig.stage, attackInfo.colourConfig.Colour.Count);
                    }
                    if (attackInfo.colourConfig.colourMode == colourChange.mode.random)
                    {
                        int newstageL = Random.Range(0, attackInfo.colourConfig.Colour.Count); //sets the stage (the colour in the array) randomly)
                        attackInfo.colourConfig.stage = changeColourState(newstageL, attackInfo.colourConfig.Colour.Count); //Makes the bullet be another colour next time
                    }
                    test = GameObject.FindGameObjectsWithTag("bullet");
                    Debug.Log(test.Length); //For debugging reasons, shows how many bullets are on screen (for stress testing)
                    float width = gunSource.GetComponent<SpriteRenderer>().bounds.size.x;
                    if (gunSource.transform.Find("bulletSpawn") != null) //Puts the bullet in the correct part of its source. Depending on the "FlipY" propety of the source
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
                        shootBullet.transform.position = placeBullet(gunSource); //Sets position correclty
                    }
                    shootBullet.transform.rotation = gunSource.transform.rotation; //The rotation of the bullet == the rotation of the source
                    if (startAngle != 0) //Checks for 0 just to make sure the formula doesn't break
                    {
                        shootBullet.transform.Rotate(new Vector3(0, 0, shootBullet.transform.rotation.z + startAngle + (angleChunks * j)));
                    }
                    shootBullet.GetComponent<moveBullet>().movementSpeedMultiplier = attackInfo.bulletSpeedMultiplier; //gives the bullet script a property from this script
                    if (shatterConfig != null)
                    {
                        if (shatterConfig.travelShot != null && shatterConfig.shootsWhileTraveling && gunSource != null)
                        {
                            splitShot(shatterConfig.travelShot, shootBullet); //Makes it shoot split bullets if you select it
                        }
                    }
                    if(shootBullet.GetComponent<moveBullet>().bulletInfo.richRocket)
                    {
                        if(!shootBullet.GetComponent<moveBullet>().bulletInfo.rickRocketSettings.rickRocketsOnWallHit)
                        {
                            StartCoroutine(startRichRocket(shootBullet.GetComponent<moveBullet>().bulletInfo.rickRocketSettings.timeToRickRocket, shootBullet));
                        }
                    }
                }

                yield return new WaitForSeconds(attackInfo.bulletDelay); //Waits the delay between bullets
                if(attackInfo.colourConfig.colourMode == colourChange.mode.row) //Another color setting
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
                Destroy(gunSource); //destroys the bullets after all bullets have been shot (provided the bullets are not beams and the source isn't a bullet)
            }
        }
    }
    public IEnumerator startRichRocket(float delay, GameObject shootBullet)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Imma do rickrocket");    
        shootBullet.transform.Rotate(0, 0, 180);
        
    }
    public void splitShot(BulletSimple information, GameObject source)
    {
        StartCoroutine(spawnBullets(information, source)); //spawns bullets but with less information as to not create an infinite loop
    }
    public GameObject createBullet(GameObject shootBullet, BulletSimple attackInfo, GameObject gunSource, shatterShotConfig shatterConfig = null)
    {
        shootBullet.GetComponent<moveBullet>().bulletInfo = attackInfo.shallowCopy(); //Gives the bullet scripts all information they require to function
        shootBullet.GetComponent<damagePlayer>().damage = attackInfo.bulletDamage;
        shootBullet.GetComponent<damagePlayer>().removeWhenHit = attackInfo.removeHit;
        shootBullet.GetComponent<damagePlayer>().removeWhenInvincible = attackInfo.removeInvincible;
        shootBullet.GetComponent<moveBullet>().gunOrigin = gunSource;
        shootBullet.GetComponent<moveBullet>().sourceScript = this;
        if(shatterConfig != null)
        {
            if(shatterConfig.destoyShoot != null && shatterConfig.shootsWhenDestroyed) //Provides shatter information if present
            {
                shootBullet.GetComponent<moveBullet>().shootsWhenDie = shatterConfig.shootsWhenDestroyed;
                shootBullet.GetComponent<moveBullet>().dieProperties = shatterConfig.destoyShoot;
            }
        }
        return shootBullet;
    }
    public Vector3 placeBullet(GameObject location)
    {
        return new Vector3(location.transform.position.x, location.transform.position.y, -3); //function that copies the location except for the Z so that camera layering works properly
    }
    public int changeColourState(int stage, int count) 
    {
        stage++; //Increases the stage of the color (representing the n-th color in the list)
        if(stage >= count) //resets the stage if the stage is greater or equal to the amount of items in the list of colors
        {
            stage = 0;
        }
        return stage;
    }
    IEnumerator destroyBullet(GameObject shootBullet, float time, GameObject gunSource, bool kil = false)
    {
        yield return new WaitForSeconds(time); //time before X is destroyed
        if(shootBullet != null && shootBullet.GetComponent<moveBullet>().shootsWhenDie && shootBullet.GetComponent<moveBullet>().dieProperties != null && this.gameObject.GetComponent<objectKiller>() != null)
        {
            this.gameObject.GetComponent<objectKiller>().shatterDie(shootBullet, true); //Makes the shot shatter on death if that option was picked
        }
        if(shootBullet != null)
        {
            Destroy(shootBullet);
        }
        if(kil) //Destroys the source if all beams have been destroyed.
        {
            Destroy(gunSource);
        }
    }
    IEnumerator pickAttack(List<attackBasic> attackBasicList)
    {
        int oldIndex = 0;
        while(true)
        {
            int pickedAttack;
            if (attackBasicList.Count > 1) //Needed so that the index isn't too low to function
            {
                pickedAttack = Random.Range(0, attackBasicList.Count - 1); //picks a random attack from the list of attacks
                if (pickedAttack >= oldIndex)
                {
                    pickedAttack++; //Makes sure the same attack isn't picked twice.
                }
            }
            else
            {
                pickedAttack = Random.Range(0, attackBasicList.Count);
            }
            oldIndex = pickedAttack;
            IEnumerator attackCoroutine = null;
            switch(attackBasicList[pickedAttack].Type) //Runs the coroutine that corresponds to the correct attack
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
            if(attackCoroutine != null) //null check
            {
                StartCoroutine(attackCoroutine);
                yield return new WaitForSeconds(attackBasicList[pickedAttack].duration); //waits for the attacks duration
                StopCoroutine(attackCoroutine);
            }
            else
            {
                yield return new WaitForSeconds(attackBasicList[pickedAttack].duration);
            }
        }
    }
    void changeColour(int stage, List<Color> ColourList, GameObject whatToChange) //Sets the color of the provided game object to the listed stage
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
        public GameObject spawnLocation; //Where the gun spawns (takes the location's transform values)
        public float rotation; //rotation of the gun (in Euler angles)
        public GameObject gun; //The gun object prefab (used for bullet locations)
        public Sprite gunTexture; //Incase I want to make reskins of the same prefab
        public float gunScaleX; //scales for the weapon
        public float gunScaleY;
        public bool shootTowardsPlayer; //Sets the rotation to towards the player upon spawning
        public bool tracksPlayer; //Sets the rotation towards the player every "trackingAccuracy" seconds
        public float trackingAccuracy;
        public bool rotatesOverTime; //Turns on overtime rotations
        public BulletSimple bulletConfig; //Contains all the bullet properties
        public rotationSettings rotationConfig; //--=-- rotation properties
        public shatterShotConfig shatterConfig; // --=-- shattering properties (both while traveling and on destruction
        int stage;
    }
    public float spawnDelay; //Delay between each gun spawning
    public List<spawnedGun> gunInfo = new List<spawnedGun>(); //List of all the guns in the attack
}
[System.Serializable]
public class multiShootSettings //Settings required for shooting multiple bullets in a cone shape
{
    public int bulletMultiplier;
    public float angle;
}
[System.Serializable]
public class colourChange //All settings for changing colors
{
    public int stage;
    public enum mode
    {
        line,  //Changing for every vertical line of bullets
        row, //Changing for every horizontal line of bullets
        random //Changes per-bullet randomly
    }
    public mode colourMode;
    public List<Color> Colour = new List<Color>(); //Every color the bullets cycle through
}
[System.Serializable]
public class beamSettings
{
    public float lifespan; //Also used for dying early (is the time in seconds it takes for a beam/bullet to get destroyed)
}
[System.Serializable]
public class rotationSettings
{
    public float timeToStart; //Time before the gun starts rotating
    public float degrees; //The amount of degrees it rotates before reseting/going back
    public float speedMultiplier;
    public bool rotatesBack;            
    public bool lockedToGun;
}
[System.Serializable]
public class BulletSimple
{
    public BulletSimple shallowCopy()
    {
        return (BulletSimple)this.MemberwiseClone();
    }
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
    public bool removeInvincible; //Settings for if the bullet gets destroyed when you hit it while invincible
    public bool removeHit; //same but you don't need to be invincible
    public bool incrementalGrowth; //grows over time
    public bool beam; //For all the differances between normal bullets and beams
    public bool dieEarly;
    public bool richRocket;
    public bool diesOnWallHit;
    public multiShootSettings multiShootConfig; //for all settings needed to shoot multiple bullets
    public colourChange colourConfig;
    public beamSettings beamConfig;
    public rickrocket rickRocketSettings;
}

[System.Serializable]
public class shatterShotConfig
{
    public bool shootsWhileTraveling;
    public BulletSimple travelShot; //contains the bullet class to give the information needed for what the bullet shot from the bullet should do
    public bool shootsWhenDestroyed;
    public BulletSimple destoyShoot;

}
[System.Serializable]
public class rickrocket
{
    public float timeToRickRocket;
    public bool rickRocketsOnWallHit;
    public int timesItBounces;
}