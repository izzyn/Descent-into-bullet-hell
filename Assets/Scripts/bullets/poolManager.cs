using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class poolManager : MonoBehaviour
{
    
    [SerializeField]
    List<bulletPoolConfig> bulletPoolConfigs = new List<bulletPoolConfig>();
    Dictionary<GameObject, bulletPool> bulletPools = new Dictionary<GameObject, bulletPool> ();

    // Start is called before the first frame update
    private void Start()
    {
        foreach(bulletPoolConfig config in bulletPoolConfigs)
        {
            bulletPools.Add(config.bulletPrefab, new bulletPool(config));
        }
    }
    public GameObject requestBullet(GameObject prefab)
    {
        return bulletPools[prefab].requestBullet();
    }
    public void unrequestBullet(GameObject removeObject)
    {
        bulletPools[removeObject.GetComponent<moveBullet>().prefab].unrequestBullet(removeObject);
    }
    public void resetAll()
    {
        foreach(bulletPool pools in bulletPools.Values)
        {
            pools.resetAll();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public class bulletPool
    {
        int size;
        GameObject[] pool;
        int usedBullets = 0;
        public bulletPool(bulletPoolConfig poolConfigs)
        {
            size = poolConfigs.bulletCount;
            pool = new GameObject[poolConfigs.bulletCount];
            for (int i = 0; i < poolConfigs.bulletCount; i++)
            {
                pool[i] = Instantiate(poolConfigs.bulletPrefab);
                pool[i].GetComponent<moveBullet>().prefab = poolConfigs.bulletPrefab;
                DontDestroyOnLoad(pool[i]);
                pool[i].SetActive(false);
            }
        }
        public GameObject requestBullet()
        {
            if (usedBullets < size)
            {
                GameObject saveBullet = pool[usedBullets];
                saveBullet.GetComponent<moveBullet>().index = usedBullets;
                usedBullets++;
                saveBullet.SetActive(true);
                return saveBullet;
            }
            else
            {
                throw new Exception("No bullets left");
            }
        }
        public void unrequestBullet(GameObject removeObject)
        {
            if (removeObject.activeSelf == true)
            {
                int bulletIndex = removeObject.GetComponent<moveBullet>().index;
                removeObject.SetActive(false);
                usedBullets--;
                (pool[bulletIndex], pool[usedBullets]) = (pool[usedBullets], pool[bulletIndex]);
                pool[bulletIndex].GetComponent<moveBullet>().index = bulletIndex;
            }
        }
        public void resetAll()
        {
            for (int i = 0; i < usedBullets; i++)
            {
                pool[i].SetActive(false);
            }
            usedBullets = 0;
        }
    }
    [System.Serializable]
    public class bulletPoolConfig
    {
        public GameObject bulletPrefab;
        public int bulletCount;
    }
}
