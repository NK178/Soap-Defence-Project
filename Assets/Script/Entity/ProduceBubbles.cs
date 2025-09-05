using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ProduceBubbles", menuName = "Scriptable Objects/ProduceBubbles")]
public class ProduceBubbles : EntityFunctions
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float bubbleAmount;
    [SerializeField] private float productionRate;
    

    public override IEnumerator ExcuteCoroutine(GameObject parentObject = null)
    {
        if (parentObject == null)
            yield return null;

        //run infintely until stopped 
        while (true)
        {
            if (ShopManager.instance != null)
            {
                //will replace with spawning bubbles but for now this will do 
                ShopManager.instance.AddMoney(bubbleAmount);
                GameObject newBubble = Instantiate(bubblePrefab, parentObject.transform.position, parentObject.transform.rotation);
            }

            yield return new WaitForSeconds(productionRate);
        }

    }

}
