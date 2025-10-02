
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
    private Queue<Entity> entityQueue;


    void Awake()
    {
        entityQueue = new Queue<Entity>();
    }

    private void SpawnEntity()
    {
        if (entityQueue.Count != 0)
        {
            Entity reference = entityQueue.Dequeue();
            Entity newSpawn = Instantiate(reference, gameObject.transform.position, gameObject.transform.rotation);
        }
    }


    public IEnumerator ExcuteSpawnCoroutine()
    {
        int waitTime = Random.Range(timeMinRange, timeMaxRange);
        yield return new WaitForSeconds(waitTime);
        SpawnEntity();
    }


    public void AddItemsIntoQueue(Entity entity)
    {
        entityQueue.Enqueue(entity);
    }


    
}
