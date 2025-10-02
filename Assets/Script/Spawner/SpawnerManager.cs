using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField] private List<Spawner> spawnerList;
    [SerializeField] private List<SpawnerData> waveDataList;
    [SerializeField] private float timeBetweenWaves; 


    //temp public make it private later 
    public bool isActive;
    private bool changeWave;
    private int waveIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //start from one I guess 
        waveIndex = 1;
        isActive = changeWave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (changeWave)
                StartNewWave();

        }
    }

    
    //TO DO 2/10
    private IEnumerator ExcuteWaveTimer()
    {
        yield return new WaitForSeconds(timeBetweenWaves);  
    }


    private void StartNewWave()
    {
        AssignEntitiesToSpawners();
        waveIndex++;
        changeWave = false;
    }

    private void AssignEntitiesToSpawners()
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
                int spanwerIndex = Random.Range(0, spawnerList.Count - 1);
                spawnerList[spanwerIndex].AddItemsIntoQueue(entity);
            }
        }
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
}
