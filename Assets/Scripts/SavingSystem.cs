using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    string defaultDataPath;
    BinaryFormatter BinaryFormatter = new BinaryFormatter();
    versionedData saveDataObject = new versionedData();
    public bool hasDebug;
    //damn

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        defaultDataPath = Application.persistentDataPath + "/saveData";
        loadData();
        saveData();
    }
    void saveData()
    {
        FileStream saveStream = File.Create(defaultDataPath);
        BinaryFormatter.Serialize(saveStream, saveDataObject);
        saveStream.Close();
    }
    void loadData()
    {
        if (File.Exists(defaultDataPath))
        {
            FileStream saveTwo = File.Open(defaultDataPath, FileMode.Open);
            saveDataObject = (versionedData)BinaryFormatter.Deserialize(saveTwo);
            saveTwo.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    [System.Serializable]
    public class saveBossData
    {
        public saveBossData(int Difficulty, string BossName)
        {
            this.difficulty = Difficulty;
            this.bossName = BossName;
        }
        public int difficulty;
        public string bossName;
    }
    #region saveVersions
    [System.Serializable]
    public class allSaveData
    {
        public bool hasDebug;
        public List<saveBossData> completeBosses = new List<saveBossData>();
    }
    #endregion
    [System.Serializable]
    public class versionedData
    {
        public int version;
        public object data;
    }
}
