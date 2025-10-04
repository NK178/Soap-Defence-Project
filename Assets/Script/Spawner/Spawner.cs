
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
            SpawnEntity();
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



}
