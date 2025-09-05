using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ProduceBubbles", menuName = "Scriptable Objects/ProduceBubbles")]
public class ProduceBubbles : EntityFunctions
{
    [SerializeField] private float bubbleAmount;
    [SerializeField] private float productionRate;

    private void OnEnable()
    {
        coroutine = ExcuteCoroutine();
    }

    public override IEnumerator ExcuteCoroutine()
    {
        //run infintely until stopped 
        while (true)
        {
            if (ShopManager.instance != null)
            {
                //will replace with spawning bubbles but for now this will do 
                ShopManager.instance.AddMoney(bubbleAmount);
            }

            yield return new WaitForSeconds(productionRate);
        }

    }

}
