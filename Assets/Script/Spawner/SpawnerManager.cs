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
    //private IEnumerator waveCooldown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //start from one I guess 
        waveIndex = 0;
        //isActive = false;
        changeWave = true;
        LoadNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (changeWave)
            {
                StartWave();
                LoadNextWave();
                ExcuteWaveTimer();
            }

        }
    }

    
    //TO DO 2/10
    private IEnumerator ExcuteWaveTimer()
    {
        changeWave = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        changeWave = true;
    }


    private void StartWave()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            StartCoroutine(spawnerList[i].ExcuteSpawnCoroutine());
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
            for (   int j = 0; j < quantity; j++)
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
