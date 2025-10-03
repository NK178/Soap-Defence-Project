
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnDetails
{
    public Entity entity;
    public int quantity;
}

public class Spawner : MonoBehaviour
{

    [SerializeField] private int timeMaxRange; 
    [SerializeField] private int timeMinRange;
    //[HideInInspector] public IEnumerator spawnCoroutine; 
    private Queue<Entity> entityQueue;

    

    void Awake()
    {
        if (entityQueue == null) 
            entityQueue = new Queue<Entity>();
        //spawnCoroutine = ExcuteSpawnCoroutine();
    }

    private void SpawnEntity()
    {
        if (entityQueue.Count > 0)
        {
            Entity reference = entityQueue.Dequeue();
            Entity newSpawn = Instantiate(reference, gameObject.transform.position, gameObject.transform.rotation);
        }
    }


    public IEnumerator ExcuteSpawnCoroutine()
    {
        //self disable when empty 
        while (entityQueue.Count > 0)
        {
            int waitTime = Random.Range(timeMinRange, timeMaxRange);
            yield return new WaitForSeconds(waitTime);
            SpawnEntity();
        }
    }

    //broken due to load timing cyka 
    public void AddItemsIntoQueue(Entity entity)
    {
        if (entityQueue == null)
            entityQueue = new Queue<Entity>();
        else 
            entityQueue.Enqueue(entity);
    }


    
}
