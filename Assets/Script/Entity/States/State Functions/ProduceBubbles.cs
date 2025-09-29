using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ProduceBubbles", menuName = "Scriptable Objects/ProduceBubbles")]
public class ProduceBubbles : EntityFunctions
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float bubbleAmount;
    [SerializeField] private float productionRate;
    [SerializeField] private float spawnRadius;

    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        Vector3 position = entity.gameObject.transform.position;

        float xFactor = Random.Range(-spawnRadius, spawnRadius);
        Vector3 spawnPosition = new Vector3(xFactor + position.x, position.y, position.z);
        GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, entity.gameObject.transform.rotation);
        yield return null;
    }
}
