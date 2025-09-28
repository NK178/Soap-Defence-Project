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
        //run only once per enter into this state
        if (parentObject == null)
            yield return null;

        float xFactor = Random.Range(-spawnRadius, spawnRadius);
        Vector3 spawnPosition = new Vector3(xFactor + parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z);
        GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, parentObject.transform.rotation);
        yield return null;
    }
}
