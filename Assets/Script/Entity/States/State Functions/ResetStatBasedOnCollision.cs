using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResetStatBasedOnCollision", menuName = "Scriptable Objects/ResetStatBasedOnCollision")]
public class ResetStatBasedOnCollision : EntityFunctions
{

    [SerializeField] private List<EntityNameList.ENTITYNAME> namesToCheckAgainst;
    [SerializeField] private string dataKey;
    [SerializeField] private float resetValue;

    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        if (entity == null)
            yield return null;

        while (true)
        {
            bool shouldReset = true;
            //Check if any whitelisted entities are still colliding with the main if so then dont reset the value 
            foreach (GameObject gameObject in entity.GetTagCollider().allColliding)
            {
                Entity reference = gameObject.GetComponent<Entity>();
                if (reference != null && shouldReset)
                {
                    foreach (EntityNameList.ENTITYNAME enumName in namesToCheckAgainst)
                    {
                        if (reference.entityName == enumName)
                        {
                            shouldReset = false;
                            break;
                        }
                    }
                }
            }

            if (shouldReset)
                entity.dataLibrary.SetFloat(dataKey, resetValue);
            yield return null;
        } 
    }
}
