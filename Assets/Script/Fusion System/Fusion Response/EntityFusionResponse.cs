using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EntityFusionResponse", menuName = "Scriptable Objects/EntityFusionResponse")]
public class EntityFusionResponse : FusionResponse<Entity>
{


    public override void HandleFusionResponse(List<Entity> outputList, List<Entity> inputToResolveList)
    {
        //assume only at least for entity only can have 1 
        if (outputList.Count > 1 || outputList.Count == 0)
            return;


        //method, figure out if the entity has a parent ( which means its in the grid ), if so, take that as the reference position to place the fused form 

        //find which one has the target location 
        Transform target = null;
        List<Entity> cleanUpList = new List<Entity>();  
        foreach (Entity entity in inputToResolveList)
        {
            //ensure that the entity data is actually in the scene 
            if (entity.gameObject.scene.IsValid())
            {
                //check if its a child, only save this value once as the grid location
                if (entity.gameObject.transform.parent != null && target == null)
                    target = entity.gameObject.transform.parent;

                //add valid scene object to be removed
                cleanUpList.Add(entity);
            }
        }


        //Place new fusion into the grid slot
        if (target != null)
        {

            // 8/10 I need to handle the grid addition properly here, rnow here its just a temp way to work it 
            Entity newEntity = Instantiate(outputList[0], target.transform.position, target.transform.rotation);
            newEntity.transform.SetParent(target.transform);

            //BROKEN 
            //clean up 
            foreach (Entity entity in cleanUpList)
            {
                Destroy(entity.gameObject);
            }
        }
    }
}
