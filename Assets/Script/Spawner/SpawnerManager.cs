using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField] private List<Spawner> spawnerList;
    [SerializeField] private List<SpawnerData> waveDataList;
    [SerializeField] private float timeBetweenWaves;

    private List<Entity> activeSpawnList;
    //temp public make it private later 
    private bool isActive;
    private bool changeWave;
    [HideInInspector] public bool areAllWavesSent;
    private int waveIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        activeSpawnList = new List<Entity>();
        //start from one I guess 
        waveIndex = 0;
        //isActive = false;
        changeWave = true;
        areAllWavesSent = isActive = false;
        LoadNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (changeWave && !areAllWavesSent)
            {
                StartWave();
                LoadNextWave();
                ExcuteWaveTimer();
            }

            if (waveIndex == waveDataList.Count)
            {
                //Debug.Log("WAVE DATA SENT");
                areAllWavesSent = true;
            }
        }
    }

   
    private IEnumerator ExcuteWaveTimer()
    {
        changeWave = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        changeWave = true;
    }


    private void StartWave()
    {
        SpawnerData data = GetCurrentWaveData();
        if (data == null)
            return;

        for (int i = 0; i < spawnerList.Count; i++)
        {
            //check if wave is fast spawn mode 
            if (data.fastSpawn)
                spawnerList[i].ActivateFastSpawn();
            StartCoroutine(spawnerList[i].ExcuteSpawnCoroutine(this));
        }
        waveIndex++;
        changeWave = false;
    }

    private void LoadNextWave()
    {

        SpawnerData data = GetCurrentWaveData();
        if (data == null)
            return;


        for (int iter = 0; iter < data.dataList.Count; iter++)
        {
            int quantity = data.dataList[iter].quantity;
            Entity entity = data.dataList[iter].entity;
            for (int j = 0; j < quantity; j++)
            {
                //for some reason the int version of range is max exclusive ???
                int spanwerIndex = Random.Range(0, spawnerList.Count);
                spawnerList[spanwerIndex].AddItemsIntoQueue(entity);
            }
        }
    }

    public bool AreAllSpawnsInactive()
    {
        foreach (Entity entity in activeSpawnList)
        {
            if (entity.GetActiveStatus())
                return false; 
        }
        return true;
    } 


    public bool AreAllSpawnersEmpty()
    {
        foreach (Spawner spawner in spawnerList)
        {
            if (spawner.GetCurrentSpawnDataCount() > 0)
                return false;
        }
        return true;
    }

    private SpawnerData GetCurrentWaveData()
    {
        for(int i = 0; i < waveDataList.Count; i++)
        {
            if (i == waveIndex)
                return waveDataList[i];
        }
        return null;
    }

    public void AddActiveEntity(Entity entity)
    {
        if (isActive)
        {
            //Debug.Log("ADDING ACTIVE ENTITY: " + entity.name);
            activeSpawnList.Add(entity);
        }
    }

    public void GetActiveSpawnsList(out List<Entity> entityList)
    {
        //shallow copy should be fine I think
        entityList = activeSpawnList;
    }

    public void SetActiveStatus(bool condition)
    {
        isActive = condition; 
    }
}

