using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowAndDamageTargets", menuName = "Scriptable Objects/SlowAndDamageTargets")]
public class SlowAndDamageTargets : EntityFunctions
{


    [SerializeField] private float speedModifier; 

    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        string dataKey = "MoveInDirection_speedModifier";
        CheckColliderByTag tagCollider = entity.GetTagCollider();
        List<Entity> targetList = new List<Entity>();
        for (int iter = 0; iter < tagCollider.allColliding.Count; iter++)
        {
            Entity reference = tagCollider.allColliding[iter].gameObject.GetComponent<Entity>();
            if (reference != null)
            {
                string typeName = reference.GetMaterialType().GetEntityType().ToString();
                if (typeName.StartsWith("E_"))
                    targetList.Add(reference);
            }
        }

        for (int iter = 0; iter < targetList.Count; iter++)
        {
            targetList[iter].dataLibrary.SetFloat(dataKey, speedModifier);
        }

        yield return null;  
    }
}
