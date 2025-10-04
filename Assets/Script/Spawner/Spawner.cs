
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool fastSpawn;
    

    void Awake()
    {
        if (entityQueue == null) 
            entityQueue = new Queue<Entity>();
        fastSpawn = false;
    }

    private void SpawnEntity(SpawnerManager manager)
    {
        if (entityQueue.Count > 0)
        {
            Entity reference = entityQueue.Dequeue();
            Entity newSpawn = Instantiate(reference, gameObject.transform.position, gameObject.transform.rotation);
            manager.AddActiveEntity(newSpawn);
            //Debug.Log(name + " SPAWNED ENTITY COUNT LEFT : " + entityQueue.Count);
        }
    }


    public IEnumerator ExcuteSpawnCoroutine(SpawnerManager manager)
    {
        float waitTime = 0f;

        //self disable when empty 
        while (entityQueue.Count > 0)
        {
            //spawn fast have little delay but still fast 
            if (fastSpawn)
                waitTime = 0.5f; //abritary num 
            else 
                waitTime = Random.Range(timeMinRange, timeMaxRange);
            yield return new WaitForSeconds(waitTime);
            SpawnEntity(manager);
        }
        fastSpawn = false;
    }


    public void AddItemsIntoQueue(Entity entity)
    {
        //timing issue cyka 
        if (entityQueue == null)
            entityQueue = new Queue<Entity>();

        if (entityQueue != null)
            entityQueue.Enqueue(entity);
    }

    public void ActivateFastSpawn()
    {
        fastSpawn = true;
    }

    public int GetCurrentSpawnDataCount()
    {
        return entityQueue.Count;
    }


}
