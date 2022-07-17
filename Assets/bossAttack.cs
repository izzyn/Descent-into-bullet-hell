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
            gunSpawned.GetComponent<Transform>().position = attackInfo.spawnInfo.gunInfo[i].spawnLocation.GetComponent<Transform>().position;
            gunSpawned.transform.Rotate(new Vector3(0, 0, attackInfo.spawnInfo.gunInfo[i].rotation));
            for(int j = 0; j > 3; j++)
            {
                GameObject shootBullet = Instantiate(attackInfo.spawnInfo.gunInfo[i].bullet, transform.position, Quaternion.identity);
                shootBullet.GetComponent<damagePlayer>().damage = 5f;
                shootBullet.transform.position += transform.right * Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }
    IEnumerator pickAttack()
    {
        int oldIndex = 0;
        while(true)
        {
            int pickedAttack = Random.Range(0, attackBasicList.Count -1);
            if(pickedAttack >= oldIndex)
            {
                pickedAttack++;
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
                Debug.Log("Shooting");
                StartCoroutine(attackCoroutine);
                yield return new WaitForSeconds(attackBasicList[pickedAttack].duration);
                StopCoroutine(attackCoroutine);
            }
            else
            {
                Debug.Log("Waiting");
                yield return new WaitForSeconds(attackBasicList[pickedAttack].duration);
            }
        }
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
        public enum weaponSpawned
        {
            basicGun
        }
    }
    public List<spawnedGun> gunInfo = new List<spawnedGun>();

}
