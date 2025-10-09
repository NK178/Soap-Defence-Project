using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityFusionDecision", menuName = "Scriptable Objects/EntityFusionDecision")]
public class EntityFusionDecision : FusionDecision<Entity>
{

    public override bool IsFusionValid(List<Entity> recipe, List<Entity> fuseInput)
    {
        if (recipe.Count != fuseInput.Count)
            return false;

        List<Entity> tempList = new List<Entity>();
        for (int iter = 0; iter < fuseInput.Count; iter++)
        {
            tempList.Add(fuseInput[iter]);
        }


        //not efficent but it is what it is for now 
        for (int i = 0; i < recipe.Count; i++)
        {
            for (int j = 0; j < tempList.Count; j++)
            {
                if (tempList[j].entityName == recipe[i].entityName)
                {
                    tempList.Remove(tempList[j]);
                    break;
                }
            }
        }

        //if all items are gone then valid fuse
        if (tempList.Count > 0)
            return false;
        else
            return true;

    }
}
