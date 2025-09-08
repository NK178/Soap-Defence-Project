using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ProduceBubbles", menuName = "Scriptable Objects/ProduceBubbles")]
public class ProduceBubbles : EntityFunctions
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float bubbleAmount;
    [SerializeField] private float productionRate;
    [SerializeField] private float spawnRadius; 
    

    public override IEnumerator ExcuteCoroutine(GameObject parentObject = null)
    {
        if (parentObject == null)
            yield return null;

        //run infintely until stopped 
        while (true)
        {
            if (ShopManager.instance != null)
            {
                //float xFactor = Random.Range(-spawnRadius, spawnRadius);
                float xFactor = 0f;
                Vector3 spawnPosition = new Vector3(xFactor + parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z);
                GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, parentObject.transform.rotation);
            }

            yield return new WaitForSeconds(productionRate);
        }

    }

}
