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
        for (int iter = 0; iter < recipe.Count; iter++)
        {
            Entity content = recipe[iter];
            for (int j = 0; j < tempList.Count; j++)
            {
                if (tempList[j] == content)
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
