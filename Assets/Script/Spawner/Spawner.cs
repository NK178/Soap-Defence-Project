
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;


[System.Serializable]
public class SpawnDetails
{
    public Entity entity;
    public int quantity;
}

public class Spawner : MonoBehaviour
{

    [SerializeField] private float timeMaxRange; 
    [SerializeField] private float timeMinRange;
    private Queue<Entity> entityQueue;

    

    void Awake()
    {
        if (entityQueue == null) 
            entityQueue = new Queue<Entity>();
    }

    private void SpawnEntity()
    {
        if (entityQueue.Count > 0)
        {
            Entity reference = entityQueue.Dequeue();
            Entity newSpawn = Instantiate(reference, gameObject.transform.position, gameObject.transform.rotation);
            //Debug.Log(name + " SPAWNED ENTITY COUNT LEFT : " + entityQueue.Count);
        }
    }


    public IEnumerator ExcuteSpawnCoroutine()
    {
        //self disable when empty 
        while (entityQueue.Count > 0)
        {
            float waitTime = Random.Range(timeMinRange, timeMaxRange);
            yield return new WaitForSeconds(waitTime);
            SpawnEntity();
        }
    }


    public void AddItemsIntoQueue(Entity entity)
    {
        //timing issue cyka 
        if (entityQueue == null)
            entityQueue = new Queue<Entity>();

        if (entityQueue != null)
            entityQueue.Enqueue(entity);
    }


    
}
